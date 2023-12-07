// SPDX-License-Identifier: MITTypes.sol
pragma solidity >=0.8.21;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";

import { Position, PositionTableId, PositionData, Health, Action, NPC, Aggro, Seek, Move, Wander, Fling, Thrower, Thief, Cursed} from "../codegen/index.sol";
import { Soldier, Barbarian, Archer, LastMovement, LastAction} from "../codegen/index.sol";
import { ActionName, NPCType, MoveType } from "../codegen/common.sol";

import { Rules } from "../utility/rules.sol";
import { Actions } from "../utility/actions.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { getDistance, getVectorNormalized, addPosition, lineWalkPositions, multiplyPosition } from "../utility/grid.sol";
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

    if(Rules.hasTicked(entity)) {return;}
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
    if (Rules.onMap(walkPos.x, walkPos.y) == false) { 
      Actions.setAction(entity, ActionName.Idle, walkPos.x, walkPos.y);
      return;
    }
    bytes32[] memory atDest = Rules.getKeysAtPosition(world, walkPos.x, walkPos.y, 0);
    SystemSwitch.call(abi.encodeCall(world.moveTo, (causedBy, entity, entityPos, walkPos, atDest, ActionName.Walking)));

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
    uint32 thief = Thief.get(entity);
    if(thief > 0 && thief >= distance) { doThief(causedBy, entity, target, entityPos, targetPos);}
    uint32 thrower = Thrower.get(entity);
    if(thrower > 0 && thrower >= distance) { doThrower(causedBy, entity, target, entityPos, targetPos);}
 }

  function callFling(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory entityPos, PositionData memory targetPos) public {
    console.log("fling");

    IWorld world = IWorld(_world());
    PositionData memory newPos = addPosition(targetPos, getVectorNormalized(entityPos, targetPos));
    SystemSwitch.call(abi.encodeCall(world.doFling, (causedBy, entity, target, targetPos, newPos, ActionName.Throw)));
  }

  function doCurse(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory entityPos, PositionData memory targetPos) public {
    console.log("curse");

    IWorld world = IWorld(_world());
    SystemSwitch.call(abi.encodeCall(world.doSwap, (causedBy, entity, entityPos, targetPos)));
  }

  function doThief(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory entityPos, PositionData memory targetPos) public {
    console.log("thief");
    IWorld world = IWorld(_world());
    SystemSwitch.call(abi.encodeCall(world.softWithdrawCoins, (target, 5)));
    SystemSwitch.call(abi.encodeCall(world.giveCoins, (entity, 5)));
  }

  function doThrower(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory entityPos, PositionData memory targetPos) public {
     console.log("throw");

    IWorld world = IWorld(_world());
    PositionData memory throwStrength = multiplyPosition(getVectorNormalized(targetPos, entityPos), 2);
    PositionData memory newPos = addPosition(targetPos, throwStrength);
    SystemSwitch.call(abi.encodeCall(world.doFling, (causedBy, entity, target, targetPos, newPos, ActionName.Throw)));
  }

  function doSeek(bytes32 causedBy, bytes32 seek, bytes32 target, PositionData memory seekerPos, PositionData memory targetPos) public {
    console.log("seek");

    IWorld world = IWorld(_world());
    //walk towards target
    PositionData memory walkPos = addPosition(seekerPos, getVectorNormalized(seekerPos,targetPos));
    bytes32[] memory atDest = Rules.getKeysAtPosition(world, walkPos.x, walkPos.y, 0);
    SystemSwitch.call(abi.encodeCall(world.moveTo, (causedBy, seek, seekerPos, walkPos, atDest, ActionName.Walking)));
  }

  function canAggroEntity(bytes32 attacker, bytes32 target) public returns(bool) {

    //soldiers only attack barbarians
    bool attackerIsSoldier = Soldier.get(attacker);
    if(attackerIsSoldier) {
      return Barbarian.get(target);
    } 
    
    //barbarians attack everything not a soldier
    bool attackerIsBarbarian = Barbarian.get(attacker);
    if(attackerIsBarbarian) {
      NPCType npc = NPCType(NPC.get(target));
      return npc == NPCType.Player || npc == NPCType.Soldier;
    }

    return true;
  }

  function doAggro(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory entityPos, PositionData memory targetPos) public {
    console.log("aggro");
    IWorld world = IWorld(_world());

    //soldiers don't attack players or other soldiers
    if(canAggroEntity(entity, target) == false) {return;}

    //kill target
    Actions.setActionTargeted(entity, ActionName.Melee, targetPos.x, targetPos.y, target);
    SystemSwitch.call(abi.encodeCall(world.kill, (causedBy, target, entity, targetPos)));
  }

  function doArrow(bytes32 causedBy, bytes32 entity, bytes32 target, PositionData memory entityPos, PositionData memory targetPos) public {
    console.log("archer");
    IWorld world = IWorld(_world());

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

    //soldiers don't attack players or other soldiers
    if(canAggroEntity(entity, target) == false) {return;}
    
    Actions.setActionTargeted(entity, ActionName.Bow, targetPos.x, targetPos.y, target);

    //kill target if it is NPC
    uint32 npc = NPC.get(target);
    if(npc > 0) {
      SystemSwitch.call(abi.encodeCall(world.kill, (causedBy, target, entity, targetPos)));
    }

  }

}