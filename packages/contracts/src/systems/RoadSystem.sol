// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, RoadConfig, Chunk, Position, PositionTableId, PositionData } from "../codegen/Tables.sol";
import { Player, Rock, Obstruction, Tree } from "../codegen/Tables.sol";
import { TerrainType, ObjectType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { positionToEntityKey } from "../utility/positionToEntityKey.sol";

contract RoadSystem is System {
  //updateRow
  //finishRow

  function createMile(uint32 mileNumber) public {

    //create an entity for the chunk itself
    bytes32 chunkEntity = keccak256(abi.encode("chunk", mileNumber));

    // MyTable.pushFooArray(keccak256("some.key"), 4242); // adds 4242 at end of fooArray
    // MyTable.popFooArray(keccak256("some.key")); // pop fooArray
    // MyTable.setItemFooArray(keccak256("some.key"), 0, 123); // set fooArray[0] to 123

    bytes32[] memory entitiesArray = new bytes32[](0);
    bytes32[] memory contributorsArray = new bytes32[](0);

    //create a new chunk
    (uint32 mapWidth, uint32 mapHeight, ) = MapConfig.get();
    (uint32 roadWidth, uint32 roadHeight) = RoadConfig.get();

    int32 heightStart = int32(mileNumber) * int32(roadHeight);

    //spawn all the rows
    //spawn all the obstacles
    //spawn all the rocks/resources

    for (uint32 y = 0; y < roadHeight; y++) {

      TerrainType[] memory map = new TerrainType[](roadWidth);

      for (uint32 x = 0; x < roadWidth; x++) {

        int32 positionX = int32(x);
        int32 positionY = int32(y) + heightStart;

        //set the terrain type to empty
        TerrainType terrainType = TerrainType.None;

        if (x == 3) {
            terrainType = TerrainType.Tree;
        } else if (x == 0) {
           terrainType = TerrainType.Rock;
        }

        //don't spawn anything
        if(terrainType == TerrainType.None) {
          continue;
        }

        spawnTerrain(int32(x), int32(y) + heightStart, terrainType);

      }
    }

    //set the chunk of road
    Chunk.set(chunkEntity, false, mileNumber, entitiesArray, contributorsArray);

  }

   function spawnTerrain(int32 x, int32 y, TerrainType tType) public {

        bytes32 entity = positionToEntityKey(x, y);
        
        Position.set(entity, x, y);
        Obstruction.set(entity, true);

        if(tType == TerrainType.Rock) {
            Rock.set(entity, 5, ObjectType.Statumen);
        } else if(tType == TerrainType.Tree) {
            Tree.set(entity, true);
        } else if(tType == TerrainType.Mine) {

        }
    }

}
