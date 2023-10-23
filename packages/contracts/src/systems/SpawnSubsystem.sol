// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Player, Health, GameState, Bounds } from "../codegen/index.sol";
import { Move, Bones, Name, Stats, Coinage, Weight, Boots, NPC, XP, Eth, Pickaxe,Shovel, Axe } from "../codegen/index.sol";
import { Soldier, Barbarian, Ox, Aggro, Seeker, Archer } from "../codegen/index.sol";
import { Position, PositionTableId, PositionData } from "../codegen/index.sol";
import { MoveType, ActionType, NPCType } from "../codegen/common.sol";

import { Actions } from "../utility/actions.sol";
import { randomCoord } from "../utility/random.sol";
import { getUniqueEntity } from "@latticexyz/world-modules/src/modules/uniqueentity/getUniqueEntity.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";

import { RewardSubsystem } from "../systems/RewardSubsystem.sol";

contract SpawnSubsystem is System {
    
  function spawnPlayerNPC(bytes32 entity, int32 x, int32 y) public {
    Name.set(entity, true, uint32(randomCoord(0, 35, x,y)), uint32(randomCoord(0, 1024, x,y+1)), uint32(randomCoord(0, 1733, x,y+2) ));  
    spawnPlayer(entity, x, y, true);
  }

  function spawnPlayer(bytes32 entity, int32 x, int32 y, bool isBot) public {

    bool playerExists = Player.get(entity);

    if(!playerExists) {
      Player.set(entity, true);
      Coinage.set(entity, 10);
      Eth.set(entity, 10000);
      Weight.set(entity, -1);

      NPC.set(entity, uint32(NPCType.Player));
      NPC.set(entity, uint32(NPCType.Player));
      XP.set(entity, 0);

      Shovel.set(entity, true);
      Boots.set(entity, 3, 3);
      
      int32 mileJoined = GameState.getMiles();

      Stats.set(entity, mileJoined);

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
      Weight.set(entity, 1);
      Seeker.set(entity, 2);
      Aggro.set(entity,1);
    } else if (npcType == NPCType.Barbarian) {
      Barbarian.set(entity, true);
      Weight.set(entity, 1);
      Seeker.set(entity, 2);
      Aggro.set(entity,1);
    } else if (npcType == NPCType.BarbarianArcher) {
      Barbarian.set(entity, true);
      Weight.set(entity, 1);
      Archer.set(entity, 5);
    } else if (npcType == NPCType.Ox) {
      Ox.set(entity, true);
      Weight.set(entity, -5);
    }

    Move.set(entity, uint32(MoveType.Push));
    Health.set(entity, 1);
    Position.set(entity, PositionData(x,y,0));

    Actions.setAction(entity, ActionType.Spawn, x, y);
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