// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";

import { Player, Position, PositionTableId, PositionData, Entities, NPC, TickTest } from "../codegen/index.sol";
import { NPCType } from "../codegen/common.sol";

import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { neumanNeighborhoodOuter, activeEntities } from "../utility/grid.sol";
import { Rules } from "../utility/rules.sol";

contract EntitySubsystem is System {

  function triggerTicks(bytes32 causedby) public {
    uint256 lastBlock = TickTest.getLastBlock();
    bytes32 entity = TickTest.getEntities();
    if(block.number == lastBlock) {return;}

    //set the blocknumber so we can't re-enter
    TickTest.setLastBlock(block.number);
    
    IWorld world = IWorld(_world());
    SystemSwitch.call(abi.encodeCall(world.tickEntity, (causedby, TickTest.getEntities())));

  }

  //find all nearby entities that have tick updates and tick them
  function triggerEntities(bytes32 causedBy, bytes32 player, PositionData memory pos) public {

    // console.log("triggerEntities");
    IWorld world = IWorld(_world());

    //only NPC movements trigger entity ticks
    if(NPC.get(player) == uint32(NPCType.None)) {return;}

    //if we are off the map we don't trigger NPCs
    if (Rules.onMap(pos.x, pos.y) == false) { return;}

    //TODO gas golf, calculate distances and other things here so tickBehaviour doesnt do it
    PositionData[] memory positions = neumanNeighborhoodOuter(pos, 2);
    bytes32[] memory entities = activeEntities(world, positions);

    for (uint i = 0; i < positions.length; i++) {
      if (entities[i] == bytes32(0)) {continue;}

      //tick npcs
      NPCType npcType = NPCType(NPC.get(entities[i]));
      if(npcType > NPCType.None) {SystemSwitch.call(abi.encodeCall(world.tickBehaviour, (causedBy, player, entities[i], pos, positions[i])));}

      //tick other things possible (resources, idk)

      //check player is still alive, exit early if not?? 
      //test this pls, what happens if 3 npcs try to kill one player at once
      // if(Rules.canDoStuff(player) == false) {return;}
    }
  }

}
