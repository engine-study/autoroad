// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Damage, Position, Pushable,  Player, Health } from "../codegen/Tables.sol";
import { Road, Pavement, Move, State, Carrying, Obstruction } from "../codegen/Tables.sol";
import { PushableTableId, PositionTableId, PositionData} from "../codegen/Tables.sol";
import { RoadState, MoveType, StateType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions } from "../utility/grid.sol";
import { MapSystem } from "../systems/MapSystem.sol";
import { RoadSystem } from "../systems/RoadSystem.sol";

contract MoveSystem is System {


  //push
  function push(int32 x, int32 y, int32 pushX, int32 pushY) public {
    IWorld world = IWorld(_world());

    bytes32 player = addressToEntityKey(address(_msgSender()));
    PositionData memory startPos = Position.get(player);

    require(world.onMap(x, y), "off grid");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));

    require(atPosition.length >= 1, "trying to push an empty spot");
    bool pushable = Pushable.get(atPosition[0]);
    require(pushable, "pushing a non-pushable object");

    bytes32[] memory atPushPosition = getKeysWithValue(PositionTableId, Position.encode(pushX, pushY));

    assert(atPosition.length < 2);

    //check if we are pushing rocks into a road ditch
    if (atPushPosition.length >= 1) {

      //check if there is an obstruction
      bool obstruction = Obstruction.get(atPushPosition[0]);
      require(obstruction == false, "pushing into an occupied spot");

      RoadState road = Road.get(atPushPosition[0]);
      uint roadInt = uint(road);
      require(roadInt < uint(RoadState.Paved), "Road state too high");

      roadInt++;
      Road.set(atPushPosition[0], RoadState(roadInt));

      //ROAD COMPLETE!!!
      if(roadInt == uint(RoadState.Paved)) {
        Pavement.set(keccak256(abi.encode("Pavement", x, y)), true);
        Position.deleteRecord(atPushPosition[0]);
      }

    } else {

    }

    //move push object first
    Position.set(atPosition[0], pushX, pushY);

    //and then player (which ovewrites where the push object was)
    Position.set(player, x, y);
  }

  function carry(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));
    require(atPosition.length >= 1, "trying to carry an empty spot");
    MoveType move = Move.get(atPosition[0]);
    require(move == MoveType.Carry, "non-carry object");

    Carrying.set(player, atPosition[0]);

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

    Road.set(roadEntity, RoadState.Shoveled);
    Position.set(roadEntity, x, y);
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
    require(startPos.x != x || startPos.y != y, "cannot move in place");

    // get all the positions in the line we are walking
    PositionData[] memory positions = lineWalkPositions(startPos, PositionData(x, y));

    // iterate over all the positions we move over, stop at the first blockage
    for (uint i = 0; i < positions.length; i++) {
      PositionData memory pos = positions[i];
      bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(pos.x, pos.y));

      assert(atPosition.length < 2);

      //if we hit an object or at the end of our walk, move to that position
      if (atPosition.length == 1 && i > 0) {
        Position.set(player, positions[i - 1]);
        return;
      } else if (i == positions.length - 1) {
        Position.set(player, positions[i]);
        return;
      }
    }

    require(false, "No available place to move");
  }

  function spawn(int32 x, int32 y) public {
    bytes32 playerEntity = addressToEntityKey(address(_msgSender()));
    require(!Player.get(playerEntity), "already spawned");

    Player.set(playerEntity, true);
    Position.set(playerEntity, x, y);
    Health.set(playerEntity, 100);
    Damage.set(playerEntity, 10);
  }

  // function move(int32 x, int32 y) public {
  //   bytes32 player = addressToEntityKey(address(_msgSender()));
  //   // check if there is a player at the position
  //   bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));
  //   require(atPosition.length == 0, "position occupied");

  //   Position.set(player, x, y);
  // }

  function abs(int x) private pure returns (int) {
    return x >= 0 ? x : -x;
  }
}
