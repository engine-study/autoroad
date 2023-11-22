// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";

import { Player, Position, PositionTableId, PositionData, Entities, NPC, TickTest } from "../codegen/index.sol";
import { NPCType } from "../codegen/common.sol";

import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { Rules } from "../utility/rules.sol";

contract EntitySubsystem is System {

  function triggerTicks(bytes32 causedby) public {
    uint256 lastBlock = TickTest.getLastBlock();
    if(block.number == lastBlock) return;

    IWorld world = IWorld(_world());
    SystemSwitch.call(abi.encodeCall(world.tickEntity, (causedby, TickTest.getEntities())));

    //tick npcs
    // bytes32[] memory entities = Entities.getEntities();
    // for(uint i = 0; i < entities.length; i++) {
    //   SystemSwitch.call(abi.encodeCall(world.tickEntity, (causedby, entities[i])));
    // }

    TickTest.setLastBlock(block.number);

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
    bytes32[] memory entities = activeEntities(positions);

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

  function activeEntities(PositionData[] memory positions) internal returns (bytes32[] memory) {
    // console.log("activeEntities");
    bytes32[] memory neighbors = new bytes32[](positions.length);
    for (uint i = 0; i < positions.length; i++) {
      bytes32[] memory entities = Rules.getKeysAtPosition(IWorld(_world()) ,positions[i].x, positions[i].y, 0);
      if(entities.length > 0) {neighbors[i] = entities[0];}
    }

    return neighbors;
  }

    //ignore the center position
   function neumanNeighborhoodOuter(PositionData memory center, int32 distance) public pure returns (PositionData[] memory) {
    uint length = uint((uint32(distance) * 4));
    uint index = 0;
    PositionData[] memory neighbors = new PositionData[](length);

    for (int32 x = int32(-distance); x <= distance; x++) {
        if (x == 0) continue;
      neighbors[index] = PositionData(center.x + x, center.y, 0);
      index++;
    }

    for (int32 y = int32(-distance); y <= distance; y++) {
      //don't cross over centre twice
      if (y == 0) continue;
      neighbors[index] = PositionData(center.x, center.y + y, 0);
      index++;
    }

    return neighbors;
  }

  function neumanNeighborhood(PositionData memory center, int32 distance) public pure returns (PositionData[] memory) {
    uint length = uint((uint32(distance) * 4) + 1);
    uint index = 0;
    PositionData[] memory neighbors = new PositionData[](length);

    for (int32 x = int32(-distance); x <= distance; x++) {
      neighbors[index] = PositionData(center.x + x, center.y, 0);
      index++;
    }

    for (int32 y = int32(-distance); y <= distance; y++) {
      //don't cross over centre twice
      if (y == 0) continue;
      neighbors[index] = PositionData(center.x, center.y + y, 0);
      index++;
    }

    return neighbors;
  }

  function mooreNeighborhood(PositionData memory center) internal pure returns (PositionData[] memory) {
    PositionData[] memory neighbors = new PositionData[](9);
    uint256 index = 0;

    for (int32 x = -1; x <= 1; x++) {
      for (int32 y = -1; y <= 1; y++) {
        neighbors[index] = PositionData(center.x + x, center.y + y, 0);
        index++;
      }
    }

    return neighbors;
  }
}
