// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";

import { GameState, GameConfig, GameConfigData, MapConfig, RoadConfig, Chunk, Bounds, Boulder } from "../codegen/index.sol";
import { Road, Move, Player, Rock, Health, Carriage, Coinage, Weight, Stats, Entities, NPC } from "../codegen/index.sol";
import { Position, PositionData, PositionTableId, Tree, Seeds, Row } from "../codegen/index.sol";
import { TerrainType, RockType, RoadState, MoveType, NPCType } from "../codegen/common.sol";

import { MoveSubsystem } from "./MoveSubsystem.sol";
import { RewardSubsystem } from "./RewardSubsystem.sol";
import { EntitySubsystem } from "./EntitySubsystem.sol";
import { FloraSubsystem } from "./FloraSubsystem.sol";
import { SpawnSubsystem } from "./SpawnSubsystem.sol";

import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { getUniqueEntity } from "@latticexyz/world-modules/src/modules/uniqueentity/getUniqueEntity.sol";
import { randomCoord } from "../utility/random.sol";
import { Rules } from "../utility/rules.sol";

contract TerrainSubsystem is System {
  //updateRow
  //finishRow


  function sup() public {
    console.log("sup");
  }

  function createWorld() public {

    console.log("creating world");

    bool debug = true; 
    bool dummyPlayers = true; 
    bool roadComplete = true; 

    GameState.set(int32(-1), 0);
    GameConfig.set(debug, dummyPlayers, roadComplete);
    MapConfig.set(10, 10, 13);
    RoadConfig.set(1, 0, 0);
    Bounds.set(0, 0, int32(-1), 1);
    Row.set(int32(-1));

    bytes32 carriage = getCarriageEntity();
    Carriage.set(carriage, true);
    Position.set(carriage, 0, -1, 0);

  }


  function createMile() public {

    console.log("creating mile");

    int32 mile = GameState.getMiles();
    bytes32 oldChunk = getChunkEntity(mile);

    console.log("old mile");
    console.logInt(mile);

    require(mile == -1 || Chunk.getCompleted(oldChunk) == true , "fatal, mile not complete");

    mile++;

    console.log("new mile");
    console.logInt(mile);

    //TODO simple setter
    GameState.set(mile, 0);
    // GameState.setMiles(mile);

    //move carriage to top of mile
    int32 height = MapConfig.getPlayHeight();
    Position.set(getCarriageEntity(), 0, ((mile+1) * height) + 1, 0);

    //create the chunk
    bytes32 newChunk = getChunkEntity(mile);
    Chunk.set(newChunk, mile, false, false,  0, 0);

  }

  function summonMile(bytes32 causedBy, bool summonAll) public {

    console.log("summonMile");
    console.log("lets go");

    //check if chunk is already spawned
    int32 mile = GameState.getMiles();
    bytes32 chunkEntity = getChunkEntity(mile);
    
    require(mile > -1, "fatal, mile not created");
    require(Chunk.getMile(chunkEntity) == mile, "fatal, mile number doesn't match");
    require(Chunk.getCompleted(chunkEntity) == false, "fatal, already completed");
    require(Chunk.getSpawned(chunkEntity) == false, "fatal, already spawned");

    //calculate new map bounds based on chunk
    //eventually maybe should use Bounds to calculate so we can add rest sections?

    (int32 left, int32 right, int32 up, int32 down) = Rules.getMileBounds(mile);
    int32 row = Row.get();

    if(summonAll) {
      while(row < up) {row = summonRow(causedBy, left, right, up, down);}
    } else {
      //summon another row, until we have summoned all of them
      row = summonRow(causedBy, left, right, up, down);
    }

    //complete the mile, set the new bounds
    if(row < up) {return;}
    mileIsReady(causedBy, mile, left, right, up, down);

  }

  function mileIsReady(bytes32 causedBy, int32 mile, int32 left, int32 right, int32 up, int32 down) private {

    console.log("mile ready");

    console.log("create puzzle");
    SystemSwitch.call(abi.encodeCall(IWorld(_world()).createRandomPuzzle, (causedBy, right, up, down)));
    
    //set bounds 
    Bounds.set(left, right, up, down);

    console.log("set chunk");
    bytes32 chunkEntity = getChunkEntity(mile);
    Chunk.set(chunkEntity, mile, true, false, 0, 0);
  }

  function summonRow(bytes32 causedBy, int32 left, int32 right, int32 up, int32 down) public returns(int32 row) {

    console.log("summon row");

    row = Row.get();
    row++;

    spawnRow(causedBy, right, row);

    Row.set(row);
    return row;

  }
    
  function spawnRow(bytes32 causedBy, int32 width, int32 y) private {
    
    console.log("spawn row");

    IWorld world = IWorld(_world());

    GameConfigData memory config = GameConfig.get();

    // console.log("noise ", noiseCoord);
    //TODO optimise gas, we're rolling a dice for every tile,
    //lets just use a random function to decide how many objects to spawn per row
    //and then roll for that tile alone
    // uint objects = randomCoord(0, 100, x, y) > 90;

    //SPAWN TERRAIN
    for (int32 x = int32(-width); x <= width; x++) {

      uint noiseCoord = randomCoord(0, 2000, x, y);

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
          if (Rules.onRoad(x, y)) { continue; }
          terrainType = TerrainType.HeavyHeavyBoy;
        } else if (noiseCoord > 300 && noiseCoord <= 325) {
          if (Rules.onRoad(x, y)) { continue; }
          terrainType = TerrainType.Pillar;
        } 
        if(terrainType != TerrainType.None) {
          spawnTerrain(causedBy, x, y, terrainType);
        }
      } else {

        //NPCS
        NPCType npcType = NPCType.None;

        if (noiseCoord > 1000 && noiseCoord <= 1010) {
          npcType = NPCType.Ox;
        } else if (noiseCoord > 1100 && noiseCoord <= 1150) {
          npcType = NPCType.Soldier;
        } else if (noiseCoord > 1200 && noiseCoord < 1250) {
          npcType = NPCType.Barbarian;
        } else if (noiseCoord > 1300 && noiseCoord <= 1350) {
          npcType = NPCType.BarbarianArcher;
        } else if (config.dummyPlayers && noiseCoord > 1500 && noiseCoord <= 1550) {
          npcType = NPCType.Player;
        }

        if(npcType != NPCType.None) {
          SystemSwitch.call(abi.encodeCall(world.spawnNPC, (causedBy, x, y, npcType)));
        }
      }
    
    }
  }

  function getRoadEntity(int32 x, int32 y) public pure returns(bytes32) {return keccak256(abi.encode("Road", x, y));}
  function getChunkEntity(int32 mile) public pure returns(bytes32) {return keccak256(abi.encode("Chunk", mile));}
  function getCarriageEntity() public pure returns(bytes32) {return keccak256(abi.encode("Carriage"));}

  function contemplateMile(int32 mileNumber) public {
    
    (int32 left, int32 right, int32 up, int32 down) = Rules.getMileBounds(mileNumber);

    for (int32 y = down; y <= up; y++) {
      for (int32 x = left; x <= right; x++) {
        bytes32 road = getRoadEntity(x,y);

        if (Road.getState(road) < uint32(RoadState.Paved)) { continue; }

        uint noiseCoord = randomCoord(0, 100, x, y);

        //TODO golf and fix
        if (noiseCoord < 5) {
          SystemSwitch.call(abi.encodeCall(IWorld(_world()).giveRoadReward, (road)));
        }
      }
    }
  }

  function spawnTerrain(bytes32 player, int32 x, int32 y, TerrainType tType) public {

    console.log("spawn terrain");

    IWorld world = IWorld(_world());
    bytes32 entity = getUniqueEntity();

    if (tType == TerrainType.Rock) {
      Rock.set(entity, uint32(RockType.Raw));
      Weight.set(entity, 1);
      Move.set(entity, uint32(MoveType.Obstruction));
    } else if (tType == TerrainType.Tree) {
      SystemSwitch.call(abi.encodeCall(world.spawnFlora, (player, entity, x, y)));
    } else if (tType == TerrainType.HeavyBoy) {
      Rock.set(entity, uint32(RockType.Heavy));
      Boulder.set(entity, true);
      Weight.set(entity, 3);
      Move.set(entity, uint32(MoveType.Push));
    } else if (tType == TerrainType.HeavyHeavyBoy) {
      Rock.set(entity, uint32(RockType.HeavyHeavy));
      Boulder.set(entity, true);
      Weight.set(entity, 5);
      Move.set(entity, uint32(MoveType.Push));
    } else if (tType == TerrainType.Pillar) {
      Rock.set(entity, uint32(RockType.Pillar));
      Boulder.set(entity, true);
      Weight.set(entity, 99);
      Move.set(entity, uint32(MoveType.Obstruction));
    }else if (tType == TerrainType.Road) {
      spawnRoadFromPlayer(player, 0, getRoadEntity(x,y), PositionData(x,y,0));
    } else if (tType == TerrainType.Hole) {
      spawnShoveledRoad(player, x, y);
    } 

    //get rid of this hack pls
    if(tType != TerrainType.None && tType != TerrainType.Road && tType != TerrainType.Hole) {
      Position.set(entity, x, y, 0);
    }

  }

  function deleteAtRequire(PositionData memory pos) public {
    IWorld world = IWorld(_world());
    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,pos.x, pos.y, pos.layer);
    require(atPosition.length > 0, "Nothing to delete");
    Position.deleteRecord(atPosition[0]);
  }

  function deleteAt(PositionData memory pos) public {
    IWorld world = IWorld(_world());
    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,pos.x, pos.y, pos.layer);
    if(atPosition.length > 0) Position.deleteRecord(atPosition[0]);
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
    require(Rules.onRoad(x, y), "off road");

    bytes32 road = getRoadEntity(x,y);
    Position.set(road, x, y, -1);
    //TODO setfilled to save gas
    // Road.setFilled(road, player);
    Road.set(road, uint32(state), player, false);
    Move.set(road, uint32(MoveType.None));
  }

  //TODO fix this horrible thing,make it more robust
  function updateChunk() public {
    int32 currentMile = GameState.getMiles();
    bytes32 chunk = keccak256(abi.encode("Chunk", currentMile));

    uint32 pieces = Chunk.getRoads(chunk);
    int32 playHeight = MapConfig.getPlayHeight();
    uint32 roadWidth = RoadConfig.getWidth();

    pieces++;

    //road complete!
    if (pieces >= (roadWidth * uint32(playHeight))) {
      finishMile(chunk, currentMile, pieces);
    } else {
      Chunk.set(chunk, currentMile, true, false, pieces, 0);
    }
  }

  function finishMile(bytes32 chunk, int32 currentMile, uint32 pieces) public {
    console.log("finish chunk");
    console.logInt(currentMile);

    Chunk.set(chunk, currentMile, true, true, pieces, block.number);
    contemplateMile(currentMile);

    currentMile += 1;

    createMile();
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
    IWorld world = IWorld(_world());

    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,x, y, 0);
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
