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

  function createMap(address worldAddress) public {
    IWorld world = IWorld(worldAddress);

    Carriage.set(keccak256(abi.encode("Carriage")), true);

    //old emojimon method

    // TerrainType O = TerrainType.None;
    // TerrainType R = TerrainType.Rock;
    // TerrainType M = TerrainType.Mine;

    // TerrainType[13][40] memory map = [
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, R, O, O, R, O, O, O, O, R, R, O,O],
    //   [O, O, R, O, O, O, O, O, R, O, O, O,O],
    //   [O, R, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, R, O, O, O, O, O, O, O, R, O, O,O],
    //   [O, O, O, O, O, O, R, O, O, O, O, O,O],
    //   [O, O, O, R, O, O, O, O, O, O, O, O,O],
    //   [O, O, R, O, O, O, O, O, O, O, R, O,O],
    //   [O, O, O, O, O, O, R, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, R, O, O, O, O, O, R, R, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, R, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, R, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, R, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, R, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, R, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, R, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, R, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, R, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, R, O, O, O, O, O, O, O, O, O,O],
    //   [O, O, O, O, O, O, O, O, O, O, O, O,O]
    // ];

    // uint32 height = uint32(map.length);
    // uint32 width = uint32(map[0].length);
    // bytes memory terrain = new bytes(width * height);

    // for (uint32 y = 0; y < height; y++) {
    //   for (uint32 x = 0; x < width; x++) {

    //     int32 positionX = int32(x);
    //     int32 positionY = int32(y);

    //     TerrainType terrainType = map[y][x];
    //     if (terrainType == TerrainType.None) continue;

    //     terrain[(y * width) + x] = bytes1(uint8(terrainType));

    //     bytes32 entity = positionToEntityKey(x,y);

    //     if(terrainType == TerrainType.Rock) {
    //       Rock.set(world, entity, 5, RockType.Statumen);
    //       Position.set(world, entity, positionX, positionY);
    //       Obstruction.set(world, entity, true);
    //     }
    //   }
    // }

    // MapConfig.set(world, width, height, terrain);
  }
}
