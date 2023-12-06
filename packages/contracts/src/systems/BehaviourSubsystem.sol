// SPDX-License-Identifier: MITTypes.sol
pragma solidity >=0.8.21;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Position, PositionTableId, PositionData, Health, Action, NPC, Aggro, Seek, Move, Wander, Fling, Cursed, LastMovement, LastAction} from "../codegen/index.sol";
import { Soldier, Barbarian, Archer} from "../codegen/index.sol";
import { ActionType, NPCType, MoveType } from "../codegen/common.sol";

import { Rules } from "../utility/rules.sol";
import { Actions } from "../utility/actions.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { getDistance, getVectorNormalized, addPosition, lineWalkPositions } from "../utility/grid.sol";
import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";
import { randomDirection } from "../utility/random.sol";
import { neumanNeighborhoodOuter, activeEntities } from "../utility/grid.sol";


contract BehaviourSubsystem is System {

  function tickEntity(bytes32 causedBy, bytes32 entity) public {
    
    //perform the tick action that can happen once per block maximum
    //also notice entity takes credit for its own actions (ignores causedBy)
    tickAction(entity, entity, Position.get(entity));
    //do nothing else for now
  }

  function tickAction(bytes32 causedBy, bytes32 entity, PositionData memory entityPos) public {

    if(Rules.hasMoved(entity)) {return;}
    IWorld world = IWorld(_world());

    LastMovement.set(entity, block.number);

    //all movement related stuff
    console.log("tick movement");
    if(Wander.get(entity) > 0) { doWander(causedBy, entity, entityPos);} 

    //all action related stuff, refresh position
    console.log("tick behaviour");
    entityPos = Position.get(entity);

    PositionData[] memory positions = neumanNeighborhoodOuter(entityPos, 1);
    bytes32[] memory entities = activeEntities(world, positions);

    for (uint i = 0; i < positions.length; i++) {
      if (entities[i] == bytes32(0)) {continue;}
      console.log("tick");
      tickBehaviour(causedBy, entity, entities[i], entityPos, positions[i]);
    }

  }

  function doWander(bytes32 causedBy, bytes32 entity, PositionData memory entityPos) public {
    console.log("wander");

    IWorld world = IWorld(_world());
    //walk towards target

    PositionData memory walkPos = addPosition(entityPos, randomDirection(entity, entityPos.x, entityPos.y, 0));
    if (Rules.onMap(walkPos.x, walkPos.y) == false) { return;}
    bytes32[] memory atDest = Rules.getKeysAtPosition(world, walkPos.x, walkPos.y, 0);
    SystemSwitch.call(abi.encodeCall(world.moveTo, (causedBy, entity, entityPos, walkPos, atDest, ActionType.Walking)));

  }

  //out of world positions have been already filtered  at this point, can trust this is hapepning on map
  function tickBehaviour(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory entityPos, PositionData memory targetPos) public {

    console.log("tickBehaviour");

    if(Rules.isTired(entity)) {return;}
    if(Rules.canDoStuff(entity) == false) {return;}
    
    LastAction.set(entity, block.number);

    //activate all behaviours
    uint32 distance = uint32(getDistance(targetPos, entityPos));

    //Seek should either trigger or aggro, not both, and check for 0 values
    uint32 fling = Fling.get(entity);
    if(fling == distance) { callFling(causedBy, entity, target, entityPos, targetPos);} 
    uint32 seek = Seek.get(entity);
    if(seek == distance) { doSeek(causedBy, entity, target, entityPos, targetPos);} 
    uint32 aggro = Aggro.get(entity);
    if(aggro > 0 && aggro == distance) { doAggro(causedBy, entity, target, entityPos, targetPos);}
    uint32 archer = Archer.get(entity);
    if(archer > 0 && archer >= distance) { doArrow(causedBy, entity, target, entityPos, targetPos);}
    uint32 cursed = Cursed.get(entity);
    if(cursed > 0 && cursed >= distance) { doCurse(causedBy, entity, target, entityPos, targetPos);}
 }

  function callFling(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory entityPos, PositionData memory targetPos) public {
    console.log("fling");

    IWorld world = IWorld(_world());
    PositionData memory newPos = addPosition(targetPos, getVectorNormalized(entityPos, targetPos));

    Actions.setActionTargeted(entity, ActionType.Melee, newPos.x, newPos.y, target);
    SystemSwitch.call(abi.encodeCall(world.doFling, (causedBy, target, targetPos, newPos)));
  }

  function doCurse(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory entityPos, PositionData memory targetPos) public {
    console.log("curse");

    IWorld world = IWorld(_world());
    SystemSwitch.call(abi.encodeCall(world.doSwap, (causedBy, entity, entityPos, targetPos)));
  }

  function doSeek(bytes32 causedBy, bytes32 seek, bytes32 target, PositionData memory seekerPos, PositionData memory targetPos) public {
    console.log("seek");

    IWorld world = IWorld(_world());
    //walk towards target
    PositionData memory walkPos = addPosition(seekerPos, getVectorNormalized(seekerPos,targetPos));
    bytes32[] memory atDest = Rules.getKeysAtPosition(world, walkPos.x, walkPos.y, 0);
    SystemSwitch.call(abi.encodeCall(world.moveTo, (causedBy, seek, seekerPos, walkPos, atDest, ActionType.Walking)));
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

  function doAggro(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory entityPos, PositionData memory targetPos) public {
    console.log("aggro");
    IWorld world = IWorld(_world());

    //soldiers don't attack players or other soldiers
    if(canAggroEntity(entity, target) == false) {return;}

    //kill target
    Actions.setActionTargeted(entity, ActionType.Melee, targetPos.x, targetPos.y, target);
    SystemSwitch.call(abi.encodeCall(world.kill, (causedBy, target, entity, targetPos)));
  }

  function doArrow(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory entityPos, PositionData memory targetPos) public {
    console.log("archer");
    IWorld world = IWorld(_world());

    //soldiers don't attack players or other soldiers
    if(canAggroEntity(entity, target) == false) {return;}

    PositionData[] memory positions = lineWalkPositions(entityPos, targetPos);

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

    Actions.setActionTargeted(entity, ActionType.Bow, targetPos.x, targetPos.y, target);

    //kill target if it is NPC
    uint32 npc = NPC.get(target);
    if(npc > 0) {
      SystemSwitch.call(abi.encodeCall(world.kill, (causedBy, target, entity, targetPos)));
    }

  }

}