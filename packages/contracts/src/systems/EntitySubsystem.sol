// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { getKeysWithValue } from "@latticexyz/world-modules/src/modules/keyswithvalue/getKeysWithValue.sol";
import { Player, Position, PositionTableId, PositionData, Entities, NPC } from "../codegen/Tables.sol";
import { NPCType } from "../codegen/Types.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { BehaviourSubsystem } from "./BehaviourSubsystem.sol";
import { MapSubsystem } from "./MapSubsystem.sol";

contract EntitySubsystem is System {

  function createEntities(bytes32 chunkEntity, int32 playWidth, uint32 playHeight) public {
    //set entities arrays
    uint256 totalWidth = uint256(uint32(playWidth) + uint32(playWidth) + 1);
    bytes32[] memory width = new bytes32[](totalWidth);
    bytes32[] memory height = new bytes32[](uint256(playHeight));
    Entities.set(chunkEntity, width, height);
  }

  //find all nearby entities that have tick updates and tick them
  function triggerEntities(bytes32 causedBy, bytes32 player, PositionData memory pos) public {
    console.log("triggerEntities");

    //only NPC movements trigger entity ticks
    if(NPC.get(player) == uint32(NPCType.None)) {return;}

    //if we are off the map we don't trigger NPCs
    if (IWorld(_world()).onMap(pos.x, pos.y) == false) { return;}

    //TODO gas golf
    PositionData[] memory positions = neumanNeighborhoodOuter(pos, 2);
    bytes32[] memory entities = activeEntities(positions);

    for (uint i = 0; i < positions.length; i++) {
      if (entities[i] == bytes32(0)) {continue;}
      IWorld(_world()).tickBehaviour(causedBy, player, entities[i], pos, positions[i]);
    }
  }

  function activeEntities(PositionData[] memory positions) internal returns (bytes32[] memory) {
    console.log("activeEntities");
    bytes32[] memory neighbors = new bytes32[](positions.length);
    for (uint i = 0; i < positions.length; i++) {
      bytes32[] memory entities = IWorld(_world()).getKeysAtPosition(positions[i].x, positions[i].y, 0);
      neighbors[i] = entities.length > 0 ? entities[0] : bytes32(0);
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
