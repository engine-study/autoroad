// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Position, Player, Health } from "../codegen/index.sol";
import { Road, Move, Action, Boots, Weight, NPC } from "../codegen/index.sol";
import { PositionTableId, PositionData } from "../codegen/index.sol";
import { RoadState, MoveType, ActionName, NPCType } from "../codegen/common.sol";

import { Rules } from "../utility/rules.sol";
import { Actions } from "../utility/actions.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions, withinManhattanDistance, withinChessDistance, getDistance, withinManhattanMinimum, addPosition } from "../utility/grid.sol";
import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";

contract MoveSubsystem is System {

  //never call directly without requiring canInteract() and Rules.canDoStuff()

  function moveOrPush(bytes32 causedBy, bytes32 player, PositionData memory startPos, PositionData memory vector, int32 distance) public {
      IWorld world = IWorld(_world());
      bytes32[] memory atNext = Rules.getKeysAtPosition(world,startPos.x + vector.x, startPos.y + vector.y, 0);
      if(atNext.length > 0 && Rules.isPushable(atNext[0])) { doPush(causedBy, player, startPos, vector); } 
      else { doMoveDistance(causedBy, player, startPos, vector, distance); }
      
  }

  function moveSimple(bytes32 player, int32 x, int32 y) public {

    PositionData memory playerPos = Position.get(player);
    PositionData memory toPos = PositionData(x,y,0);

    doMoveWithBoots(player, player, Position.get(player), PositionData(toPos.x - playerPos.x, toPos.y - playerPos.y, 0));
  }

  function moveSimpleDistance(bytes32 player, int32 x, int32 y, int32 distance) public {

    int32 maxDistance = Boots.getMaxMove(player);
    require(distance <= maxDistance, "Boots out of range");

    PositionData memory playerPos = Position.get(player);
    PositionData memory toPos = PositionData(x,y,0);

    doMoveDistance(player, player, Position.get(player), PositionData(toPos.x - playerPos.x, toPos.y - playerPos.y, 0), distance);
  }

  function doMoveWithBoots(bytes32 causedBy, bytes32 player, PositionData memory startPos, PositionData memory vector) private {

    int32 minDistance = Boots.getMinMove(player);
    int32 maxDistance = Boots.getMaxMove(player);
    if(minDistance == 0 && maxDistance == 0) { minDistance = 1; maxDistance = 1;}
    
    doMoveDistance(causedBy, player, startPos, vector, minDistance);
  }

  function doMoveDistance(bytes32 causedBy, bytes32 entity, PositionData memory startPos, PositionData memory vector, int32 distance) private {

    int32 remainingMoves = distance;
    PositionData memory endPos = PositionData(startPos.x + (vector.x * distance), startPos.y + (vector.y * distance), 0);
    
    require(Rules.requireLegalMove(entity, startPos, endPos, uint256(uint32(distance))), "Bad move");

    // get all the positions in the line we are walking (including starting position)
    PositionData memory movePos = startPos;
    PositionData memory nextPos = addPosition(movePos, vector);

    // iterate over all the positions we move over, stop at the first blockage
    //START index at 1, ignoring our own position

    while(remainingMoves > 0) {

      bytes32[] memory atDest = Rules.getKeysAtPosition(IWorld(_world()), nextPos.x, nextPos.y, 0);
      require(atDest.length < 2, "double position found");

      //check if valid position
      bool canWalk = Rules.onMapOrSpawn(entity, nextPos);

      //don't let entities walk into holes themselves, need someone else to force them into holes
      //if we hit an object or at the end of our walk, break out
      if(canWalk && atDest.length > 0) {
          MoveType moveType = MoveType(Move.get(atDest[0]));
          canWalk = causedBy == entity ? Rules.canWalkOn(moveType) : Rules.canPlaceOn(moveType);
      }

      if (!canWalk) { return; }

      moveTo(causedBy, entity, movePos, nextPos, atDest, ActionName.Walking);

      //if we die while moving we must stop
      if(Rules.canDoStuff(entity) == false) { return; }

      //update where we are (moveTo can cause positions to shift other than where we walked)
      movePos = Position.get(entity);
      nextPos = addPosition(movePos, vector);
      remainingMoves--;
    }
  }

  function doFling(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory startPos, PositionData memory endPos, ActionName actionType) public {
    IWorld world = IWorld(_world());
    bytes32[] memory atDest = Rules.getKeysAtPosition(world, endPos.x, endPos.y, 0);
    bool canFling = Rules.onMapOrSpawn(target, endPos) && MoveType(Move.get(target)) == MoveType.Push;
    
    if(canFling) { 
      Actions.setActionTargeted(entity, actionType, startPos.x, startPos.y, target);
      moveTo(causedBy, target, startPos, endPos, atDest, ActionName.Hop); 
    }
  }

  function push(bytes32 player, int32 x, int32 y) public {
        
    PositionData memory playerPos = Position.get(player);
    PositionData memory toPos = PositionData(x,y,0);

    require(Rules.canDoStuff(player), "hmm");
    require(Rules.canInteract(player, playerPos, Rules.getKeysAtPosition(IWorld(_world()), x, y, 0), 1), "bad interact");

    doPush(player, player, playerPos, PositionData(toPos.x - playerPos.x, toPos.y - playerPos.y,0));
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
    bytes32[] memory atPos = Rules.getKeysAtPosition(world,shoverPos.x, shoverPos.y, 0);

    //continue push loop as long as there is something to push
    while (atPos.length > 0) {
      console.logUint(index);

      //leave loop early if we get blocked or finally reach a non-pushable space (empty or hole)
      if(Rules.isPushableNotEmpty(atPos) == false) {
        break;
      }

      //add the shover to the push array
      require(index < maxLength, "too many");
      pushArray[index] = atPos[0];

      //weight always must be pushable, else we fail
      totalWeight += Weight.get(atPos[0]);
      if(!(totalWeight < 1)) {console.log("TOO HEAVY"); return;}

      //don't let anything go off map
      Rules.requireOnMap(atPos[0], pushPos);

      //next space
      atPos = Rules.getKeysAtPosition(world,pushPos.x, pushPos.y, 0);

      //increment push and check the destination is on map
      index++;
      shoverPos.x += vector.x;
      shoverPos.y += vector.y;
      pushPos.x += vector.x;
      pushPos.y += vector.y;

    }

    if(!(index > uint(1))) {console.log("NO PUSHABLE OBJECT"); return;}
    
    //go back to the last non empty object
    index--;
    pushPos.x -= vector.x;
    pushPos.y -= vector.y;
    shoverPos.x -= vector.x;
    shoverPos.y -= vector.y;

    //iterate backwards pushing everything forward one by one with a lighter position set that doesn't check for holes
    //DONT iterate to the 0 index (so we don't get -1 index lookup on the pushArray)
    for (int i = int(index); i >= 0; i--) {

      bytes32 pushed = pushArray[uint(i)];

      //recalculate at position everytime
      atPos = Rules.getKeysAtPosition(world, pushPos.x, pushPos.y, 0);
      moveTo(causer, pushed, shoverPos, pushPos, atPos, ActionName.Push);

      pushPos.x -= vector.x;
      pushPos.y -= vector.y;
      shoverPos.x -= vector.x;
      shoverPos.y -= vector.y;

    }

  }

  function moveTo(
    bytes32 causedBy,
    bytes32 entity,
    PositionData memory from,
    PositionData memory to,
    bytes32[] memory atDest,
    ActionName actionType
  ) public {

    //simple move, no terrain at destination, exit out of method early
    if(atDest.length == 0) {
      setPosition(causedBy, entity, to, actionType);
    } else {
      
      //move onto a MoveType
      MoveType moveTypeAtDest = MoveType(Move.get(atDest[0]));

      //we soft fail bad moves and exit out of them
      if(actionType != ActionName.Hop && Rules.canPlaceOn(moveTypeAtDest) == false) {return;}

      //check if we survive the move through terrain then move
      bool canMove = handleMoveType(causedBy, entity, to, atDest[0], moveTypeAtDest, actionType);
      if(canMove && Rules.canDoStuff(entity)) {
        setPosition(causedBy, entity, to, actionType);
      } else {
        //maybe set action to idle here to show rotation?
        // Actions.setAction(entity, action, pos.x, pos.y);
      }

    }
  }

  function handleMoveType(bytes32 causedBy, bytes32 entity, PositionData memory to, bytes32 atDest, MoveType moveTypeAtDest, ActionName actionType) public returns(bool) {
    IWorld world = IWorld(_world());

    //kill or destroy whatever fell into the hole
    if(moveTypeAtDest == MoveType.Hole) {

      //spawn road, move pushed thing to under road
      if(Road.getState(atDest) == uint32(RoadState.Shoveled)) {
        SystemSwitch.call(abi.encodeCall(world.spawnRoadFromPush, (causedBy, entity, atDest, to)));
      }

      //kill
      Actions.setAction(entity, actionType, to.x, to.y);
      SystemSwitch.call(abi.encodeCall(world.destroy, (causedBy, entity, causedBy, to)));
      return false;

    } else if(moveTypeAtDest == MoveType.Trap) {

      //kill if it was an NPC
      if(NPC.get(entity) > 0) { 
        Actions.setAction(entity, actionType, to.x, to.y);
        SystemSwitch.call(abi.encodeCall(world.kill, (causedBy, entity, causedBy, to)));
        return false;
      } else {
        //otherwise trap is destroyed with no effect to pushed object
        SystemSwitch.call(abi.encodeCall(world.destroy, (causedBy, atDest, causedBy, to)));
      }
      
    } else if(actionType == ActionName.Hop) {

      if(Rules.canCrush(moveTypeAtDest) && Rules.canSquish(entity, atDest)) {
        SystemSwitch.call(abi.encodeCall(world.destroy, (causedBy, atDest, entity, to)));
      } else {
        return false;
      }
      
    }

    return true;

  }

  function setPosition(bytes32 causedBy, bytes32 entity, PositionData memory pos, ActionName action) public {

    //we move
    Position.set(entity, pos);
    Actions.setAction(entity, action, pos.x, pos.y);

    //only movements onto main game map update stuff
    if(pos.layer != 0) { return; }
      
    causeEffects(causedBy, entity, pos, action);

  }

  function causeEffects(bytes32 causedBy, bytes32 entity, PositionData memory pos, ActionName action) public {
    IWorld world = IWorld(_world());

    //this should be in handle move really..
    SystemSwitch.call(abi.encodeCall(world.movementTriggers, (causedBy, entity, pos)));
    //trigger all local entities effect by the move
    SystemSwitch.call(abi.encodeCall(world.movementTicks, (causedBy, entity, pos)));

  }

  function teleport(bytes32 player, int32 x, int32 y, ActionName actionType) public {
    require(Rules.canDoStuff(player), "hmm");

    IWorld world = IWorld(_world());
    require(Rules.onWorld(x, y), "offworld");

    PositionData memory startPos = Position.get(player);
    PositionData memory endPos = PositionData(x, y, 0);
    
    bytes32[] memory atPos = Rules.getKeysAtPosition(world,x, y, 0);
    Rules.requireCanPlaceOn(atPos);

    moveTo(player, player, startPos, endPos, atPos, actionType);

  }


}
