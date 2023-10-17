// SPDX-License-Identifier: MITTypes.sol
pragma solidity >=0.8.21;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Position, PositionTableId, PositionData, Health, Action, NPC, Aggro, Seeker, Move} from "../codegen/index.sol";
import { Soldier, Barbarian, Archer} from "../codegen/index.sol";
import { ActionType, NPCType, MoveType } from "../codegen/common.sol";

import { Rules } from "../utility/rules.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { getDistance, getVectorNormalized, addPosition, lineWalkPositions } from "../utility/grid.sol";

import { MoveSubsystem } from "./MoveSubsystem.sol";
import { ActionSystem } from "./ActionSystem.sol";

contract BehaviourSubsystem is System {


  //out of world positions have been already filtered  at this point, can trust this is hapepning on map
  function tickBehaviour(bytes32 causedBy, bytes32 player, bytes32 entity, PositionData memory playerPos, PositionData memory entityPos) public {

    console.log("tickBehaviour");

    //activate all behaviours
    uint32 distance = uint32(getDistance(playerPos, entityPos));

    //seeker should either trigger or aggro, not both, and check for 0 values
    uint32 seeker = Seeker.get(entity);
    if(seeker == distance) { doSeek(causedBy, player, entity, playerPos, entityPos);} 
    uint32 aggro = Aggro.get(entity);
    if(aggro > 0 && aggro == distance) { doAggro(causedBy, player, entity, playerPos, entityPos);}
    uint32 archer = Archer.get(entity);
    if(archer > 0 && archer >= distance) { doArrow(causedBy, player, entity, playerPos, entityPos);}

  }
  
  function doSeek(bytes32 causedBy, bytes32 target, bytes32 seeker, PositionData memory targetPos, PositionData memory seekerPos) public {
    console.log("seek");

    IWorld world = IWorld(_world());
    //walk towards target
    PositionData memory walkPos = addPosition(seekerPos,getVectorNormalized(seekerPos,targetPos));
    bytes32[] memory atDest = Rules.getKeysAtPosition(world,walkPos.x, walkPos.y, 0);
    world.moveTo(causedBy, seeker, seekerPos, walkPos, atDest, ActionType.Walking);
  }

  function canAggroEntity(bytes32 attacker, bytes32 target) public returns(bool) {

    bool attackerIsSoldier = Soldier.get(attacker);
    bool targetIsBarbarian = Barbarian.get(target);

    //soldiers only attack barbarians
    if(attackerIsSoldier) {
      return targetIsBarbarian;
    } 
    
    //barbarians attack everything not a soldier
    bool attackerIsBarbarian = Barbarian.get(attacker);
    if(attackerIsBarbarian) {
      return !targetIsBarbarian;
    }

    return true;
  }

  function doAggro(bytes32 causedBy, bytes32 target, bytes32 attacker, PositionData memory targetPos, PositionData memory attackerPos) public {
    console.log("aggro");
    IWorld world = IWorld(_world());

    //soldiers don't attack players or other soldiers
    if(canAggroEntity(attacker, target) == false) {return;}

    //kill target
    world.setActionTargeted(attacker, ActionType.Melee, targetPos.x, targetPos.y, target);
    world.kill(causedBy, target, attacker, targetPos);
  }

  function doArrow(bytes32 causedBy, bytes32 target, bytes32 attacker, PositionData memory targetPos, PositionData memory attackerPos) public {
    console.log("archer");
    IWorld world = IWorld(_world());

    //soldiers don't attack players or other soldiers
    if(canAggroEntity(attacker, target) == false) {return;}

    PositionData[] memory positions = lineWalkPositions(attackerPos, targetPos);

    //check if anything is in the way 
    for (uint i = 1; i < positions.length-1; i++) {
  
      bytes32[] memory atDest = Rules.getKeysAtPosition(world,positions[i].x, positions[i].y, 0);

      if(atDest.length > 0) {
        //check if this movetype will intercept the arrow
        //set this to target instead and continue
        MoveType atMove = MoveType(Move.get(atDest[0]));
        if(Rules.canPlaceOn(atMove) == false) {
          targetPos = positions[i];
          target = atDest[0];
        }
      }
    }

    world.setActionTargeted(attacker, ActionType.Bow, targetPos.x, targetPos.y, target);

    //kill target if it is NPC
    uint32 npc = NPC.get(target);
    if(npc > 0) {
      world.kill(causedBy, target, attacker, targetPos);
    }

  }

}