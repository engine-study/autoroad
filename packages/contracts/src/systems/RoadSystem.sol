// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";
import { GameState, GameConfig, GameConfigData, MapConfig, RoadConfig, Chunk, Position, PositionTableId, PositionData, Bounds} from "../codegen/Tables.sol";
import { Road, Player, Rock, Obstruction, Tree, Pushable } from "../codegen/Tables.sol";
import { Move } from "../codegen/Tables.sol";
import { TerrainType, RockType, RoadState, MoveType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { positionToEntityKey, position3DToEntityKey } from "../utility/positionToEntityKey.sol";
import { randomCoord } from "../utility/random.sol";

contract RoadSystem is System {
  //updateRow
  //finishRow

  
  function onRoad(int32 x, int32 y) public returns (bool) {
    // bound to map
    (uint32 width, uint32 height, int32 left, int32 right) = RoadConfig.get();
    return x >= int32(left) && x <= right;
  }

  function createMile(uint32 mileNumber) public {
    //create an entity for the chunk itself
    bytes32 chunkEntity = keccak256(abi.encode("Chunk", mileNumber));

    GameState.set(mileNumber);

    // MyTable.pushFooArray(keccak256("some.key"), 4242); // adds 4242 at end of fooArray
    // MyTable.popFooArray(keccak256("some.key")); // pop fooArray
    // MyTable.setItemFooArray(keccak256("some.key"), 0, 123); // set fooArray[0] to 123

    bytes32[] memory entitiesArray = new bytes32[](0);
    bytes32[] memory contributorsArray = new bytes32[](0);

    //create a new chunk
    (uint32 mapWidth,,) = MapConfig.get();
    (uint32 roadWidth, uint32 roadHeight,,) = RoadConfig.get();


    int32 yStart = int32(mileNumber) * int32(roadHeight);
    int32 yEnd = (int32(mileNumber) * int32(roadHeight)) + int32(roadHeight);
    int32 halfWidth = int32(mapWidth) / int32(2);
    
    Bounds.set(int32(-halfWidth), halfWidth, yEnd, 0);

    //spawn all the rows
    //spawn all the obstacles
    //spawn all the rocks/resources

    for (int32 y = yStart; y < int32(roadHeight) + int32(yStart); y++) {

      //SPAWN TERRAIN
      for (int32 x = int32(-halfWidth); x <= halfWidth; x++) {
        //set the terrain type to empty
        TerrainType terrainType = TerrainType.None;
        GameConfigData memory config = GameConfig.get();

        // //spawn the road
        // not doing this anymore, instead players will spawn the road by digging
        // if (x >= int32(-halfRoad) && x <= halfRoad) {
        //   bytes32 entity = position3DToEntityKey(x, -1, y);
        //   Road.set(entity, RoadState.None);
        //   Position.set(entity, x, y);
        // }  

        uint noiseCoord = randomCoord(0, 100, x, y);

        // console.log("noise ", noiseCoord);

        if (noiseCoord < 2) {
          terrainType = TerrainType.Tree;
        } else if (noiseCoord < 8) {
          terrainType = TerrainType.Rock;
        } else if (config.dummyPlayers && noiseCoord == 50) {
          terrainType = TerrainType.Player;
        } else if (config.stressTest && noiseCoord < 50) {}

        //don't spawn anything
        if (terrainType == TerrainType.None) {
          continue;
        }

        spawnTerrain(x, y, terrainType);
      }
    }

    //set the chunk of road
    // Chunk.set(chunkEntity, false, mileNumber, entitiesArray, contributorsArray);
    Chunk.set(chunkEntity, false, mileNumber);
    console.log("added mile ", mileNumber);
  }

  function spawnTerrain(int32 x, int32 y, TerrainType tType) public {
    bytes32 entity = positionToEntityKey(x, y);

    Position.set(entity, x, y);

    if (tType == TerrainType.Rock) {
      Rock.set(entity, 1);
      Move.set(entity, MoveType.Push);
      Pushable.set(entity,true);
    } else if (tType == TerrainType.Tree) {
      Tree.set(entity, true);
      Obstruction.set(entity, true);
    } else if (tType == TerrainType.Player) {
      Player.set(entity, true);
      Move.set(entity, MoveType.Push);
      Pushable.set(entity,true);

    }
  }
}
