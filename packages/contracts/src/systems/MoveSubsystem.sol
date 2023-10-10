// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Position, Player, Health } from "../codegen/Tables.sol";
import { Road, Move, Action, Boots, Weight, Animation, NPC } from "../codegen/Tables.sol";
import { PositionTableId, PositionData } from "../codegen/Tables.sol";
import { RoadState, MoveType, ActionType, NPCType } from "../codegen/Types.sol";

import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions, withinManhattanDistance, withinChessDistance, getDistance, withinManhattanMinimum } from "../utility/grid.sol";

import { MapSubsystem } from "./MapSubsystem.sol";
import { TerrainSubsystem } from "./TerrainSubsystem.sol";
import { EntitySubsystem } from "./EntitySubsystem.sol";

contract MoveSubsystem is System {
  
  function canDoStuff(bytes32 player) public returns (bool) {
    //TODO add game pausing global
    if(Health.get(player) < 1) return false;
    return true;
  }

  function canInteract(
    bytes32 player,
    PositionData memory playerPos,
    bytes32[] memory entities,
    uint distance
  ) public returns (bool) {
    require(entities.length > 0, "empty position");
    //todo check off grid?
    //TODO this should be a parameter, not get()
    PositionData memory entityPos = Position.get(entities[0]);
    require(requireLegalMove(player, playerPos, entityPos, distance), "Bad move");

    return true;
  }

  function abs(int x) private pure returns (int) {
    return x >= 0 ? x : -x;
  }


  function requirePushable(bytes32[] memory at) public view {
    require(at.length > 0, "empty");
    uint32 move = Move.get(at[0]);
    require(move == uint32(MoveType.Push), "not push");
  }


  function canWalkOn(bytes32[] memory at) public view returns(bool) {
    if(at.length == 0) return true;
    uint32 move = Move.get(at[0]);
    return(move == uint32(MoveType.None) || move == uint32(MoveType.Trap));
  }

  function canPlaceOn(MoveType moveAt) public view returns(bool) {
    return(moveAt == MoveType.None || moveAt == MoveType.Hole || moveAt == MoveType.Trap);
  }

  function requireCanPlaceOn(bytes32[] memory at) public view {
    if(at.length == 0) return;
    MoveType move = MoveType(Move.get(at[0]));
    require(canPlaceOn(move), "cannot place on");
  }

  function requireOnMap(bytes32 at, PositionData memory pos) public view {
    require(onMapOrSpawn(at,pos), "off world");
  }

  function onMapOrSpawn(bytes32 at, PositionData memory pos) public view returns(bool) {
    IWorld world = IWorld(_world());
    if(Player.get(at)) {return world.onWorld(pos.x, pos.y);}
    else {return world.onMap(pos.x, pos.y);}
  }

  function requireEmptyOrPushable(bytes32[] memory at) public view returns(bool) {
    if(at.length == 0) return false;
    uint32 move = Move.get(at[0]);
    require(move != uint32(MoveType.Obstruction), "blocked");
    return move == uint32(MoveType.Push);
  }


  function requireLegalMove(bytes32 player, PositionData memory from, PositionData memory to, uint distance) public returns(bool) {
    // checks that the position is below the min and maximum distance and is not diagonal
    require(from.x == to.x || from.y == to.y, "cannot move diagonally ");
    require(withinManhattanMinimum(to, from, distance), "too far or too close");
    return true;
  }

  function canInteractEmpty(
    bytes32 player,
    PositionData memory playerPos,
    PositionData memory entityPos,
    bytes32[] memory entities,
    uint distance
  ) public returns (bool) {
    require(entities.length == 0, "not empty");
    // checks that positions are where they should be, also that the entities actually have positions
    require(requireLegalMove(player, entityPos, playerPos, distance), "too far or too close");
    return true;
  }

  function moveSimple(bytes32 player, int32 x, int32 y) public {

    IWorld world = IWorld(_world());
    require(canDoStuff(player), "hmm");

    PositionData memory playerPos = Position.get(player);
    PositionData memory toPos = PositionData(x,y,0);

    doMoveWithBoots(player, player, Position.get(player), PositionData(toPos.x - playerPos.x, toPos.y - playerPos.y, 0));
  }

  //never call directly without requiring canInteract() and canDoStuff()
  function moveOrPush(bytes32 causedBy, bytes32 player, PositionData memory startPos, PositionData memory vector) public {

      bytes32[] memory atNext = getKeysWithValue(PositionTableId, Position.encode(startPos.x + vector.x, startPos.y + vector.y, 0));
      if(atNext.length == 0) { doMoveWithBoots(causedBy, player, startPos, vector);} 
      else { doPush(causedBy, player, startPos, vector); }
      
  }

  function doMoveWithBoots(bytes32 causedBy, bytes32 player, PositionData memory startPos, PositionData memory vector) private {
    IWorld world = IWorld(_world());

    int32 minDistance = Boots.getMinMove(player);
    int32 maxDistance = Boots.getMaxMove(player);
    if(minDistance == 0 && maxDistance == 0) { minDistance = 1; maxDistance = 1;}

    PositionData memory endPos = PositionData(startPos.x + (vector.x * maxDistance), startPos.y + (vector.y * maxDistance), 0);

    uint distance = getDistance(startPos, endPos);
    require(distance >= uint(uint32(minDistance)) && distance <= uint(uint32(maxDistance)), "Boots out of range");
    require(requireLegalMove(player, startPos, endPos, uint(uint32(maxDistance))), "Bad move");
    
    doMove(causedBy, player, startPos, endPos);
  }

  function doMove(bytes32 causedBy, bytes32 player, PositionData memory startPos, PositionData memory endPos) private {
    IWorld world = IWorld(_world());

    // get all the positions in the line we are walking (including starting position)
    PositionData[] memory positions = lineWalkPositions(startPos, endPos);

    // iterate over all the positions we move over, stop at the first blockage
    //START index at 1, ignoring our own position

    for (uint i = 1; i < positions.length; i++) {
  
      bytes32[] memory atDest = getKeysWithValue( PositionTableId, Position.encode(positions[i].x, positions[i].y, 0));
      assert(atDest.length < 2);

      //check if valid position
      bool canWalk = onMapOrSpawn(player, positions[i]) && canWalkOn(atDest);

      //if we hit an object or at the end of our walk, break out
      if (canWalk == false) { require(i > 1, "Nowhere to move"); return;}

      moveTo(causedBy, player, positions[i-1], positions[i], atDest, ActionType.Walking);

      //if we die while moving we must stop
      if(canDoStuff(player) == false) { return;}
    }

  }

  function push(bytes32 player, int32 x, int32 y) public {
        
    PositionData memory playerPos = Position.get(player);
    PositionData memory toPos = PositionData(x,y,0);

    require(canDoStuff(player), "hmm");
    require(canInteract(player, playerPos, getKeysWithValue(PositionTableId, Position.encode(x, y, 0)), 1), "bad interact");

    doPush(player, player, playerPos, PositionData(toPos.x - playerPos.x,toPos.y - playerPos.y,0));
  }

  //push
  function doPush(bytes32 causedBy, bytes32 player, PositionData memory shoverPos, PositionData memory vector) private {
    bytes32 causer = causedBy;
    IWorld world = IWorld(_world());

    console.log("start push");

    //push everything until we cant, starting at the beginning
    int32 totalWeight = 0;
    uint index = 0;
    uint maxLength = 16;
    bytes32[] memory pushArray = new bytes32[](maxLength);
    PositionData memory pushPos = PositionData(shoverPos.x + vector.x, shoverPos.y + vector.y, 0);
    bytes32[] memory atPos = getKeysWithValue(PositionTableId, Position.encode(shoverPos.x, shoverPos.y, 0));

    //continue push loop as long as there is something to push
    while (atPos.length > 0) {
      console.logUint(index);

      //leave loop early if we get blocked or finally reach a non-pushable space (empty or hole)
      if(requireEmptyOrPushable(atPos) == false) {
        break;
      }

      //add the shover to the push array
      require(index < maxLength, "too many");
      pushArray[index] = atPos[0];

      //weight always must be pushable, else we fail
      totalWeight += Weight.get(atPos[0]);
      require(totalWeight < 1, "too heavy");

      //don't let anything go off map
      requireOnMap(atPos[0], pushPos);

      //next space
      atPos = getKeysWithValue(PositionTableId, Position.encode(pushPos.x, pushPos.y, 0));

      //increment push and check the destination is on map
      index++;
      shoverPos.x += vector.x;
      shoverPos.y += vector.y;
      pushPos.x += vector.x;
      pushPos.y += vector.y;

    }

    require(index > uint(1), "no pushable object");
    
    //go back to the last non empty object
    index--;
    pushPos.x -= vector.x;
    pushPos.y -= vector.y;
    shoverPos.x -= vector.x;
    shoverPos.y -= vector.y;

    //iterate backwards pushing everything forward one by one with a lighter position set that doesn't check for holes
    //DONT iterate to the 0 index (so we don't get -1 index lookup on the pushArray)
    for (uint i = index; i > 0; i--) {

      bytes32 shover = pushArray[i-1];
      bytes32 pushed = pushArray[i];

      //recalculate at position everytime
      atPos = getKeysWithValue(PositionTableId, Position.encode(pushPos.x, pushPos.y, 0));
      moveTo(causer, pushed, shoverPos, pushPos, atPos, ActionType.Push);

      pushPos.x -= vector.x;
      pushPos.y -= vector.y;
      shoverPos.x -= vector.x;
      shoverPos.y -= vector.y;

      atPos = new bytes32[](0);

    }

    //first player to do the push pushes themselves
    moveTo(causer, player, shoverPos, pushPos, atPos, ActionType.Push);

  }

  function moveTo(
    bytes32 causedBy,
    bytes32 entity,
    PositionData memory from,
    PositionData memory to,
    bytes32[] memory atDest,
    ActionType animation
  ) public {

    //simple move, no terrain at destination, exit out of method early
    if(atDest.length == 0) {
      setPosition(causedBy, entity, to, animation);
    } else {
      
      //move onto a MoveType
      MoveType moveTypeAtDest = MoveType(Move.get(atDest[0]));

      //we soft fail bad moves, don't require this to work
      if(canPlaceOn(moveTypeAtDest) == false) {return;}

      handleMoveType(causedBy, entity, to, atDest, moveTypeAtDest);

      //if we're still alive, move into the position (this will trigger an entity update too)
      if(canDoStuff(entity)) {
        setPosition(causedBy, entity, to, animation);
      }

    }
  }

  function handleMoveType(bytes32 causedBy, bytes32 entity, PositionData memory to, bytes32[] memory atDest, MoveType moveTypeAtDest) private {
    IWorld world = IWorld(_world());

    if(moveTypeAtDest == MoveType.Hole) {
      
      //kill if it was an NPC
      if(NPC.get(entity) > 0) { 
        world.kill(causedBy, entity, causedBy, to);
      }

      //spawn road, move pushed thing to under road
      if(Road.getState(atDest[0]) == uint32(RoadState.Shoveled)) {
        IWorld(_world()).spawnRoadFromPlayer(causedBy, entity, atDest[0], to);
      }

    } else if(moveTypeAtDest == MoveType.Trap) {
      if(NPC.get(entity) > 0) { 
        //kill if it was an NPC
        world.kill(causedBy, entity, causedBy, to);
      } else {
        //otherwise trap is destroyed with no effect
        Position.deleteRecord(atDest[0]);
      }
    }
  }

  function setPosition(bytes32 causedBy, bytes32 entity, PositionData memory pos, ActionType action) public {

    IWorld world = IWorld(_world());

    //game will get the action movement type before we move
    world.setAction(entity, action, pos.x, pos.y);

    //we move
    Position.set(entity, pos);

    //only movements onto main game map update stuff
    if(pos.layer != 0) { return; }
      
    //we have to be careful not to infinite loop here 
    //(ie. an entity moves that triggers a move that triggers a move)

    world.triggerEntities(causedBy, entity, pos);
    world.triggerPuzzles(causedBy, entity, pos);

  }


  function teleport(bytes32 player, int32 x, int32 y) public {
    require(canDoStuff(player), "hmm");

    IWorld world = IWorld(_world());
    require(world.onWorld(x, y), "offworld");

    PositionData memory startPos = Position.get(player);
    PositionData memory endPos = PositionData(x, y, 0);
    bytes32[] memory atPos = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));
    moveTo(player, player, startPos, endPos, atPos, ActionType.Teleport);
  }


}
