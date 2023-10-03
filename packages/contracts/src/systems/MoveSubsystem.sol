// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Position, Player, Health, GameState, Bounds } from "../codegen/Tables.sol";
import { Road, Move, Action, Carrying, Rock, Tree, Bones, Name, Scroll, Seeds, Boots, Weight, Animation, NPC } from "../codegen/Tables.sol";
import { PositionTableId, PositionData } from "../codegen/Tables.sol";
import { RoadState, RockType, MoveType, ActionType, AnimationType, NPCType } from "../codegen/Types.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions, withinManhattanDistance, withinChessDistance, getDistance, withinManhattanMinimum } from "../utility/grid.sol";
import { MapSubsystem } from "./MapSubsystem.sol";
import { TerrainSubsystem } from "./TerrainSubsystem.sol";
import { RewardSubsystem } from "./RewardSubsystem.sol";
import { EntitySubsystem } from "./EntitySubsystem.sol";
// import { PackedCounter } from "@latticexyz/store/src/PackedCounter.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";

contract MoveSubsystem is System {

  function getKeysAtPosition(int32 x, int32 y, int32 layer) public view returns(bytes32[] memory) {
    // (bytes memory staticData, PackedCounter encodedLengths, bytes memory dynamicData) = Position.encode(x, y, layer);
    // return getKeysWithValue(IWorld(_world()), PositionTableId, staticData, encodedLengths, dynamicData);
    return getKeysWithValue(IWorld(_world()), PositionTableId, Position.encode(x, y, layer));
  }

  function moveSimple(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(canDoStuff(player), "hmm");

    PositionData memory startPos = Position.get(player);
    PositionData memory endPos = PositionData(x, y, 0);
    uint distance = getDistance(startPos, endPos);
    uint minDistance = uint(uint32(Boots.getMinMove(player)));
    uint maxDistance = uint(uint32(Boots.getMaxMove(player)));

    require(distance >= minDistance && distance <= maxDistance, "Boots out of range");
    require(requireLegalMove(player, startPos, endPos, maxDistance), "Bad move");

    // get all the positions in the line we are walking (including starting position)
    PositionData[] memory positions = lineWalkPositions(startPos, endPos);
    // iterate over all the positions we move over, stop at the first blockage
    //START index at 1, ignoring our own position

    uint walkLength = 0;
    for (uint i = 1; i < positions.length; i++) {
      
      bytes32[] memory atPosition = world.getKeysAtPosition ( positions[i].x, positions[i].y, 0);
      assert(atPosition.length < 2);

      bool continueWalk = world.onWorld(positions[i].x, positions[i].y);
      if(continueWalk && atPosition.length > 0) {
        //we can't walk into holes on purpose, only traps
        continueWalk = canWalkOn(atPosition);
      }
      
      //if we hit an object or at the end of our walk, move to that position
      if (continueWalk == false) {
        require(i > 1, "Nowhere to move");
        walkLength = i-1;
        break;
      }

    }

    //we walked the full distance without getting stopped
    if(walkLength == 0) {
      walkLength = positions.length-1;
    }

    //walk all the spaces (remember the 0 index is our original position, start at index 1)
    for (uint i = 1; i <= walkLength; i++) {
      bytes32[] memory atPosTemp = world.getKeysAtPosition ( positions[i].x, positions[i].y, 0);
      moveTo(player, player, positions[i-1], positions[i], atPosTemp, ActionType.Walking);

      //if we die while moving we must stop
      if(canDoStuff(player) == false) {
        return;
      }
    }

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
    uint32 move = Move.get(at[0]);
    return(move == uint32(MoveType.None) || move == uint32(MoveType.Trap));
  }
  function canPlaceOn(bytes32[] memory at) public view returns(bool) {
    uint32 move = Move.get(at[0]);
    return(move == uint32(MoveType.None) || move == uint32(MoveType.Hole) || move == uint32(MoveType.Trap));
  }

  function requireCanPlaceOn(bytes32[] memory at) public view {
    if(at.length == 0) return;
    uint32 move = Move.get(at[0]);
    require(canPlaceOn(at), "cannot place on");
  }

  function requireOnMap(bytes32[] memory at, PositionData memory pos) public view {
    IWorld world = IWorld(_world());
    if(at.length > 0 && Player.get(at[0])) require(world.onWorld(pos.x, pos.y), "off world");
    else {require(world.onMap(pos.x, pos.y), "off map");}
  }

  function requirePushableOrEmpty(bytes32[] memory at) public view returns(bool) {
    if(at.length == 0) return false;
    uint32 move = Move.get(at[0]);
    require(move != uint32(MoveType.Obstruction), "blocked");
    return move == uint32(MoveType.Push);
  }

  //push
  function push(bytes32 player, int32 x, int32 y) public {
    require(canDoStuff(player), "hmm");

    IWorld world = IWorld(_world());
    PositionData memory shoverPos = Position.get(player);
    PositionData memory pushPos = PositionData(x, y, 0);

    //check initial push is good
    require(canInteract(player, x, y, world.getKeysAtPosition(pushPos.x, pushPos.y, 0), 1), "bad interact");

    console.log("start push");

    //push everything until we cant, starting at the beginning
    int32 totalWeight = 0;
    uint index = 0;
    uint maxLength = 16;
    bytes32[] memory pushArray = new bytes32[](maxLength);
    PositionData memory vector = PositionData(pushPos.x - shoverPos.x, pushPos.y - shoverPos.y, 0);
    bytes32[] memory atPos = world.getKeysAtPosition(shoverPos.x, shoverPos.y, 0);

    //continue push loop as long as there is something to push
    while (atPos.length > uint(0)) {
      console.logUint(index);

      //leave loop early if we get blocked or finally reach a non-pushable space (empty or hole)
      if(requirePushableOrEmpty(atPos) == false) {
        break;
      }

      //add the shover to the push array
      require(index < maxLength, "too many");
      pushArray[index] = atPos[0];

      //weight always must be pushable, else we fail
      totalWeight += Weight.get(atPos[0]);
      require(totalWeight < 1, "too heavy");

      requireOnMap(atPos, pushPos);

      //next space
      atPos = world.getKeysAtPosition(pushPos.x, pushPos.y, 0);

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
    //DONT iterate to the 0 index (so we don't get -1 index lookup)
    for (uint i = index; i > 0; i--) {

      bytes32 shover = pushArray[i-1];
      bytes32 pushed = pushArray[i];

      moveTo(shover, pushed, shoverPos, pushPos, atPos, ActionType.Push);

      pushPos.x -= vector.x;
      pushPos.y -= vector.y;
      shoverPos.x -= vector.x;
      shoverPos.y -= vector.y;

      atPos = new bytes32[](0);

    }

    //first player to do the push pushes themselves
    moveTo(player, player, shoverPos, pushPos, atPos, ActionType.Push);

  }

  function moveTo(
    bytes32 causedBy,
    bytes32 entity,
    PositionData memory from,
    PositionData memory to,
    bytes32[] memory atDest,
    ActionType animation
  ) public {

    //there is an entity at our destination
    if (atDest.length > 0) {

      //check if there is an obstruction
      if(canPlaceOn(atDest) == false) { return; }

      //handle the move on first
      moveOn(causedBy, entity, to, atDest);

      //if we're still alive, move into the position (this will trigger an entity update too)
      if(canDoStuff(entity)) {
        setPositionData(causedBy, entity, to, animation);
      }

    } else {

      //MOVE
      setPositionData(causedBy, entity, to, animation);

    }

  }

  function moveOn(bytes32 causedBy, bytes32 entity, PositionData memory to, bytes32[] memory atDestination) private {

      //kill the player if they move onto a hole or trap
      MoveType moveType = MoveType(Move.get(atDestination[0]));
      if (moveType == MoveType.Hole || moveType == MoveType.Trap) {
        //kill if it was a player
        if(Player.get(entity)) { IWorld(_world()).kill(causedBy, entity, causedBy, to);}
        if(Road.getState(atDestination[0]) == uint32(RoadState.Shoveled)) {
          IWorld(_world()).spawnRoadFromPlayer(causedBy, entity, atDestination[0], to);
        }
      }

  }

  function canDoStuff(bytes32 player) public returns (bool) {
    //TODO add game pausing global
    if(Health.get(player) < 1) return false;
    return true;
  }

  function canInteract(
    bytes32 player,
    int32 x,
    int32 y,
    bytes32[] memory entities,
    uint distance
  ) public returns (bool) {
    require(entities.length > 0, "empty position");
    //todo check off grid?
    PositionData memory playerPos = Position.get(player);
    PositionData memory entityPos = Position.get(entities[0]);
    
    require(requireLegalMove(player, playerPos, entityPos, distance), "Bad move");

    return true;
  }

  function requireLegalMove(bytes32 player, PositionData memory from, PositionData memory to, uint distance) public returns(bool) {
    // checks that the position is below the min and maximum distance and is not diagonal
    require(from.x == to.x || from.y == to.y, "cannot move diagonally ");
    require(withinManhattanMinimum(to, from, distance), "too far or too close");
    return true;
  }

  function canInteractEmpty(
    bytes32 player,
    int32 x,
    int32 y,
    bytes32[] memory entities,
    uint distance
  ) public returns (bool) {
    require(entities.length == 0, "not empty");

    PositionData memory playerPos = Position.get(player);
    PositionData memory entityPos = PositionData(x, y, 0);

    // checks that positions are where they should be, also that the entities actually have positions
    require(withinManhattanMinimum(entityPos, playerPos, distance), "too far or too close");

    return true;
  }

  function setPosition(bytes32 causedBy, bytes32 entity, int32 x, int32 y, int32 layer, ActionType action) public {
    setPositionData(causedBy, entity, PositionData(x, y, layer), action);
  }

  function setPositionData(bytes32 causedBy, bytes32 entity, PositionData memory pos, ActionType action) public {
    IWorld(_world()).setAction(entity, action, pos.x, pos.y);
    setPositionRaw(causedBy, entity, pos);
  }

  function setPositionRaw(bytes32 causedBy, bytes32 entity, PositionData memory pos) public {
    Position.set(entity, pos);

    IWorld world = IWorld(_world());

    //only movements onto main game map update stuff
    if(pos.layer == 0) {
      
      //we have to be careful not to infinite loop here 
      //(ie. an entity moves that triggers a move that triggers a move)
      world.triggerEntities(causedBy, entity, pos);
      world.triggerPuzzles(causedBy, entity, pos);

      //TODO add atPosition argument to help speed up subsequent ticks?
    }

  }

}
