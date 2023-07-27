// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Damage, Position, Player, Health, GameState, Bounds } from "../codegen/Tables.sol";
import { Road, Pavement, Move, State, Carrying, Rock, Tree, Bones, Name } from "../codegen/Tables.sol";
import { PositionTableId, PositionData } from "../codegen/Tables.sol";
import { RoadState, RockType, MoveType, StateType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions, withinManhattanDistance, withinChessDistance, getDistance, withinManhattanMinimum } from "../utility/grid.sol";
import { MapSystem } from "../systems/MapSystem.sol";
import { RoadSystem } from "../systems/RoadSystem.sol";

contract MoveSystem is System {
  //push
  function push(int32 x, int32 y, int32 pushX, int32 pushY) public {
    IWorld world = IWorld(_world());

    bytes32 pusher = addressToEntityKey(address(_msgSender()));
    PositionData memory startPos = Position.get(pusher);

    require(world.onMap(pushX, pushY), "off grid");
    require(withinManhattanDistance(PositionData(x, y), PositionData(pushX, pushY), 1), "too far to push");

    bytes32[] memory atPush = getKeysWithValue(PositionTableId, Position.encode(x, y));

    require(atPush.length > 0, "trying to push an empty spot");
    uint32 move = Move.get(atPush[0]);
    require(move == uint32(MoveType.Push), "pushing a non-pushable object");

    bytes32[] memory atDestination = getKeysWithValue(PositionTableId, Position.encode(pushX, pushY));

    assert(atPush.length < 2);

    bool canPush = true;

    //check if we are pushing rocks into a road ditch
    if (atDestination.length > 0) {
      //check if there is an obstruction
      bool obstruction = Move.get(atDestination[0]) != 0;
      require(obstruction == false, "pushing into an occupied spot");

      uint32 roadInt = Road.get(atDestination[0]);
      //there is a road here
      if (roadInt != 0) {
        require(roadInt == uint32(RoadState.Shoveled), "Road already full");

        //this has now become a fill()
        canPush = false;
        fill(atPush[0], atDestination[0]);
      }
    } else {}

    //move push object before then setting the pusher to the new position
    if (canPush) {
      Position.set(atPush[0], pushX, pushY);
    }

    //and then player (which ovewrites where the push object was)
    Position.set(pusher, x, y);
  }

  function fill(bytes32 filler, bytes32 hole) private {
    uint32 roadInt = Road.get(hole);

    // roadInt++;
    //go directly to finished road for now
    roadInt = uint32(RoadState.Paved);
    Road.set(hole, roadInt);

    //ROAD COMPLETE!!!
    if (roadInt == uint32(RoadState.Paved)) {
      // Pavement.set(keccak256(abi.encode("Pavement", x, y)), true);
      Position.deleteRecord(filler);
      Position.deleteRecord(hole);
    }
  }

  function interact(bytes32 player, int32 x, int32 y, bytes32[] memory entities, uint distance) private returns (bool) {

    require(entities.length > 0, "empty position");

    PositionData memory playerPos = Position.get(player);
    PositionData memory entityPos = Position.get(entities[0]);

    // checks that positions are where they should be, also that the entities actually have positions
    require(withinManhattanMinimum(entityPos, playerPos, distance), "too far or too close");

    return true;
  }

  function mine(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));

    require(interact(player, x, y, atPosition, 1),"bad interact");

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
    else if (rockState == uint32(RockType.Nucleus)) {
      Move.set(atPosition[0], uint32(MoveType.Shovel));
    }
  }

  function chop(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));

    require(interact(player, x, y, atPosition, 1),"bad interact");
    require(Tree.get(atPosition[0]), "no tree");

    int32 health = Health.get(atPosition[0]);
    health--;

    if (health == 0) {
      Tree.deleteRecord(atPosition[0]);
      Position.deleteRecord(atPosition[0]);
      Health.deleteRecord(atPosition[0]);
      Move.deleteRecord(atPosition[0]);
    } else {
      Health.set(atPosition[0], health);
    }
  }

  function carry(int32 carryX, int32 carryY) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(carryX, carryY));

    // Position
    require(atPosition.length > 0, "trying to carry an empty spot");
    uint32 move = Move.get(atPosition[0]);
    require(move == uint32(MoveType.Carry), "non-carry object");

    Carrying.set(player, atPosition[0]);

    // Position.deleteRecord()
    // bytes32[] memory atPushPosition = getKeysWithValue(PositionTableId, Position.encode(pushX, pushY));
    // require(atPushPosition.length != 1, "pushing into an occupied spot");
  }

  function drop(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));

    bytes32 carrying = Carrying.get(player);
    require(carrying != 0, "Not carrying anything");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));

    if (atPosition.length == 0) {
      //drop rocks back on ground
    } else {
      //check to see if we can fill hole
    }
  }

  function shovel(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));

    IWorld world = IWorld(_world());
    require(world.onMap(x, y), "off grid");
    require(world.onRoad(x, y), "off road");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));

    require(atPosition.length < 1, "trying to dig an occupied spot");
    require(Pavement.get(keccak256(abi.encode("Pavement", x, y))) == false, "Digging on pavement");

    bytes32 roadEntity = keccak256(abi.encode("Road", x, y));

    Road.set(roadEntity, 1);
    Position.set(roadEntity, x, y);
  }

  function melee(int32 x, int32 y) public {

    bytes32 player = addressToEntityKey(address(_msgSender()));
    PositionData memory pos = PositionData(x,y);
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));
    require(atPosition.length > 0, "attacking an empty spot");
    require(withinManhattanDistance(pos, Position.get(player), 1), "too far to attack");

    int32 health = Health.get(atPosition[0]);
    require(health > 0, "this thing on?");

    health--;

    Health.set(atPosition[0], health);

    if(health <= 0) {
      kill(player, atPosition[0], pos);
    }
  }

  function kill(bytes32 attacker, bytes32 target, PositionData memory pos) private {
    Position.deleteRecord(target);
    bytes32 bonesEntity = keccak256(abi.encode("Bones", pos.x, pos.y));

    Bones.set(bonesEntity, true);
    Position.set(bonesEntity, pos);
    Move.set(bonesEntity, uint32(MoveType.Push));

  }

  function teleport(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));
    require(atPosition.length < 1, "occupied");
    Position.set(player, PositionData(x, y));
  }

  function moveFrom(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    PositionData memory startPos = Position.get(player);

    //bound to map size
    // (uint32 width, uint32 height, ) = MapConfig.get();
    // if (x >= int32(width)) {
    //   x = int32(width) - 1;
    // } else if (x < 0) {
    //   x = 0;
    // }

    // if (y >= int32(height)) {
    //   y = int32(height) - 1;
    // } else if (y < 0) {
    //   y = 0;
    // }


    require(startPos.x == x || startPos.y == y, "cannot move diagonally ");
    require(startPos.x != x || startPos.y != y, "moving in place");

    PositionData memory endPos = PositionData(x, y);
    require(getDistance(startPos,endPos) == 5, "move distance is not 5");

    // get all the positions in the line we are walking
    PositionData[] memory positions = lineWalkPositions(startPos, endPos);

    // iterate over all the positions we move over, stop at the first blockage
    //START index at 1, ignoring our own position
    for (uint i = 1; i < positions.length; i++) {

      bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(positions[i].x, positions[i].y));
      assert(atPosition.length < 2);

      //if we hit an object or at the end of our walk, move to that position
      if (atPosition.length > 0) {
        require(i > 1, "nowhere to move");
        Position.set(player, positions[i - 1]);
        return;
      } else if (i == positions.length - 1) {
        Position.set(player, positions[i]);
        return;
      }
    }

    require(false, "No available place to move");
  }

  function name(uint32 firstName, uint32 middleName, uint32 lastName) public {
    bytes32 playerEntity = addressToEntityKey(address(_msgSender()));
    (bool hasName,,,) = Name.get(playerEntity);
    require(!hasName, "already has name");
    require(firstName < 36, "first name");
    require(middleName < 1025, "middle name");
    require(lastName < 1734, "last name");
    require(!Player.get(playerEntity), "already spawned");
    Name.set(playerEntity, true, firstName, middleName, lastName);

  }

  function spawn(int32 x, int32 y) public {

    bytes32 playerEntity = addressToEntityKey(address(_msgSender()));
    require(!Player.get(playerEntity), "already spawned");

    // uint32 mileDistance = GameState.get()
    (int32 l, int32 r, int32 up, ) = Bounds.get();
    require(y > up, "spawning too low");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));
    require(atPosition.length < 1, "occupied");

    Player.set(playerEntity, true);
    Health.set(playerEntity, 3);
    Move.set(playerEntity, uint32(MoveType.Push));
    Position.set(playerEntity, x, y);

  }

  function abs(int x) private pure returns (int) {
    return x >= 0 ? x : -x;
  }
}
