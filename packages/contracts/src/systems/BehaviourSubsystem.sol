// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Position, PositionTableId, PositionData, Health, Action, NPC, Aggro, Seeker } from "../codegen/Tables.sol";
import { ActionType, NPCType } from "../codegen/Types.sol";
import { MoveSubsystem } from "./MoveSubsystem.sol";
import { ActionSystem } from "./ActionSystem.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { getDistance, getVectorNormalized, addPosition } from "../utility/grid.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";

contract BehaviourSubsystem is System {

  function tickBehaviour(bytes32 player, bytes32 entity, PositionData memory playerPos, PositionData memory entityPos) public {

    console.log("tickBehaviour");

    //can't tick non-npcs
    NPCType npcType = NPCType(NPC.get(entity));
    if(npcType != NPCType.None) return; 

    IWorld world = IWorld(_world());
    int32 health = Health.get(player);
    if(health == 0) return;

    uint distance = getDistance(playerPos, entityPos);

    if(Seeker.get(entity) == distance) {
      seek(player, entity, playerPos, entityPos);
    } 
    
    if(Aggro.get(entity) == distance) {
      aggro(player, entity, playerPos, entityPos);
    }
  }
  
  function seek(bytes32 player, bytes32 entity, PositionData memory playerPos, PositionData memory entityPos) public {
    IWorld world = IWorld(_world());
    //walk towards player
    PositionData memory walkPos = addPosition(entityPos,getVectorNormalized(entityPos,playerPos));
    bytes32[] memory atDest = getKeysWithValue(PositionTableId, Position.encode(walkPos.x, walkPos.y, 0));
    world.moveTo(player, entity, entityPos, walkPos, atDest, ActionType.Walking);
  }

  function aggro(bytes32 player, bytes32 entity, PositionData memory playerPos, PositionData memory entityPos) public {
    IWorld world = IWorld(_world());
    //kill player
    world.setAction(entity, ActionType.Melee, playerPos.x, playerPos.y);
    world.kill(player, entity, playerPos);
  }

}