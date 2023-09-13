// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Position, Player, Health, GameState, Bounds } from "../codegen/Tables.sol";
import { Road, Move, Action, Carrying, Rock, Tree, Bones, Name, Stats, Coinage, Scroll, Seeds, Boots, Weight, Animation } from "../codegen/Tables.sol";
import { PositionTableId, PositionData } from "../codegen/Tables.sol";
import { RoadState, RockType, MoveType, ActionType, AnimationType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions, withinManhattanDistance, withinChessDistance, getDistance, withinManhattanMinimum } from "../utility/grid.sol";
import { MapSubsystem } from "./MapSubsystem.sol";
import { RoadSubsystem } from "./RoadSubsystem.sol";
import { EntitySubsystem } from "./EntitySubsystem.sol";

contract MoveSubsystem is System {

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

    uint walkSpaces = 0;
    for (uint i = 1; i < positions.length; i++) {
      
      bytes32[] memory atPosition = getKeysWithValue( PositionTableId, Position.encode(positions[i].x, positions[i].y, 0));
      assert(atPosition.length < 2);

      bool stopWalk = world.onWorld(positions[i].x, positions[i].y) == false;
      if(!stopWalk && atPosition.length > 0) {
        //we can't walk into holes on purpose, only traps
        MoveType move = MoveType(Move.get(atPosition[0]));
        stopWalk = move == MoveType.Obstruction || move == MoveType.Hole;
      }
      
      //if we hit an object or at the end of our walk, move to that position
      if (stopWalk) {
        require(i > 1, "Nowhere to move");
        walkSpaces = i-1;
        break;
      }

    }

    //we walked the full distance without getting stopped
    if(walkSpaces == 0) {
      walkSpaces = positions.length-1;
    }

    for (uint i = 1; i <= walkSpaces; i++) {
      bytes32[] memory atPosTemp = getKeysWithValue( PositionTableId, Position.encode(positions[i].x, positions[i].y, 0));
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

  function requireCanPlaceOn(bytes32[] memory at) public view {
    if(at.length == 0) return;
    uint32 move = Move.get(at[0]);
    require(move == uint32(MoveType.None) || move == uint32(MoveType.Hole) || move == uint32(MoveType.Trap), "blocked");
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
    require(canInteract(player, x, y, getKeysWithValue(PositionTableId, Position.encode(pushPos.x, pushPos.y, 0)), 1), "bad interact");

    console.log("start push");

    //push everything until we cant, starting at the beginning
    int32 totalWeight = 0;
    uint index = 0;
    uint maxLength = 16;
    bytes32[] memory pushArray = new bytes32[](maxLength);
    PositionData memory vector = PositionData(pushPos.x - shoverPos.x, pushPos.y - shoverPos.y, 0);
    bytes32[] memory atPos = getKeysWithValue(PositionTableId, Position.encode(shoverPos.x, shoverPos.y, 0));

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
    bytes32 moveCausedBy,
    bytes32 entity,
    PositionData memory from,
    PositionData memory to,
    bytes32[] memory atDest,
    ActionType animation
  ) public {

    //there is an entity at our destination
    if (atDest.length > 0) {

      //check if there is an obstruction
      requireCanPlaceOn(atDest);

      //handle the move on first
      moveOn(moveCausedBy, entity, to, atDest);

      //if we're still alive, move into the position (this will trigger an entity update too)
      if(canDoStuff(entity)) {
        setPositionData(entity, to, animation);
      }

    } else {

      //MOVE
      setPositionData(entity, to, animation);

    }

  }

  function moveOn(bytes32 moveCausedBy, bytes32 entity, PositionData memory to, bytes32[] memory atDestination) public {

      //kill the player if they move onto a hole or trap
      MoveType moveType = MoveType(Move.get(atDestination[0]));
      if (moveType == MoveType.Hole || moveType == MoveType.Trap) {
        //kill if it was a player
        if(Player.get(entity)) { kill(entity, moveCausedBy, to);}
        if(Road.getState(atDestination[0]) == uint32(RoadState.Shoveled)) {
          IWorld(_world()).spawnRoadFromPlayer(moveCausedBy, entity, atDestination[0], to);
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

  function mine(bytes32 player, int32 x, int32 y) public {
    require(canDoStuff(player), "hmm");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));

    require(canInteract(player, x, y, atPosition, 1), "bad interact");

    uint32 rockState = Rock.get(atPosition[0]);

    require(rockState > uint32(RockType.None), "Rock not found or none");
    require(rockState < uint32(RockType.Nucleus), "Rock ground to a pulp");

    //increment the rock state
    rockState += 1;

    Rock.set(atPosition[0], rockState);

    //give rocks that are mined a pushable component
    if (rockState == uint32(RockType.Statumen)) {
      Move.set(atPosition[0], uint32(MoveType.Push));
    }
    //become shovelable once we are broken down enough
    else if (rockState == uint32(RockType.Rudus)) {
      Position.deleteRecord(atPosition[0]);
      // Move.set(atPosition[0], uint32(MoveType.Shovel));
    }

    // int32 stat = Stats.getMined(player);
    // Stats.setMined(player, stat + 1);
  }

  function shovel(bytes32 player, int32 x, int32 y) public {
    require(canDoStuff(player), "hmm");
    IWorld world = IWorld(_world());
    require(world.onRoad(x, y), "off road");
    require(withinManhattanDistance(PositionData(x, y, 0), Position.get(player), 1), "too far");

    world.spawnShoveledRoad(player, x,y);

  }

  function stick(bytes32 player, int32 x, int32 y) public {
    require(canDoStuff(player), "hmm");

    PositionData memory pos = PositionData(x, y, 0);
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));
    require(atPosition.length > 0, "attacking an empty spot");
    require(withinManhattanDistance(pos, Position.get(player), 1), "too far to attack");

    int32 health = Health.get(atPosition[0]);
    require(health > 0, "this thing on?");

    health--;

    if (health <= 0) {
      kill(atPosition[0], player, pos);
    } else {
      Health.set(atPosition[0], health);
    }
  }

  function melee(bytes32 player, int32 x, int32 y) public {
    require(canDoStuff(player), "hmm");

    PositionData memory pos = PositionData(x, y, 0);
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));
    require(atPosition.length > 0, "attacking an empty spot");
    require(withinManhattanDistance(pos, Position.get(player), 1), "too far to attack");

    int32 health = Health.get(atPosition[0]);
    require(health > 0, "this thing on?");

    health--;

    if (health <= 0) {
      kill(atPosition[0], player, pos);
    } else {
      Health.set(atPosition[0], health);
    }
  }

  function kill(bytes32 target, bytes32 attacker, PositionData memory pos) public {

    //kill target
    Health.set(target, -1);
    Position.deleteRecord(target);

    //set to dead
    IWorld(_world()).setAction(target, ActionType.Dead, pos.x, pos.y);

    //spawn bones
    // bytes32 bonesEntity = keccak256(abi.encode("Bones", pos.x, pos.y));
    // Bones.set(bonesEntity, true);
    // Position.set(bonesEntity, pos);
    // Move.set(bonesEntity, uint32(MoveType.Push));
  }

  function teleportScroll(bytes32 player, int32 x, int32 y) public {
    require(canDoStuff(player), "hmm");

    //remove scrolls
    uint32 scrolls = Scroll.get(player);
    require(scrolls > uint32(0), "not enough scrolls");
    Scroll.set(player, scrolls - 1);

    teleport(player, x, y);

  }

  function teleport(bytes32 player, int32 x, int32 y) public {
    require(canDoStuff(player), "hmm");

    IWorld world = IWorld(_world());
    require(world.onWorld(x, y), "offworld");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));
    require(atPosition.length < 1, "occupied");
    setPosition(player, x, y, 0, ActionType.Teleport);
  }

  // function carry(int32 carryX, int32 carryY) public {
  //   require(canDoStuff(player), "hmm");

  //   bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(carryX, carryY, 0));

  //   // Position
  //   require(atPosition.length > 0, "trying to carry an empty spot");
  //   uint32 move = Move.get(atPosition[0]);
  //   require(move == uint32(MoveType.Carry), "non-carry object");

  //   Carrying.set(player, atPosition[0]);

  //   // Position.deleteRecord()
  //   // bytes32[] memory atPushPosition = getKeysWithValue(PositionTableId, Position.encode(pushX, pushY));
  //   // require(atPushPosition.length != 1, "pushing into an occupied spot");
  // }

  // function drop(int32 x, int32 y) public {
  //   bytes32 player = addressToEntityKey(address(_msgSender()));
  //   require(canDoStuff(player), "hmm");

  //   bytes32 carrying = Carrying.get(player);
  //   require(carrying != 0, "Not carrying anything");

  //   bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));

  //   if (atPosition.length == 0) {
  //     //drop rocks back on ground
  //   } else {
  //     //check to see if we can fill hole
  //   }
  // }

  function fish(bytes32 player, int32 x, int32 y) public {
    require(canDoStuff(player), "hmm");
    IWorld world = IWorld(_world());

    PositionData memory startPos = Position.get(player);
    PositionData memory fishPos = PositionData(x, y, 0);

    //check initial push is good
    bytes32[] memory atPos = getKeysWithValue(PositionTableId, Position.encode(fishPos.x, fishPos.y, 0));
    require(canInteract(player, x, y, atPos, 1), "bad interact");
    require(Weight.get(atPos[0]) <= 0, "too heavy");
    

    PositionData memory vector = PositionData(startPos.x - fishPos.x, startPos.y - fishPos.y, 0);
    PositionData memory endPos = PositionData(startPos.x + vector.x, startPos.y + vector.y, 0);
    bytes32[] memory atDest = getKeysWithValue(PositionTableId, Position.encode(endPos.x, endPos.y, 0));

    requirePushable(atPos);
    requireOnMap(atDest, endPos);
    requireCanPlaceOn(atDest);
    moveTo(player, atPos[0], startPos, endPos, atDest, ActionType.Hop);
  }

  function setPosition(bytes32 player, int32 x, int32 y, int32 layer, ActionType action) public {
    setPositionData(player, PositionData(x, y, layer), action);
  }

  function setPositionData(bytes32 player, PositionData memory pos, ActionType action) public {
    IWorld(_world()).setAction(player, action, pos.x, pos.y);
    setPositionRaw(player, pos);
  }

  function setPositionRaw(bytes32 player, PositionData memory pos) public {
    Position.set(player, pos);

    //we have to be careful not to infinite loop here
    IWorld(_world()).triggerEntities(player, pos);
  }

  // function animation(bytes32 player, AnimationType anim) public {
  //   Animation.emitEphemeral(player, uint32(anim));
  // }

}
