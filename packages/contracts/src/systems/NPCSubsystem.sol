// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { NPC, Seeker, Weight, Move, Position, PositionData, Barbarian, Soldier, Ox, Aggro } from "../codegen/Tables.sol";
import { NPCType, MoveType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { getUniqueEntity } from "@latticexyz/world/src/modules/uniqueentity/getUniqueEntity.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { SpawnSubsystem } from "./SpawnSubsystem.sol";

contract NPCSubsystem is System {

  function spawnNPC(bytes32 spawner, int32 x, int32 y, NPCType npcType) public {

    require(npcType != NPCType.None, "None");

    bytes32 entity = getUniqueEntity();
    IWorld world = IWorld(_world());

    NPC.set(entity, uint32(npcType));

    if (npcType == NPCType.Player) {
      world.spawnPlayerNPC(entity, x, y);
    } else if (npcType == NPCType.Soldier) {
      Soldier.set(entity, true);
      Weight.set(entity, 1);
      Move.set(entity, uint32(MoveType.Push));
      Seeker.set(entity, 2);
      Aggro.set(entity,1);
    } else if (npcType == NPCType.Barbarian) {
      Barbarian.set(entity, true);
      Weight.set(entity, 1);
      Move.set(entity, uint32(MoveType.Push));
      Seeker.set(entity, 2);
      Aggro.set(entity,1);
    } else if (npcType == NPCType.Ox) {
      Ox.set(entity, true);
      Weight.set(entity, -10);
      Move.set(entity, uint32(MoveType.Push));
    }
  }

}