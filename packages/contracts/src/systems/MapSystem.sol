// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, Bounds, Chunk, Position, PositionTableId, PositionData, Carriage } from "../codegen/Tables.sol";
import { TerrainType, RockType } from "../codegen/Types.sol";
import { positionToEntityKey } from "../utility/positionToEntityKey.sol";

contract MapSystem is System {

  function onMap(int32 x, int32 y) public returns (bool) {
    // bound to map
    (int32 left, int32 right, int32 up, int32 down) = Bounds.get();
    return x >= int32(left) && x <= right && y <= up && y >= down;
  }

  function onWorld(int32 x, int32 y) public returns (bool) {
    (,,int32 up, int32 down ) = Bounds.get();
    (, int32 spawnWidth) = MapConfig.get();
    return x >= int32(-spawnWidth) && x <= spawnWidth && y <= up && y >= down;
  }

  function createMap(address worldAddress) public {
    IWorld world = IWorld(worldAddress);

    Carriage.set(keccak256(abi.encode("Carriage")), true);

    // MapConfig.set(world, width, height, terrain);
  }
}
