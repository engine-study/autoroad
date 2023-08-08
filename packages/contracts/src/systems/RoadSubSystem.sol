// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";
import { GameState, GameConfig, GameConfigData, MapConfig, RoadConfig, Chunk, Bounds } from "../codegen/Tables.sol";
import { Road, Move, Player, Rock, Health, Carriage } from "../codegen/Tables.sol";
import { Position, PositionTableId, Tree, Seeds } from "../codegen/Tables.sol";

import { SpawnSystem } from "./SpawnSystem.sol";
import { ChunkTableId } from "../codegen/Tables.sol";
import { TerrainType, RockType, RoadState, MoveType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { positionToEntityKey, position3DToEntityKey } from "../utility/positionToEntityKey.sol";
import { randomCoord } from "../utility/random.sol";
import { MoveSystem } from "./MoveSystem.sol";
import { getUniqueEntity } from "@latticexyz/world/src/modules/uniqueentity/getUniqueEntity.sol";

contract RoadSubSystem is System {
  //updateRow
  //finishRow

  function onRoad(int32 x, int32 y) public returns (bool) {
    // bound to map
    (uint32 width, uint32 height, int32 left, int32 right) = RoadConfig.get();
    return x >= int32(left) && x <= right;
  }

  function spawnFinishedRoad(int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.onRoad(x, y), "off road");
    bytes32 entity = keccak256(abi.encode("Road", x, y));
    Road.set(entity, uint32(RoadState.Paved), entity);
    Position.set(entity, x, y);
  }

  function spawnShoveledRoad(int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.onRoad(x, y), "off road");
    bytes32 entity = keccak256(abi.encode("Road", x, y));
    Road.set(entity, uint32(RoadState.Shoveled), entity);
    Position.set(entity, x, y);
  }

  // function debugMile(int32 mile) public {
  //   //create a new chunk
  //   (int32 playArea, int32 spawnArea) = MapConfig.get();
  //   (uint32 roadWidth, uint32 roadHeight, , ) = RoadConfig.get();

  //   int32 yStart = mile * int32(roadHeight);
  //   int32 yEnd = yStart + 2;

  //   for (int32 y = yStart; y < yEnd; y++) {
  //     //SPAWN TERRAIN
  //     for (int32 x = int32(-playArea); x <= playArea; x++) {
  //       if (x < playArea || y == yStart) {
  //         spawnFinishedRoadAdmin(x, y);
  //       } else {
  //         spawnShoveledRoadAdmin(x, y);
  //       }
  //     }
  //   }
  // }

  function createMile(int32 mileNumber) public {
    bytes32[] memory atPosition = getKeysWithValue(ChunkTableId, Chunk.encode(true, int32(mileNumber - 1)));
    require(mileNumber == 0 || atPosition.length > 0, "cannot create a new mile until the active one is complete");

    //create an entity for the chunk itself
    bytes32 chunkEntity = keccak256(abi.encode("Chunk", mileNumber));

    (, int32 players) = GameState.get();
    GameState.set(mileNumber, players);
    // GameState.setMiles(mileNumber);

    // MyTable.pushFooArray(keccak256("some.key"), 4242); // adds 4242 at end of fooArray
    // MyTable.popFooArray(keccak256("some.key")); // pop fooArray
    // MyTable.setItemFooArray(keccak256("some.key"), 0, 123); // set fooArray[0] to 123

    bytes32[] memory entitiesArray = new bytes32[](0);
    bytes32[] memory contributorsArray = new bytes32[](0);

    //create a new chunk
    (int32 playArea, int32 spawnArea) = MapConfig.get();
    (uint32 roadWidth, uint32 roadHeight, , ) = RoadConfig.get();

    //the start of the curret mile
    int32 yStart = mileNumber * int32(roadHeight);
    //the end of the current mile
    int32 yEnd = int32(yStart) + int32(roadHeight) + -1;

    Bounds.set(int32(-playArea), playArea, yEnd, yStart);

    //spawn all the rows
    //spawn all the obstacles
    //spawn all the rocks/resources

    for (int32 y = yStart; y <= yEnd; y++) {
      //SPAWN TERRAIN
      for (int32 x = int32(-playArea); x <= playArea; x++) {
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

        if (noiseCoord < 4) {
          terrainType = TerrainType.Tree;
        } else if (noiseCoord < 10) {
          terrainType = TerrainType.Rock;
        } else if (config.dummyPlayers && noiseCoord == 50) {
          terrainType = TerrainType.Player;
        }

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
    Position.set(keccak256(abi.encode("Carriage")), 0, yEnd + 1);
    // console.log("added mile ", mileNumber);
  }

  function spawnTerrain(int32 x, int32 y, TerrainType tType) public {
    IWorld world = IWorld(_world());

    bytes32 entity = getUniqueEntity();
    // bytes32 entity = keccak256(abi.encode("Terrain", x, y));
    Position.set(entity, x, y);

    if (tType == TerrainType.Rock) {
      Rock.set(entity, uint32(RockType.Raw));
      Move.set(entity, uint32(MoveType.Obstruction));
    } else if (tType == TerrainType.Tree) {
      Tree.set(entity, true);
      Health.set(entity, 1);
      Move.set(entity, uint32(MoveType.Obstruction));
    } else if (tType == TerrainType.Player) {
      world.spawnBotAdmin(x, y, entity);
    }
  }


}