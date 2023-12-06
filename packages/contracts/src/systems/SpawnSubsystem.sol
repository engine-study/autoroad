// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Player, Health, GameState, Bounds, Entities, Animal } from "../codegen/index.sol";
import { Move, Bones, Name, Stats, Coinage, Weight, Boots, NPC, XP, Eth, Shovel, Conscription, Head, Robe, Effect, Material, Throw, EnumTest, Thief, Cursed} from "../codegen/index.sol";
import { Soldier, Barbarian, Ox, Aggro, Seek, Archer, Fling, Wander, Rock } from "../codegen/index.sol";
import { Position, PositionTableId, PositionData } from "../codegen/index.sol";
import { MoveType, ActionType, RockType, NPCType, ArmorSet, EffectSet, MaterialSet } from "../codegen/common.sol";

import { Actions } from "../utility/actions.sol";
import { randomCoord } from "../utility/random.sol";
import { findEmptyPositionInArea } from "../utility/grid.sol";
import { getUniqueEntity } from "@latticexyz/world-modules/src/modules/uniqueentity/getUniqueEntity.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";


contract SpawnSubsystem is System {

  function spawnPlayerNPC(bytes32 entity, int32 x, int32 y) public {
    spawnPlayer(entity, x, y, true);
  }

  function spawnPlayer(bytes32 entity, int32 x, int32 y, bool isBot) public {
    bool playerExists = Player.get(entity);

    if (!playerExists) {
      Player.set(entity, true);
      Coinage.set(entity, 10);
      Eth.set(entity, 10000);
      Weight.set(entity, -1);

      bool[] memory h = new bool[](uint(ArmorSet.Count));
      h[0] = true;
      Head.set(entity, 0, h);

      bool[] memory r = new bool[](uint(ArmorSet.Count));
      r[0] = true;
      Robe.set(entity, 0, r);

      bool[] memory e = new bool[](uint(EffectSet.Count));
      e[0] = true;
      Effect.set(entity, 0, e);

      bool[] memory m = new bool[](uint(MaterialSet.Count));
      m[0] = true;
      Material.set(entity, 0, m);


      // Conscription.set(entity, true);
      NPC.set(entity, uint32(NPCType.Player));
      XP.set(entity, 0);

      Shovel.set(entity, true);
      Boots.set(entity, 1, 3);

      int32 mileJoined = GameState.getMiles();

      Stats.set(entity, mileJoined);

      // uint8[] memory array = new uint8[](4);
      // array[0] = 3;
      // array[1] = 6;
      // int32[] memory arrayInt = new int32[](4);
      // arrayInt[0] = 3;
      // arrayInt[1] = 6;
      // int256[] memory arrayBig = new int256[](4);
      // arrayBig[0] = 3;
      // arrayBig[1] = 6;
      // uint256[] memory arrayUintBig = new uint256[](4);
      // arrayUintBig[0] = 3;
      // arrayUintBig[1] = 6;
      // EnumTest.set(entity, NPCType.Player, array, arrayInt, arrayBig, arrayUintBig);

    } else {
      // EnumTest.pushMaxMove(entity, 5);
    }

    Health.set(entity, 3);
    Move.set(entity, uint32(MoveType.Push));
    Position.set(entity, x, y, 0);
    Actions.setAction(entity, ActionType.Spawn, x, y);
    

    
  }

  function spawnNPC(bytes32 spawner, int32 x, int32 y, NPCType npcType) public {
    console.log("spawn NPC");

    require(npcType != NPCType.None, "None");

    bytes32 entity = getUniqueEntity();

    NPC.set(entity, uint32(npcType));

    if (npcType == NPCType.Player) {
      spawnPlayerNPC(entity, x, y);
    } else if (npcType == NPCType.Soldier) {
      Soldier.set(entity, true);
      Weight.set(entity, 0);
      Seek.set(entity, 2);
      Aggro.set(entity, 1);
    } else if (npcType == NPCType.Barbarian) {
      Barbarian.set(entity, true);
      Weight.set(entity, 1);
      Seek.set(entity, 2);
      Aggro.set(entity, 1);
    } else if (npcType == NPCType.BarbarianArcher) {
      Barbarian.set(entity, true);
      Weight.set(entity, 1);
      Archer.set(entity, 5);
    } else if (npcType == NPCType.Ox) {
      Ox.set(entity, true);
      Weight.set(entity, -5);
    } else if (npcType == NPCType.Deer) {
      Animal.set(entity, true);
      Weight.set(entity, 1);
      Fling.set(entity, 1);
      Wander.set(entity, 1);
      // Entities.setEntities(entity);
      Entities.pushEntities(entity);
    } else if (npcType == NPCType.Taxman) {
      Ox.set(entity, true);
      Weight.set(entity, 1);
      Thief.set(entity, 1);
      Entities.pushEntities(entity);
    } else if (npcType == NPCType.Shoveler) {
      Ox.set(entity, true);
      Weight.set(entity, -5);
      Throw.set(entity, 1);
      Entities.pushEntities(entity);
    } else if (npcType == NPCType.Gargoyle) {
      Rock.set(entity, uint32(RockType.Gargoyle));
      Weight.set(entity, 99);
      Cursed.set(entity, 1);
      Entities.pushEntities(entity);
    }

    Move.set(entity, uint32(MoveType.Push));
    Health.set(entity, 1);
    Position.set(entity, PositionData(x, y, 0));

    Actions.setAction(entity, ActionType.Spawn, x, y);
  }

  function createTickers(bytes32 causedBy, int32 width, int32 up, int32 down) public {
    IWorld world = IWorld(_world());
    int32 roadSide = RoadConfig.getRight();

    bytes32 entity = getUniqueEntity();

    PositionData memory pos = findEmptyPositionInArea(world, entity, width, up, down, 0, roadSide);
    Actions.deleteAt(world, pos);

    spawnNPC(causedBy, pos.x, pos.y, NPCType.Deer);

  }

  function destroy(bytes32 causedBy, bytes32 target, bytes32 attacker, PositionData memory pos) public {
    if (NPC.get(target) > 0) {
      //kill if it was an NPC
      kill(causedBy, target, attacker, pos);
    } else {
      //destroy
      Position.set(target, pos.x, pos.y, -2);
      Health.set(target, -1);
    }
  }

  function kill(bytes32 causedBy, bytes32 target, bytes32 attacker, PositionData memory pos) public {
    IWorld world = IWorld(_world());

    //kill and move underground
    pos.layer = -2;
    Position.set(target, pos);
    Health.set(target, -1);

    //set to dead
    Actions.setAction(target, ActionType.Dead, pos.x, pos.y);

    //rewards
    SystemSwitch.call(abi.encodeCall(world.killRewards, (causedBy, target, attacker)));

    //spawn bones
    // bytes32 bonesEntity = keccak256(abi.encode("Bones", pos.x, pos.y));
    // Bones.set(bonesEntity, true);
    // Position.set(bonesEntity, pos);
    // Move.set(bonesEntity, uint32(MoveType.Push));
  }
}
