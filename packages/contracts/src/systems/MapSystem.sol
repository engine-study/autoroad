// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, Chunk, Position, PositionTableId, Player, Rock, Obstruction, PositionData } from "../codegen/Tables.sol";
import { TerrainType, ObjectType } from "../codegen/Types.sol";
import { positionToEntityKey } from "../utility/positionToEntityKey.sol";


contract MapSystem is System {
  function createMap(address worldAddress) public {
    IWorld world = IWorld(worldAddress);

    TerrainType O = TerrainType.None;
    TerrainType R = TerrainType.Rock;
    TerrainType M = TerrainType.Mine;

    TerrainType[13][40] memory map = [
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, R, O, O, R, O, O, O, O, R, R, O,O],
      [O, O, R, O, O, O, O, O, R, O, O, O,O],
      [O, R, O, O, O, O, O, O, O, O, O, O,O],
      [O, R, O, O, O, O, O, O, O, R, O, O,O], 
      [O, O, O, O, O, O, R, O, O, O, O, O,O],
      [O, O, O, R, O, O, O, O, O, O, O, O,O],
      [O, O, R, O, O, O, O, O, O, O, R, O,O],
      [O, O, O, O, O, O, R, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, R, O, O, O, O, O, R, R, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, R, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O], 
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, R, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, R, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, R, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, R, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, R, O, O, O, O,O], 
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, R, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, R, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O], 
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O],
      [O, O, R, O, O, O, O, O, O, O, O, O,O],
      [O, O, O, O, O, O, O, O, O, O, O, O,O]
    ];

    uint32 height = uint32(map.length);
    uint32 width = uint32(map[0].length);
    bytes memory terrain = new bytes(width * height);

    for (uint32 y = 0; y < height; y++) {
      for (uint32 x = 0; x < width; x++) {

        int32 positionX = int32(x);
        int32 positionY = int32(y);

        TerrainType terrainType = map[y][x];
        if (terrainType == TerrainType.None) continue;

        terrain[(y * width) + x] = bytes1(uint8(terrainType));

        bytes32 entity = positionToEntityKey(x,y);

        if(terrainType == TerrainType.Rock) {
          Rock.set(world, entity, 5, ObjectType.Statumen);
          Position.set(world, entity, positionX, positionY);
          Obstruction.set(world, entity, true);
        }
      }
    }

    MapConfig.set(world, width, height, terrain);
  }

}