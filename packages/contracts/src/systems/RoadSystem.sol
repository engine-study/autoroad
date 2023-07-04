// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, RoadConfig, Chunk, Position, PositionTableId, Player, PositionData } from "../codegen/Tables.sol";
import { TerrainType, ObjectType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions } from "../utility/grid.sol";

contract RoadSystem is System {
  //updateRow
  //finishRow

  function createMile(uint32 mileNumber) public {
    bytes32 chunkEntity = keccak256(abi.encode("chunk", mileNumber));

    TerrainType O = TerrainType.None;
    TerrainType R = TerrainType.Rock;
    TerrainType M = TerrainType.Mine;

    // TerrainType[][] memory map = new TerrainType[](0);

    // MyTable.pushFooArray(keccak256("some.key"), 4242); // adds 4242 at end of fooArray
    // MyTable.popFooArray(keccak256("some.key")); // pop fooArray
    // MyTable.setItemFooArray(keccak256("some.key"), 0, 123); // set fooArray[0] to 123

    bytes32[] memory entitiesArray = new bytes32[](0);
    bytes32[] memory contributorsArray = new bytes32[](0);

    //create a new chunk
    (uint32 mapWidth, uint32 mapHeight, ) = MapConfig.get();
    (uint32 roadWidth, uint32 roadHeight ) = RoadConfig.get();

    //spawn all the rows
    //spawn all the obstacles
    //spawn all the rocks/resources

    //move through rows
    for (uint32 i = 0; i < roadHeight; i++) {

        //move through columns of stones on road
        for (uint32 j = 0; j < roadWidth; j++) {

        }
    }

    //set the chunk of road
    Chunk.set(chunkEntity, mileNumber, entitiesArray, contributorsArray);


  }
}
