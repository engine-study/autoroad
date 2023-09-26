// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Player, Health, GameState, Bounds } from "../codegen/Tables.sol";
import { Move, Carrying, Bones, Name, Stats, GameEvent, Coinage, Weight, Boots, NPC, XP, Eth } from "../codegen/Tables.sol";
import { Position, PositionTableId } from "../codegen/Tables.sol";
import { MoveType, ActionType, NPCType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { MapSubsystem } from "../systems/MapSubsystem.sol";
import { randomCoord } from "../utility/random.sol";

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

}