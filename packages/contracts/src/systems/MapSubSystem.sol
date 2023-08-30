// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, Bounds, Chunk, Position, PositionTableId, PositionData, Carriage } from "../codegen/Tables.sol";
import { TerrainType, RockType } from "../codegen/Types.sol";
import { positionToEntityKey } from "../utility/positionToEntityKey.sol";
import { RoadSubsystem } from "./RoadSubsystem.sol";

contract MapSubsystem is System {

  //the coordinate exists inside the current and previous miles excluding spawn zone
  function onMap(int32 x, int32 y) public view returns (bool) {
    // bound to map
    (int32 left, int32 right, int32 up, int32 down) = Bounds.get();
    return x >= int32(left) && x <= right && y <= up && y >= 0;
  }

  //the coordinate exists inside ALL CURRENT AND PREVIOUS MILES and INSIDE THE SPAWN ZONES
  function onWorld(int32 x, int32 y) public view returns (bool) {
    int32 up = Bounds.getUp();
    int32 spawnWidth = MapConfig.getSpawnArea();
    return x >= int32(-spawnWidth) && x <= spawnWidth && y <= up && y >= 0;
  }

  function createMap(address worldAddress) public {

    IWorld world = IWorld(worldAddress);
    Carriage.set(world.getCarriageEntity(), true);
    // MapConfig.set(world, width, height, terrain);
  }
}
