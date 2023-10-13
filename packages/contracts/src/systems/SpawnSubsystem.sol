// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Player, Health, GameState, Bounds } from "../codegen/index.sol";
import { Move, Carrying, Bones, Name, Stats, Coinage, Weight, Boots, NPC, XP, Eth } from "../codegen/index.sol";
import { Position, PositionTableId, PositionData } from "../codegen/index.sol";
import { MoveType, ActionType, NPCType } from "../codegen/common.sol";

import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { randomCoord } from "../utility/random.sol";

import { ActionSystem } from "../systems/ActionSystem.sol";
import { MapSubsystem } from "../systems/MapSubsystem.sol";
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
      Coinage.set(entity, 0);
      Eth.set(entity, 10000);
      Weight.set(entity, -1);
      Boots.set(entity, 3, 3);
      NPC.set(entity, uint32(NPCType.Player));
      NPC.set(entity, uint32(NPCType.Player));
      XP.set(entity, 0);

      int32 mileJoined = GameState.getMiles();

      Stats.set(entity, mileJoined);

    }

    Health.set(entity, 3);
    Move.set(entity, uint32(MoveType.Push));
    Position.set(entity, x, y, 0);
    IWorld(_world()).setAction(entity, ActionType.Spawn, x, y);

  }
  
  function kill(bytes32 causedBy, bytes32 target, bytes32 attacker, PositionData memory pos) public {
    IWorld world = IWorld(_world());

    //kill and move underground
    pos.layer = -2;
    Position.set(target, pos);
    Health.set(target, -1);

    //set to dead
    world.setAction(target, ActionType.Dead, pos.x, pos.y);

    //rewards
    world.killRewards(causedBy, target, attacker);

    //spawn bones
    // bytes32 bonesEntity = keccak256(abi.encode("Bones", pos.x, pos.y));
    // Bones.set(bonesEntity, true);
    // Position.set(bonesEntity, pos);
    // Move.set(bonesEntity, uint32(MoveType.Push));
  }

}