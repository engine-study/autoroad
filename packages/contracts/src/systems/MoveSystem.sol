// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Damage, Position, Player, Health, GameState, Bounds } from "../codegen/Tables.sol";
import { Road, Pavement, Move, State, Carrying, Rock, Tree } from "../codegen/Tables.sol";
import { PositionTableId, PositionData} from "../codegen/Tables.sol";
import { RoadState, RockType, MoveType, StateType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions, withinManhattanDistance, withinChessDistance } from "../utility/grid.sol";
import { MapSystem } from "../systems/MapSystem.sol";
import { RoadSystem } from "../systems/RoadSystem.sol";

contract MoveSystem is System {


  //push
  function push(int32 x, int32 y, int32 pushX, int32 pushY) public {
    IWorld world = IWorld(_world());

    bytes32 player = addressToEntityKey(address(_msgSender()));
    PositionData memory startPos = Position.get(player);

    require(world.onMap(pushX, pushY), "off grid");
    require(withinManhattanDistance(PositionData(x, y), PositionData(pushX, pushY), 1), "too far to push");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));

    require(atPosition.length >= 1, "trying to push an empty spot");
    uint32 move = Move.get(atPosition[0]);
    require(move == uint32(MoveType.Push), "pushing a non-pushable object");

    bytes32[] memory atPushPosition = getKeysWithValue(PositionTableId, Position.encode(pushX, pushY));

    assert(atPosition.length < 2);

    //check if we are pushing rocks into a road ditch
    if (atPushPosition.length >= 1) {

      //check if there is an obstruction
      bool obstruction = Move.get(atPushPosition[0]) != 0;
      require(obstruction == false, "pushing into an occupied spot");

      uint32 roadInt = Road.get(atPushPosition[0]);
      require(roadInt < uint32(RoadState.Paved), "Road state too high");

      roadInt++;
      Road.set(atPushPosition[0], roadInt);

      //ROAD COMPLETE!!!
      if(roadInt == uint32(RoadState.Paved)) {
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


  function chop(bytes32 tree) public {

      bytes32 player = addressToEntityKey(address(_msgSender()));

      require(Tree.get(tree), "not a tree");
      require(withinManhattanDistance(Position.get(tree), Position.get(player), 1), "too far to chop");

      int32 health = Health.get(tree);
      health--;

      if(health == 0) {
        Tree.deleteRecord(tree);
        Position.deleteRecord(tree);
        Health.deleteRecord(tree);
        Move.deleteRecord(tree);
      } else {
        Health.set(tree, health);
      }
  }

  function mine(int32 x, int32 y) public {
    
    bytes32 player = addressToEntityKey(address(_msgSender()));
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));

     // Position
    require(withinManhattanDistance(PositionData(x, y), Position.get(player), 1), "too far to mine");
    require(atPosition.length >= 1, "mining an empty spot");

    uint32 rockState = Rock.get(atPosition[0]);

    require(rockState > uint32(RockType.None), "Rock not found or none");
    require(rockState < uint32(RockType.Nucleus), "Rock ground to a pulp");

    //increment the rock state
    rockState += 1;

    Rock.set(atPosition[0], rockState);

    //give rocks that are mined a pushable component
    if(rockState == uint32(RockType.Statumen)) {
      Move.set(atPosition[0], uint32(MoveType.Push));
    } 
    //become shovelable once we are broken down enough
    else if(rockState == uint32(RockType.Nucleus)) {
      Move.set(atPosition[0], uint32(MoveType.Shovel));
    }
  }

  function carry(int32 carryX, int32 carryY) public {

    bytes32 player = addressToEntityKey(address(_msgSender()));
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(carryX, carryY));

    // Position
    require(atPosition.length >= 1, "trying to carry an empty spot");
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
      if (atPosition.length > 0 && i > 0) {
        Position.set(player, positions[i - 1]);
        return;
      } else if (i == positions.length - 1) {
        Position.set(player, positions[i]);
        return;
      }
    }

    require(false, "No available place to move");
  }

  function spawn() public {
    
    bytes32 playerEntity = addressToEntityKey(address(_msgSender()));
    require(!Player.get(playerEntity), "already spawned");

    // uint32 mileDistance = GameState.get()
    (int32 l,int32 r,int32 up,) = Bounds.get();

    Player.set(playerEntity, true);
    Move.set(playerEntity, uint32(MoveType.Push));

    //spawn at the top of the road
    Position.set(playerEntity, 0, int32(up + 5));
  }

  function abs(int x) private pure returns (int) {
    return x >= 0 ? x : -x;
  }
}
