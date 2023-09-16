// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";
import { GameState, GameConfig, GameConfigData, MapConfig, RoadConfig, Chunk, Bounds, Boulder } from "../codegen/Tables.sol";
import { Road, Move, Player, Rock, Health, Carriage, Coinage, Weight, Stats, Entities, NPC } from "../codegen/Tables.sol";
import { Position, PositionData, PositionTableId, Tree, Seeds } from "../codegen/Tables.sol";

import { TerrainType, RockType, RoadState, MoveType, NPCType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { positionToEntityKey, position3DToEntityKey } from "../utility/positionToEntityKey.sol";
import { randomCoord } from "../utility/random.sol";
import { MoveSubsystem } from "./MoveSubsystem.sol";
import { RewardSubsystem } from "./RewardSubsystem.sol";
import { EntitySubsystem } from "./EntitySubsystem.sol";
import { FloraSubsystem } from "./FloraSubsystem.sol";
import { NPCSubsystem } from "./NPCSubsystem.sol";
import { SpawnSubsystem } from "./SpawnSubsystem.sol";

import { getUniqueEntity } from "@latticexyz/world/src/modules/uniqueentity/getUniqueEntity.sol";

contract TerrainSubsystem is System {
  //updateRow
  //finishRow

  function onRoad(int32 x, int32 y) public returns (bool) {
    // bound to map
    return x >= RoadConfig.getLeft() && x <= RoadConfig.getRight();
  }

  function createMile(int32 mileNumber) public {
    IWorld world = IWorld(_world());
    int32 currentMile = GameState.getMiles();

    console.log("create mile");
    console.logInt(mileNumber);
    console.log("current mile");
    console.logInt(currentMile);

    require(mileNumber > currentMile, "creating mile again");

    //create an entity for the chunk itself
    bytes32 chunkEntity = getChunkEntity(mileNumber);

    //TODO simple setter
    int32 players = GameState.getPlayerCount();
    GameState.set(mileNumber, players);
    // GameState.setMiles(mileNumber);

    //create the chunk
    Chunk.set(chunkEntity, false, mileNumber, 0, 0, false);

  }

  function createMileTerrain(bytes32 causedBy) public {

    //check if chunk is already spawned
    int32 mileNumber = GameState.getMiles();
    bytes32 chunkEntity = getChunkEntity(mileNumber);
    require(Chunk.getSpawned(chunkEntity) == false, "already spawned");


    int32 playWidth = MapConfig.getPlayWidth();
    int32 playHeight = MapConfig.getPlayHeight();
    int32 yStart = mileNumber * playHeight;
    int32 yEnd = int32(yStart) + playHeight + -1;

    //spawn the area
    IWorld world = IWorld(_world());
    world.createTerrain(causedBy, playWidth, yStart, yEnd);
    
    //set new game area
    //todo set single setters
    Chunk.set(chunkEntity, false, mileNumber, 0, 0, true);
    Bounds.set(int32(-playWidth), playWidth, yEnd, yStart);
    Position.set(getCarriageEntity(), 0, yEnd + 1, 0);

  }

  //create terrain using an area
  function createTerrain(bytes32 causedBy, int32 width, int32 down, int32 up) public {
    
    IWorld world = IWorld(_world());

    GameConfigData memory config = GameConfig.get();

    //spawn all the rows
    //spawn all the obstacles
    //spawn all the rocks/resources
    
    for (int32 y = down; y <= up; y++) {
      //SPAWN TERRAIN
      for (int32 x = int32(-width); x <= width; x++) {

        uint noiseCoord = randomCoord(0, 2000, x, y);

        // console.log("noise ", noiseCoord);

        if(noiseCoord < 1000) {
          //TERRAIN
          TerrainType terrainType = TerrainType.None;

          if (noiseCoord <= 100) {
            terrainType = TerrainType.Tree;
          } else if (noiseCoord > 100 && noiseCoord <= 200) {
            terrainType = TerrainType.Rock;
          } else if (noiseCoord > 200 && noiseCoord <= 225) {
            terrainType = TerrainType.HeavyBoy;
          } else if (noiseCoord > 225 && noiseCoord <= 250) {
            if (world.onRoad(x, y)) { continue;}
            terrainType = TerrainType.HeavyHeavyBoy;
          } else if (noiseCoord > 300 && noiseCoord <= 325) {
            if (world.onRoad(x, y)) { continue;}
            terrainType = TerrainType.Pillar;
          } 
          if(terrainType != TerrainType.None) {
            spawnTerrain(causedBy, x, y, terrainType);
          }
        } else {
          //NPCS
          NPCType npcType = NPCType.None;

          if (noiseCoord > 1000 && noiseCoord <= 1025) {
            npcType = NPCType.Ox;
          } else if (noiseCoord > 1100 && noiseCoord <= 1150) {
            npcType = NPCType.Soldier;
          } else if (noiseCoord > 1150 && noiseCoord < 1200) {
            npcType = NPCType.Barbarian;
          } else if (config.dummyPlayers && noiseCoord > 1200 && noiseCoord <= 1300) {
            npcType = NPCType.Player;
          }

          if(npcType != NPCType.None) {
            world.spawnNPC(causedBy, x, y, npcType);
          }
        }
      
      }
    }

  }

  function getRoadEntity(int32 x, int32 y) public returns(bytes32) {return keccak256(abi.encode("Road", x, y));}
  function getChunkEntity(int32 mile) public returns(bytes32) {return keccak256(abi.encode("Chunk", mile));}
  function getCarriageEntity() public returns(bytes32) {return keccak256(abi.encode("Carriage"));}

  function contemplateMile(int32 mileNumber) public {
    int32 playHeight = MapConfig.getPlayHeight();
    int32 leftRoad = RoadConfig.getLeft();
    int32 rightRoad = RoadConfig.getRight();
    int32 yStart = mileNumber * playHeight;
    int32 yEnd = int32(yStart) + playHeight + -1;

    for (int32 y = yStart; y <= yEnd; y++) {
      for (int32 x = leftRoad; x <= rightRoad; x++) {
        bytes32 road = getRoadEntity(x,y);

        if (Road.getState(road) < uint32(RoadState.Paved)) { continue; }

        uint noiseCoord = randomCoord(0, 100, x, y);

        if (noiseCoord < 5) {
          IWorld(_world()).giveRoadReward(road);
        }
      }
    }
  }

  function spawnTerrain(bytes32 player, int32 x, int32 y, TerrainType tType) public {
    
    IWorld world = IWorld(_world());
    bytes32 entity = getUniqueEntity();

    if (tType == TerrainType.Rock) {
      Rock.set(entity, uint32(RockType.Raw));
      Weight.set(entity, 1);
      Move.set(entity, uint32(MoveType.Obstruction));
    } else if (tType == TerrainType.Tree) {
      world.spawnFlora(player, entity, x, y);
    } else if (tType == TerrainType.HeavyBoy) {
      Boulder.set(entity, true);
      Weight.set(entity, 3);
      Move.set(entity, uint32(MoveType.Push));
    } else if (tType == TerrainType.HeavyHeavyBoy) {
      Boulder.set(entity, true);
      Weight.set(entity, 5);
      Move.set(entity, uint32(MoveType.Push));
    } else if (tType == TerrainType.Pillar) {
      Boulder.set(entity, true);
      Weight.set(entity, 99);
      Move.set(entity, uint32(MoveType.Obstruction));
    }else if (tType == TerrainType.Road) {
      spawnRoadFromPlayer(player, 0, getRoadEntity(x,y), PositionData(x,y,0));
    } else if (tType == TerrainType.Hole) {
      spawnShoveledRoad(player, x, y);
    } 

    //get rid of this hack pls
    if(tType != TerrainType.None && tType != TerrainType.Road && tType != TerrainType.Hole)
      Position.set(entity, x, y, 0);

  }

  function deleteAt(int32 x, int32 y, int32 layer) public {
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, layer));
    require(atPosition.length > 0, "empty");
    Position.deleteRecord(atPosition[0]);
  }

  function spawnRoadFromPlayer(bytes32 player, bytes32 pushed, bytes32 road, PositionData memory pos) public {

    bool pushedPlayer = Player.get(pushed);
    RoadState state = pushedPlayer ? RoadState.Bones : RoadState.Paved;

    spawnRoad(player, pos.x, pos.y, state);

    //hack for making objects push into the road
    Position.set(pushed, pos.x, pos.y, -2);

    //reward the player
    IWorld(_world()).giveRoadFilledReward(player);
    updateChunk();

  }

  function spawnRoad(bytes32 player, int32 x, int32 y, RoadState state) public {
    IWorld world = IWorld(_world());
    require(world.onRoad(x, y), "off road");

    bytes32 road = getRoadEntity(x,y);
    Position.set(road, x, y, -1);
    //TODO setfilled to save gas
    // Road.setFilled(road, player);
    Road.set(road, uint32(state), player, false);
    Move.set(road, uint32(MoveType.None));
  }

  //TODO fix this horrible thing,make it more robust
  function updateChunk() public {
    (int32 currentMile, ) = GameState.get();
    bytes32 chunk = keccak256(abi.encode("Chunk", currentMile));

    uint32 pieces = Chunk.getPieces(chunk);
    int32 playHeight = MapConfig.getPlayHeight();
    uint32 roadWidth = RoadConfig.getWidth();

    pieces++;

    //road complete!
    if (pieces >= (roadWidth * uint32(playHeight))) {
      finishMile(chunk, currentMile, pieces);
    } else {
      Chunk.set(chunk, false, currentMile, pieces, 0, true);
    }
  }

  function finishMile(bytes32 chunk, int32 currentMile, uint32 pieces) public {
    console.log("finish chunk");
    console.logInt(currentMile);

    Chunk.set(chunk, true, currentMile, pieces, block.number, true);
    contemplateMile(currentMile);

    currentMile += 1;

    createMile(currentMile);
  }

  function debugMile(bytes32 credit) public {
    (uint32 roadWidth, int32 left, int32 right) = RoadConfig.get();
    int32 currentMile = GameState.getMiles();
    int32 playHeight = MapConfig.getPlayHeight();

    int32 yStart = int32(currentMile * playHeight);
    int32 yEnd = yStart + playHeight;

    console.log("debug mile");
    console.logInt(currentMile);
    console.log("from");
    console.logInt(yStart);
    console.log("to");
    console.logInt(yEnd);

    for (int32 y = yStart; y < yEnd; y++) {
      for (int32 x = left; x <= right; x++) {
        spawnDebugRoad(credit, x, y);
      }
    }

    bytes32 chunk = getChunkEntity(currentMile);
    uint32 pieces = roadWidth * uint32(playHeight);

    finishMile(chunk, currentMile, pieces);
  }

  function spawnShoveledRoad(bytes32 player, int32 x, int32 y) public {

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));
    require(atPosition.length < 1, "trying to dig an occupied spot");

    bytes32 entity = getRoadEntity(x,y);
    require(Road.getState(entity) == uint32(RoadState.None), "road");

    //TODO setState
    // Road.setState(entity, uint32(RoadState.Shoveled));
    Road.set(entity, uint32(RoadState.Shoveled), 0, false);
    Move.set(entity, uint32(MoveType.Hole));
    Position.set(entity, x, y, 0);

    IWorld(_world()).giveRoadShoveledReward(player);
  }

  
  function spawnDebugRoad(bytes32 credit, int32 x, int32 y) public {
    bytes32 entity = getRoadEntity(x,y);
    bool giveReward = randomCoord(0, 100, x, y) > 90;
    Position.set(entity, x, y, -1);
    Road.set(entity, uint32(RoadState.Paved), credit, giveReward);
  }
}
