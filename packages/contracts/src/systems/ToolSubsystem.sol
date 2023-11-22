// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Position, Player, Health, GameState, Bounds } from "../codegen/index.sol";
import { Road, Move, Action, Rock, Scroll, Seeds, Boots, Weight, NPC, ScrollSwap } from "../codegen/index.sol";
import { Shovel, Pickaxe, Stick, FishingRod, Sword} from "../codegen/index.sol";
import { PositionTableId, PositionData } from "../codegen/index.sol";
import { RoadState, RockType, MoveType, ActionType, NPCType } from "../codegen/common.sol";

import { Rules } from "../utility/rules.sol";
import { Actions } from "../utility/actions.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions, withinManhattanDistance, withinChessDistance, getDistance, withinManhattanMinimum, getVectorNormalized } from "../utility/grid.sol";
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

    Rules.requireInteractable(player, playerPos, atStick, 1);
    Rules.requirePushable(atStick);
    require(NPC.get(atStick[0]) > 0, "Not an NPC");
    
    PositionData memory vector = PositionData(stickPos.x - playerPos.x, stickPos.y - playerPos.y, 0);
    SystemSwitch.call(abi.encodeCall(world.moveOrPush, (player, atStick[0], stickPos, vector, 3)));

  }

  function shovel(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Shovel.get(player), "no shovel");
    require(Rules.canDoStuff(player), "hmm");
    require(Rules.onRoad(x, y), "off road");
    Rules.canInteractEmpty(player, Position.get(player), PositionData(x, y, 0),Rules.getKeysAtPosition(world,x, y, 0), 1);
    
    SystemSwitch.call(abi.encodeCall(world.spawnShoveledRoad, (player, x,y)));
    Actions.setAction(player, ActionType.Shoveling, x, y);

  }

  function melee(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Sword.get(player), "no Sword");
    require(Rules.canDoStuff(player), "hmm");

    PositionData memory playerPos = Position.get(player);
    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,x, y, 0);

    Rules.requireInteractable(player, playerPos, atPosition, 1);
    require(NPC.get(atPosition[0]) > 0, "attacking an empty spot");

    int32 health = Health.get(atPosition[0]);
    require(health > 0, "this thing on?");

    Actions.setActionTargeted(player, ActionType.Melee, x, y, atPosition[0]);

    health--;

    if (health <= 0) {
      PositionData memory meleePos = PositionData(x, y, 0);
      SystemSwitch.call(abi.encodeCall(world.kill, (player, atPosition[0], player, meleePos)));
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
    else if (rockState >= uint32(RockType.Pavimentum)) {
      Position.deleteRecord(atPosition[0]);
      Health.deleteRecord(atPosition[0]);
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
    require(Weight.get(atPos[0]) <= Weight.get(player), "too heavy");
    Rules.requirePushable(atPos);

    //set player action
    Actions.setActionTargeted(player, ActionType.Fishing, x, y, atPos[0]);

    PositionData memory vector = PositionData(startPos.x - fishPos.x, startPos.y - fishPos.y, 0);
    PositionData memory endPos = PositionData(startPos.x + vector.x, startPos.y + vector.y, 0);
    
    SystemSwitch.call(abi.encodeCall(world.doFling, (player, atPos[0], startPos, endPos)));

  }
  
  function teleportScroll(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Rules.canDoStuff(player), "hmm");
    uint32 scrolls = Scroll.get(player);
    require(scrolls > uint32(0), "not enough scrolls");

    Scroll.set(player, scrolls - 1);
    SystemSwitch.call(abi.encodeCall(world.teleport, (player, x, y, ActionType.Teleport)));

  }

  function swapScroll(bytes32 player, int32 x, int32 y) public {
    require(Rules.canDoStuff(player), "hmm");

    uint32 scrolls = ScrollSwap.get(player);
    require(scrolls > uint32(0), "not enough scrolls");

    IWorld world = IWorld(_world());
    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,x, y, 0);
    PositionData memory startPos = Position.get(player);
    require(Rules.canInteract(player, startPos, atPosition, 99), "bad interact");

    // get all the positions in the line we are swapping
    PositionData memory endPos = PositionData(x, y, 0);
    // PositionData [] memory vector = getVectorNormalized(startPos, endPos)
    PositionData[] memory positions = lineWalkPositions(startPos, endPos);
    // iterate over all the positions we move over, stop at the first blockage
    //START index at 1, ignoring our own position AND the last position
    for (uint i = 1; i < positions.length-1; i++) {
      bytes32[] memory atDest = Rules.getKeysAtPosition(IWorld(_world()),positions[i].x, positions[i].y, 0);
      require(atDest.length == 0 || Rules.canBlock(MoveType(Move.get(atDest[0]))) == false, "blocked");
    }

    //put swap object underground for a second
    Position.set(atPosition[0], x, y, -2);

    //move everything into place
    SystemSwitch.call(abi.encodeCall(world.teleport, (player, x, y, ActionType.Swap)));
    SystemSwitch.call(abi.encodeCall(world.teleport, (atPosition[0], startPos.x, startPos.y, ActionType.Swap)));

    ScrollSwap.set(player, scrolls - 1);

  }
}