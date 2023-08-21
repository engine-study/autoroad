// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";
import { GameState, GameConfig, GameConfigData, MapConfig, RoadConfig, Chunk, Bounds, Boulder } from "../codegen/Tables.sol";
import { Road, Move, Player, Rock, Health, Carriage, Coinage, Weight, Stats } from "../codegen/Tables.sol";
import { Position, PositionData, PositionTableId, Tree, Seeds } from "../codegen/Tables.sol";

import { SpawnSystem } from "./SpawnSystem.sol";
import { ChunkTableId } from "../codegen/Tables.sol";
import { TerrainType, RockType, RoadState, MoveType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { positionToEntityKey, position3DToEntityKey } from "../utility/positionToEntityKey.sol";
import { randomCoord } from "../utility/random.sol";
import { MoveSystem } from "./MoveSystem.sol";
import { RewardSubsystem } from "./RewardSubsystem.sol";
import { getUniqueEntity } from "@latticexyz/world/src/modules/uniqueentity/getUniqueEntity.sol";

contract RoadSubsystem is System {
  //updateRow
  //finishRow

  function onRoad(int32 x, int32 y) public returns (bool) {
    // bound to map
    (uint32 width, uint32 height, int32 left, int32 right) = RoadConfig.get();
    return x >= int32(left) && x <= right;
  }

  function createMile(int32 mileNumber) public {

    require(mileNumber != GameState.getMiles(), "creating mile again");

    if(mileNumber > 0) {
      contemplateMile(mileNumber - 1);
    }

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
    GameConfigData memory config = GameConfig.get();

    //spawn all the rows
    //spawn all the obstacles
    //spawn all the rocks/resources
    for (int32 y = yStart; y <= yEnd; y++) {
      //SPAWN TERRAIN
      for (int32 x = int32(-playArea); x <= playArea; x++) {
        //set the terrain type to empty
        TerrainType terrainType = TerrainType.None;

        uint noiseCoord = randomCoord(0, 100, x, y);

        // console.log("noise ", noiseCoord);

        if (noiseCoord <= 7) {
          terrainType = TerrainType.Tree;
        } else if (noiseCoord > 10 && noiseCoord <= 15) {
          terrainType = TerrainType.Rock;
        } else if (noiseCoord == 15) {
          terrainType = TerrainType.HeavyBoy;
        } else if (noiseCoord == 16) {
          if(IWorld(_world()).onRoad(x, y)) {continue;}
          terrainType = TerrainType.Obstruction;
        } else if (noiseCoord == 17) {
          if(IWorld(_world()).onRoad(x, y)) {continue;}
          terrainType = TerrainType.Pillar;
        } else if (config.dummyPlayers && noiseCoord > 98) {
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
    Chunk.set(chunkEntity, false, mileNumber, 0,0);
    Position.set(keccak256(abi.encode("Carriage")), 0, yEnd + 1, 0);

  }

  function contemplateMile(int32 mileNumber) public {

    GameConfigData memory config = GameConfig.get();
    (,uint32 roadHeight, int32 leftRoad, int32 rightRoad) = RoadConfig.get();
    int32 yStart = mileNumber * int32(roadHeight);
    int32 yEnd = int32(yStart) + int32(roadHeight) + -1;

    for (int32 y = yStart; y <= yEnd; y++) {

      for (int32 x = leftRoad; x <= rightRoad; x++) {

        bytes32 road = keccak256(abi.encode("Road", x, y));
        if(Road.getState(road) < uint32(RoadState.Paved)) {
          continue;
        }

        uint noiseCoord = randomCoord(0, 100, x, y);

        if (noiseCoord < 5) {
          IWorld(_world()).giveRoadReward(road);
        }
      }
    }

  }

  function spawnTerrain(int32 x, int32 y, TerrainType tType) public {
    IWorld world = IWorld(_world());

    bytes32 entity = getUniqueEntity();
    // bytes32 entity = keccak256(abi.encode("Terrain", x, y));
    Position.set(entity, x, y, 0);

    if (tType == TerrainType.Rock) {
      Rock.set(entity, uint32(RockType.Raw));
      Weight.set(entity, 1);
      Move.set(entity, uint32(MoveType.Obstruction));
    } else if (tType == TerrainType.HeavyBoy) {
      Boulder.set(entity, true);
      Weight.set(entity, 3);
      Move.set(entity, uint32(MoveType.Push));
    } else if (tType == TerrainType.Obstruction) {
      Boulder.set(entity, true);
      Weight.set(entity, 5);
      Move.set(entity, uint32(MoveType.Push));
    } else if (tType == TerrainType.Pillar) {
      Boulder.set(entity, true);
      Weight.set(entity, 99);
      Move.set(entity, uint32(MoveType.Obstruction));
    } else if (tType == TerrainType.Tree) {
      Tree.set(entity, true);
      Health.set(entity, 1);
      Move.set(entity, uint32(MoveType.Obstruction));
    } else if (tType == TerrainType.Player) {
      world.spawnBotAdmin(x, y, entity);
    }
  }


  function spawnRoad(bytes32 player, bytes32 pushed, bytes32 road, PositionData memory pos) public {

    //ROAD COMPLETE!!! set it underground
    Position.set(road, pos.x, pos.y, -1);
    //set the rock to the position under the road
    Position.set(pushed, pos.x, pos.y, -2);
    bool pushedPlayer = Player.get(pushed);

    // bool isRock = Rock.get(atDestination[0]);
    if (pushedPlayer) {
      Road.setState(road, uint32(RoadState.Bones));
    } else {
      Road.setState(road, uint32(RoadState.Paved));
    }

    //reward the player
    Road.setFilled(road, player);
    int32 coins = Coinage.get(player);
    Coinage.set(player, coins + 5);

    updateChunk();

    int32 stat = Stats.getCompleted(player);
    Stats.setCompleted(player, stat + 1);
  }

  function updateChunk() public {
    (int32 currentMile, ) = GameState.get();
    bytes32 chunk = keccak256(abi.encode("Chunk", currentMile));

    uint32 pieces = Chunk.getPieces(chunk);
    (uint32 roadWidth, uint32 roadHeight, , ) = RoadConfig.get();

    pieces++;

    //road complete!
    if (pieces >= (roadWidth * roadHeight)) {
      finishChunk(chunk, currentMile, pieces);
    } else {
      Chunk.set(chunk, false, currentMile, pieces, 0);
    }
  }

  function finishChunk(bytes32 chunk, int32 currentMile, uint32 pieces) public {
    Chunk.set(chunk, true, currentMile, pieces, block.number);
    createMile(currentMile + 1);
  }

  function spawnFinishedRoad(bytes32 credit, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.onRoad(x, y), "off road");

    bytes32 entity = keccak256(abi.encode("Road", x, y));
    Position.set(entity, x, y, -1);
    bool giveReward = randomCoord(0,100, x, y) > 90;
    Road.set(entity, uint32(RoadState.Paved), credit, giveReward);

    updateChunk();
  }

  function spawnShoveledRoad(int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.onRoad(x, y), "off road");

    bytes32 entity = keccak256(abi.encode("Road", x, y));
    require(Road.getState(entity) == uint32(RoadState.None), "road");

    Road.set(entity, uint32(RoadState.Shoveled), entity, false);
    Position.set(entity, x, y, 0);
  }

  function debugMile(bytes32 credit) public {
    (, uint32 roadHeight, int32 left, int32 right) = RoadConfig.get();
    int32 currentMile = GameState.getMiles();

    int32 yStart = int32(currentMile * int32(roadHeight));
    int32 yEnd = yStart + int32(roadHeight);

    for (int32 y = yStart; y < yEnd; y++) {
      for (int32 x = left; x <= right; x++) {
        spawnFinishedRoad(credit, x, y);
      }
    }
  }
}
