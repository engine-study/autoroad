// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Position, PositionTableId, PositionData, Health, Action, NPC, Aggro, Seeker} from "../codegen/Tables.sol";
import { ActionType, NPCType } from "../codegen/Types.sol";
import { MoveSubsystem } from "./MoveSubsystem.sol";
import { ActionSystem } from "./ActionSystem.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { getDistance, getVectorNormalized, addPosition } from "../utility/grid.sol";
import { getKeysWithValue } from "@latticexyz/world-modules/src/modules/keyswithvalue/getKeysWithValue.sol";

contract BehaviourSubsystem is System {

  function tickBehaviour(bytes32 causedBy, bytes32 player, bytes32 entity, PositionData memory playerPos, PositionData memory entityPos) public {

    console.log("tickBehaviour");

    //can't tick non-npcs
    NPCType npcType = NPCType(NPC.get(entity));
    if(npcType == NPCType.None) return; 

    //check player is still alive
    int32 health = Health.get(player);
    if(health == 0) return;

    //activate all behaviours
    IWorld world = IWorld(_world());
    uint32 distance = uint32(getDistance(playerPos, entityPos));
    if(Seeker.get(entity) == distance) { seek(causedBy, player, entity, playerPos, entityPos);} 
    if(Aggro.get(entity) == distance) {aggro(causedBy, player, entity, playerPos, entityPos);}
  }
  
  function seek(bytes32 causedBy, bytes32 target, bytes32 seeker, PositionData memory targetPos, PositionData memory seekerPos) public {
    console.log("seek");

    IWorld world = IWorld(_world());
    //walk towards target
    PositionData memory walkPos = addPosition(seekerPos,getVectorNormalized(seekerPos,targetPos));
    bytes32[] memory atDest = world.getKeysAtPosition(walkPos.x, walkPos.y, 0);
    world.moveTo(causedBy, seeker, seekerPos, walkPos, atDest, ActionType.Walking);
  }

  function aggro(bytes32 causedBy, bytes32 target, bytes32 attacker, PositionData memory targetPos, PositionData memory attackerPos) public {
    console.log("aggro");

    IWorld world = IWorld(_world());

    NPCType targetType = NPCType(NPC.get(target));
    NPCType attackerType = NPCType(NPC.get(attacker));
    
    //soldiers don't attack players or other soldiers
    if(attackerType == NPCType.Soldier && targetType != NPCType.Barbarian) {return;}
    if(attackerType == NPCType.Barbarian && targetType == NPCType.Barbarian) {return;}

    //kill target
    world.setAction(attacker, ActionType.Melee, targetPos.x, targetPos.y);
    world.kill(causedBy, target, attacker, targetPos);
  }

}