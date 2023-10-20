// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Position, Player, Health, GameState, Bounds } from "../codegen/index.sol";
import { Road, Move, Action, Rock, Scroll, Seeds, Boots, Weight, NPC } from "../codegen/index.sol";
import { Shovel, Pickaxe, Stick, FishingRod, Sword} from "../codegen/index.sol";
import { PositionTableId, PositionData } from "../codegen/index.sol";
import { RoadState, RockType, MoveType, ActionType, NPCType } from "../codegen/common.sol";

import { Rules } from "../utility/rules.sol";
import { Actions } from "../utility/actions.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions, withinManhattanDistance, withinChessDistance, getDistance, withinManhattanMinimum } from "../utility/grid.sol";
import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";

import { TerrainSubsystem } from "./TerrainSubsystem.sol";
import { SpawnSubsystem } from "./SpawnSubsystem.sol";
import { RewardSubsystem } from "./RewardSubsystem.sol";
import { EntitySubsystem } from "./EntitySubsystem.sol";
import { MoveSubsystem } from "./MoveSubsystem.sol";

contract ToolSubsystem is System {

  //forces a player to push
  function stick(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Stick.get(player), "no stick");
    require(Rules.canDoStuff(player), "hmm");

    PositionData memory playerPos = Position.get(player);
    PositionData memory stickPos = PositionData(x, y, 0);

    //check initial push is good
    bytes32[] memory atStick = Rules.getKeysAtPosition(world,stickPos.x, stickPos.y, 0);
    require(Rules.canInteract(player, playerPos, atStick, 1), "bad interact");
    
    PositionData memory vector = PositionData(stickPos.x - playerPos.x, stickPos.y - playerPos.y, 0);
    SystemSwitch.call(abi.encodeCall(world.moveOrPush, (player, atStick[0], stickPos, vector)));

  }

  function shovel(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Shovel.get(player), "no shovel");
    require(Rules.canDoStuff(player), "hmm");
    require(Rules.onRoad(x, y), "off road");
    require(withinManhattanDistance(PositionData(x, y, 0), Position.get(player), 1), "too far");

    SystemSwitch.call(abi.encodeCall(world.spawnShoveledRoad, (player, x,y)));
    Actions.setAction(player, ActionType.Shoveling, x, y);

  }

  function melee(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Sword.get(player), "no Sword");
    require(Rules.canDoStuff(player), "hmm");

    PositionData memory pos = PositionData(x, y, 0);
    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,x, y, 0);
    require(atPosition.length > 0, "attacking an empty spot");
    require(withinManhattanDistance(pos, Position.get(player), 1), "too far to attack");

    int32 health = Health.get(atPosition[0]);
    require(health > 0, "this thing on?");

    Actions.setActionTargeted(player, ActionType.Melee, x, y, atPosition[0]);

    health--;

    if (health <= 0) {
      SystemSwitch.call(abi.encodeCall(world.kill, (player, atPosition[0], player, pos)));
    } else {
      Health.set(atPosition[0], health);
    }
  }
  
  function mine(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Pickaxe.get(player), "no Pickaxe");
    require(Rules.canDoStuff(player), "hmm");

    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,x, y, 0);

    require(Rules.canInteract(player, Position.get(player), atPosition, 1), "bad interact");

    uint32 rockState = Rock.get(atPosition[0]);

    require(rockState > uint32(RockType.None), "Rock not found or none");
    require(rockState < uint32(RockType.Nucleus), "Rock ground to a pulp");

    //increment the rock state
    rockState++;

    Rock.set(atPosition[0], rockState);
    Actions.setActionTargeted(player, ActionType.Mining, x, y, atPosition[0]);

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


  function fish(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(FishingRod.get(player), "no FishingRod");
    require(Rules.canDoStuff(player), "hmm");

    PositionData memory startPos = Position.get(player);
    PositionData memory fishPos = PositionData(x, y, 0);

    //check initial push is good
    bytes32[] memory atPos = Rules.getKeysAtPosition(world,fishPos.x, fishPos.y, 0);
    require(Rules.canInteract(player, startPos, atPos, 1), "bad interact");
    require(Weight.get(atPos[0]) <= 0, "too heavy");
    Rules.requirePushable(atPos);

    //set player action
    Actions.setActionTargeted(player, ActionType.Fishing, x, y, atPos[0]);

    PositionData memory vector = PositionData(startPos.x - fishPos.x, startPos.y - fishPos.y, 0);
    PositionData memory endPos = PositionData(startPos.x + vector.x, startPos.y + vector.y, 0);
    
    bytes32[] memory atDest = Rules.getKeysAtPosition(world,endPos.x, endPos.y, 0);
    if(atDest.length > 0) {
      Rules.requireOnMap(atDest[0], endPos);
      Rules.requireCanPlaceOn(atDest);
    }

    //move other player
    SystemSwitch.call(abi.encodeCall(world.moveTo, (player, atPos[0], startPos, endPos, atDest, ActionType.Hop)));

  }
  
  function teleportScroll(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Rules.canDoStuff(player), "hmm");
    uint32 scrolls = Scroll.get(player);
    require(scrolls > uint32(0), "not enough scrolls");

    Scroll.set(player, scrolls - 1);
    SystemSwitch.call(abi.encodeCall(world.teleport, (player, x, y)));

  }

}