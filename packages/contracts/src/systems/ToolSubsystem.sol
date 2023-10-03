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

contract ToolSubsystem is System {

    
  function mine(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.canDoStuff(player), "hmm");

    bytes32[] memory atPosition = world.getKeysAtPosition ( x, y, 0 );

    require(world.canInteract(player, x, y, atPosition, 1), "bad interact");

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

  }

  function shovel(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.canDoStuff(player), "hmm");
    require(world.onRoad(x, y), "off road");
    require(withinManhattanDistance(PositionData(x, y, 0), Position.get(player), 1), "too far");

    world.spawnShoveledRoad(player, x,y);

  }

  function stick(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.canDoStuff(player), "hmm");

    PositionData memory pos = PositionData(x, y, 0);
    bytes32[] memory atPosition = IWorld(_world()).getKeysAtPosition ( x, y, 0 );

    require(atPosition.length > 0, "attacking an empty spot");
    require(withinManhattanDistance(pos, Position.get(player), 1), "too far to attack");

    int32 health = Health.get(atPosition[0]);
    require(health > 0, "this thing on?");

    health--;

    if (health <= 0) {
      world.kill(player, atPosition[0], player, pos);
    } else {
      Health.set(atPosition[0], health);
    }
  }

  function melee(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.canDoStuff(player), "hmm");

    PositionData memory pos = PositionData(x, y, 0);
    bytes32[] memory atPosition = IWorld(_world()).getKeysAtPosition ( x, y, 0 );

    require(atPosition.length > 0, "attacking an empty spot");
    require(withinManhattanDistance(pos, Position.get(player), 1), "too far to attack");

    int32 health = Health.get(atPosition[0]);
    require(health > 0, "this thing on?");

    health--;

    if (health <= 0) {
      world.kill(player, atPosition[0], player, pos);
    } else {
      Health.set(atPosition[0], health);
    }
  }


  function teleportScroll(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.canDoStuff(player), "hmm");

    //remove scrolls
    uint32 scrolls = Scroll.get(player);
    require(scrolls > uint32(0), "not enough scrolls");
    Scroll.set(player, scrolls - 1);

    teleport(player, x, y);

  }

  function teleport(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.canDoStuff(player), "hmm");
    require(world.onWorld(x, y), "offworld");

    bytes32[] memory atPosition = world.getKeysAtPosition( x, y, 0 );

    require(atPosition.length < 1, "occupied");
    world.setPosition(player, player, x, y, 0, ActionType.Teleport);
  }

  function fish(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.canDoStuff(player), "hmm");

    PositionData memory startPos = Position.get(player);
    PositionData memory fishPos = PositionData(x, y, 0);

    //check initial push is good
    bytes32[] memory atPos = world.getKeysAtPosition(fishPos.x, fishPos.y, 0);
    require(world.canInteract(player, x, y, atPos, 1), "bad interact");
    require(Weight.get(atPos[0]) <= 0, "too heavy");
    

    PositionData memory vector = PositionData(startPos.x - fishPos.x, startPos.y - fishPos.y, 0);
    PositionData memory endPos = PositionData(startPos.x + vector.x, startPos.y + vector.y, 0);
    bytes32[] memory atDest = world.getKeysAtPosition(endPos.x, endPos.y, 0);

    world.requirePushable(atPos);
    world.requireOnMap(atDest, endPos);
    world.requireCanPlaceOn(atDest);
    world.moveTo(player, atPos[0], startPos, endPos, atDest, ActionType.Hop);
  }

}
