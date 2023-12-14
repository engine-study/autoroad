// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Position, Player, Health, GameState, Bounds } from "../codegen/index.sol";
import { Road, Move, Action, Rock, Scroll, Seeds, Boots, Weight, NPC, ScrollSwap, Pocket, Carry } from "../codegen/index.sol";
import { Shovel, Pickaxe, Stick, FishingRod, Sword} from "../codegen/index.sol";
import { PositionTableId, PositionData } from "../codegen/index.sol";
import { RoadState, RockType, MoveType, ActionName, NPCType } from "../codegen/common.sol";

import { Rules } from "../utility/rules.sol";
import { Actions } from "../utility/actions.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions, withinManhattanDistance, withinChessDistance, getDistance, withinManhattanMinimum, getVectorNormalized } from "../utility/grid.sol";
import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";

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
    Actions.setActionTargeted(player, ActionName.Stick, x, y, atStick[0]);
    SystemSwitch.call(abi.encodeCall(world.moveOrPush, (player, atStick[0], stickPos, vector, 3)));

  }

  function shovel(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Shovel.get(player), "no shovel");
    require(Rules.canDoStuff(player), "hmm");
    require(Rules.onRoad(x, y), "off road");
    Rules.canInteractEmpty(player, Position.get(player), PositionData(x, y, 0),Rules.getKeysAtPosition(world,x, y, 0), 1);
    
    SystemSwitch.call(abi.encodeCall(world.spawnShoveledRoad, (player, x,y)));
    Actions.setAction(player, ActionName.Shoveling, x, y);

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

    Actions.setActionTargeted(player, ActionName.Melee, x, y, atPosition[0]);

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
    Actions.setActionTargeted(player, ActionName.Mining, x, y, atPosition[0]);

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
    require(Weight.get(atPos[0]) + Weight.get(player) <= 0, "too heavy");
    Rules.requirePushable(atPos);

    PositionData memory vector = PositionData(startPos.x - fishPos.x, startPos.y - fishPos.y, 0);
    PositionData memory endPos = PositionData(startPos.x + vector.x, startPos.y + vector.y, 0);
    
    SystemSwitch.call(abi.encodeCall(world.doFling, (player, player, atPos[0], startPos, endPos, ActionName.Fishing)));

  }
  
  function teleportScroll(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Rules.canDoStuff(player), "hmm");
    uint32 scrolls = Scroll.get(player);
    require(scrolls > uint32(0), "not enough scrolls");

    Scroll.set(player, scrolls - 1);
    SystemSwitch.call(abi.encodeCall(world.teleport, (player, x, y, ActionName.Teleport)));

  }

  function swapScroll(bytes32 player, int32 x, int32 y) public {
    require(Rules.canDoStuff(player), "hmm");

    uint32 scrolls = ScrollSwap.get(player);
    require(scrolls > uint32(0), "not enough scrolls");

    // get all the positions in the line we are swapping
    PositionData memory startPos = Position.get(player);
    PositionData memory endPos = PositionData(x, y, 0);
    
    if(doSwap(player, player, startPos, endPos)) {
      ScrollSwap.set(player, scrolls - 1);
    }

  }

  function doSwap(bytes32 causedBy, bytes32 entity, PositionData memory entityPos, PositionData memory targetPos) public returns(bool) {
    IWorld world = IWorld(_world());
    
    bytes32[] memory atPosition = Rules.getKeysAtPosition(world, targetPos.x, targetPos.y, 0);
    if(Rules.canInteract(entity, entityPos, atPosition, 99) == false) {return false;}
    
    // PositionData [] memory vector = getVectorNormalized(startPos, endPos)
    PositionData[] memory positions = lineWalkPositions(entityPos, targetPos);
    // iterate over all the positions we move over, stop at the first blockage
    //START index at 1, ignoring our own position AND the last position
    for (uint i = 1; i < positions.length-1; i++) {
      bytes32[] memory atDest = Rules.getKeysAtPosition(IWorld(_world()),positions[i].x, positions[i].y, 0);
      if(atDest.length != 0 && Rules.canBlock(MoveType(Move.get(atDest[0])))) {return false;}
    }

    //put swap object underground for a second
    Position.set(atPosition[0], targetPos.x, targetPos.y, 10);

    //move everything into place
    SystemSwitch.call(abi.encodeCall(world.teleport, (entity, targetPos.x, targetPos.y, ActionName.Swap)));
    SystemSwitch.call(abi.encodeCall(world.teleport, (atPosition[0], entityPos.x, entityPos.y, ActionName.Swap)));

    return true;

  }

   function pocket(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Pocket.get(player), "no Pocket");
    require(Rules.canDoStuff(player), "hmm");
    PositionData memory targetPos = PositionData(x,y,0);

    bytes32 carry = Carry.get(player);
    bytes32[] memory atDest = Rules.getKeysAtPosition(world,x, y, 0);
    PositionData memory playerPos = Position.get(player);

    bool isPocketing = carry == bytes32(0);

    Actions.setActionTargeted(player, ActionName.Pocket, x, y, carry);

    if(isPocketing) {

      require(Rules.canInteract(player, playerPos, atDest, 1), "bad interact");

      carry = atDest[0];

      Rules.requireIsFairGame(carry);
      Carry.set(player, carry);

      //should just use destroy
      Position.set(carry, PositionData(x,y,-2));
      Health.set(carry, -1);

      Actions.setAction(carry, ActionName.Destroy, x, y);

    } else {
      require(Rules.canInteractEmpty(player, playerPos, PositionData(x,y,0), atDest, 1), "bad interact");
      require(Rules.onMapOrSpawn(carry, targetPos), "offmap");

      Position.set(carry, x,y,0);
      Health.set(carry, 1);
      Carry.set(player, bytes32(0));

      SystemSwitch.call(abi.encodeCall(world.moveTo, (player, carry, playerPos, targetPos, atDest, ActionName.Spawn)));

    }



  }

  
}