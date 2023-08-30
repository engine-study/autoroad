// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Damage, Position, Player, Health, GameState, Bounds } from "../codegen/Tables.sol";
import { Road, Move, State, Carrying, Rock, Tree, Bones, Name, Stats, Coinage, Scroll, Seeds, Boots, Weight } from "../codegen/Tables.sol";
import { PositionTableId, PositionData } from "../codegen/Tables.sol";
import { RoadState, RockType, MoveType, StateType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions, withinManhattanDistance, withinChessDistance, getDistance, withinManhattanMinimum } from "../utility/grid.sol";
import { MapSubSystem } from "../systems/MapSubSystem.sol";
import { RoadSubsystem } from "../systems/RoadSubsystem.sol";

contract MoveSubsystem is System {
  function moveSimple(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(canDoStuff(player), "hmm");

    PositionData memory startPos = Position.get(player);

    require(startPos.x == x || startPos.y == y, "cannot move diagonally ");
    require(startPos.x != x || startPos.y != y, "moving in place");

    PositionData memory endPos = PositionData(x, y, 0);
    int distance = int(getDistance(startPos, endPos));
    require(Boots.getMinMove(player) >= distance && Boots.getMaxMove(player) <= distance, "bad boots");

    // get all the positions in the line we are walking
    PositionData[] memory positions = lineWalkPositions(startPos, endPos);

    // iterate over all the positions we move over, stop at the first blockage
    //START index at 1, ignoring our own position
    for (uint i = 1; i < positions.length; i++) {
      bytes32[] memory atPosition = getKeysWithValue(
        PositionTableId,
        Position.encode(positions[i].x, positions[i].y, 0)
      );
      assert(atPosition.length < 2);

      //if we hit an object or at the end of our walk, move to that position
      if (atPosition.length > 0 || world.onWorld(positions[i].x, positions[i].y) == false) {
        require(i > 1, "nowhere to move");
        Position.set(player, positions[i - 1]);
        return;
      } else if (i == positions.length - 1) {
        Position.set(player, positions[i]);
        return;
      }
    }

    require(false, "No available place to move");

    // int32 stat = Stats.getMoves(player);
    // Stats.setMoves(player, stat + 1);
  }

  function abs(int x) private pure returns (int) {
    return x >= 0 ? x : -x;
  }

  //push
  function push(bytes32 player, int32 x, int32 y) public {
    require(canDoStuff(player), "hmm");

    IWorld world = IWorld(_world());
    PositionData memory shoverPos = Position.get(player);
    PositionData memory pushPos = PositionData(x, y, 0);

    //check initial push is good
    bytes32[] memory atPos = getKeysWithValue(PositionTableId, Position.encode(pushPos.x, pushPos.y, 0));
    require(canInteract(player, x, y, atPos, 1), "bad interact");

    console.log("start push");

    //push everything until we cant
    int32 totalWeight = Weight.get(player);
    uint256 count = 0;
    uint256 maxLength = 16;
    bytes32[] memory pushArray = new bytes32[](maxLength);
    PositionData memory pushVector = PositionData(pushPos.x - shoverPos.x, pushPos.y - shoverPos.y, 0);

    while (atPos.length > 0) {

      require(count < maxLength, "too many");
      uint32 move = Move.get(atPos[0]);

      //leave loop early if we get blocked or finally reach an empty space
      require(move != uint32(MoveType.Obstruction), "blocked");
      if(move != uint32(MoveType.Push)) { break; }

      pushArray[count] = atPos[0];

      //we are always able to push whatever is in front of us
      totalWeight += Weight.get(atPos[0]);
      require(totalWeight < 1, "too heavy");

      //increment push and check the destination is on map
      pushPos.x += pushVector.x;
      pushPos.y += pushVector.y;
      if(Player.get(pushArray[count])) require(world.onWorld(pushPos.x, pushPos.y), "off map");
      else {require(world.onMap(pushPos.x, pushPos.y), "off world");}

      //next space
      atPos = getKeysWithValue(PositionTableId, Position.encode(pushPos.x, pushPos.y, 0));
      count++;
    }
    
    //go back to the last non empty object
    count--;

    //move the FINAL object in the push array first
    shoverPos.x = pushPos.x - pushVector.x;
    shoverPos.y = pushPos.y - pushVector.y;
    bytes32[] memory atPush = getKeysWithValue(PositionTableId, Position.encode(shoverPos.x, shoverPos.y, 0));
    moveTo(player, pushArray[count], shoverPos, pushPos, atPush, atPos);

    //iterate backwards pushing everything forward one by one with a lighter position set that doesn't check for holes
    for (int i = int(count) - 1; i >= 0; i--) {
      pushPos.x -= pushVector.x;
      pushPos.y -= pushVector.y;
      Position.set(pushArray[uint(i)], pushPos.x, pushPos.y, 0);
    }

    //move the FIRST object (the player) 
    pushPos.x -= pushVector.x;
    pushPos.y -= pushVector.y;
    Position.set(player, pushPos.x, pushPos.y, 0);

  }

  function moveTo(
    bytes32 player,
    bytes32 entity,
    PositionData memory from,
    PositionData memory to,
    bytes32[] memory atPosition,
    bytes32[] memory atDestination
  ) public {

    //check if we are pushing rocks into a road ditch
    if (atDestination.length > 0) {
      //check if there is an obstruction
      MoveType moveType = MoveType(Move.get(atDestination[0]));
      bool empty = moveType == MoveType.None || moveType == MoveType.Hole;
      require(empty, "moving into an occupied spot");

      //move into the hole
      if (moveType == MoveType.Hole) {
        if(Player.get(entity)) { Health.set(entity, -1);}
        if(Road.getState(atDestination[0]) == uint32(RoadState.Shoveled)) {
          IWorld(_world()).spawnRoadFromPlayer(player, entity, atDestination[0], to);
        }
      }

    } else {
      Position.set(entity, to.x, to.y, 0);
    }

  }

  function canDoStuff(bytes32 player) public returns (bool) {
    //TODO add game pausing global
    require(Health.get(player) > 0, "we dead");
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

    // checks that positions are where they should be, also that the entities actually have positions
    // require(player != entities[0], "???");
    require(withinManhattanMinimum(entityPos, playerPos, distance), "too far or too close");

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
      kill(player, atPosition[0], pos);
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
      kill(player, atPosition[0], pos);
    } else {
      Health.set(atPosition[0], health);
    }
  }

  function kill(bytes32 attacker, bytes32 target, PositionData memory pos) private {
    //credit attacker with kill
    // int32 stat = Stats.getKills(attacker);
    // Stats.setKills(attacker, stat + 1);

    //kill target
    Health.set(target, -1);
    Position.deleteRecord(target);

    // int32 stat2 = Stats.getDeaths(target);
    // Stats.setDeaths(target, stat2 + 1);

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
    Position.set(player, PositionData(x, y, 0));
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
    // require(canDoStuff(player), "hmm");
    // PositionData memory startPos = Position.get(player);
    // IWorld world = IWorld(_world());
    // require(world.onMap(pushX, pushY), "off grid");
    // require(withinManhattanDistance(PositionData(x, y), startPos, 3), "too far to push");
    // bytes32[] memory atPush = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));
    // require(atPush.length > 0, "trying to push an empty spot");
    // uint32 move = Move.get(atPush[0]);
    // require(move == uint32(MoveType.Push), "pushing a non-pushable object");
    // bytes32[] memory atDestination = getKeysWithValue(PositionTableId, Position.encode(pushX, pushY));
    // assert(atPush.length < 2);
    // bool canPush = true;
    // //check if we are pushing rocks into a road ditch
    // if (atDestination.length > 0) {
    //   //check if there is an obstruction
    //   bool empty = Move.get(atDestination[0]) == uint32(MoveType.None);
    //   require(empty, "pushing into an occupied spot");
    //   (uint32 roadInt, ) = Road.get(atDestination[0]);
    //   //there is a road here
    //   if (roadInt != 0) {
    //     require(roadInt == uint32(RoadState.Shoveled), "Road not shoveled");
    //     //this has now become a fill()
    //     canPush = false;
    //     fill(player, atPush[0], atDestination[0], pushX, pushY);
    //   }
    // } else {}
    // //move push object before then setting the pusher to the new position
    // if (canPush) {
    //   Position.set(atPush[0], pushX, pushY);
    // }
    // //and then player (which ovewrites where the push object was)
    // Position.set(player, x, y);
    // // int32 stat = Stats.getPushed(player);
    // // Stats.setPushed(player, stat + 1);
  }
}
