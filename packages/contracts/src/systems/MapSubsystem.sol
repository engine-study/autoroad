// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, Bounds, Chunk, Position, PositionTableId, PositionData, Carriage } from "../codegen/index.sol";
import { TerrainType, RockType } from "../codegen/common.sol";
import { positionToEntityKey } from "../utility/positionToEntityKey.sol";
import { TerrainSubsystem } from "./TerrainSubsystem.sol";

contract MapSubsystem is System {

  //TODO switch to position data so we have maps accessed through bytes32
  
  //the coordinate exists inside the current and previous miles excluding spawn zone
  function onMap(int32 x, int32 y) public view returns (bool) {
    // bound to map
    (int32 left, int32 right, int32 up, int32 down) = Bounds.get();
    return x >= int32(left) && x <= right && y <= up && y >= 0;
  }

  //the coordinate exists inside ALL CURRENT AND PREVIOUS MILES and INSIDE THE SPAWN ZONES
  function onWorld(int32 x, int32 y) public view returns (bool) {
    int32 up = Bounds.getUp();
    int32 spawnWidth = MapConfig.getPlaySpawnWidth();
    return x >= int32(-spawnWidth) && x <= spawnWidth && y <= up && y >= 0;
  }

  function onSpawn(int32 x, int32 y) public view returns (bool) {
    int32 up = Bounds.getUp();
    int32 down = Bounds.getDown();
    int32 playWidth = MapConfig.getPlayWidth();
    int32 spawnWidth = MapConfig.getPlaySpawnWidth();
    return (x > playWidth && x <= spawnWidth) && (y <= up && y >= down);
  }

}
