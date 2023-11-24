// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";

import { GameState, GameConfig, GameConfigData, MapConfig, RoadConfig, Chunk, Bounds, Entities } from "../codegen/index.sol";
import { Road, Move, Player, Rock, Health, Carriage, Coinage, Weight, Stats, NPC, WorldColumn } from "../codegen/index.sol";
import { Position, PositionData, PositionTableId, Tree, Seeds, Row } from "../codegen/index.sol";
import { TerrainType, RockType, RoadState, MoveType, NPCType, FloraType} from "../codegen/common.sol";

import { Actions } from "../utility/actions.sol";
import { Rules } from "../utility/rules.sol";
import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { getUniqueEntity } from "@latticexyz/world-modules/src/modules/uniqueentity/getUniqueEntity.sol";
import { randomCoord, randomFromEntity } from "../utility/random.sol";

contract TerrainSubsystem is System {
  //updateRow
  //finishRow
  
  function createWorld() public {

    console.log("creating world");

    bool debug = true; 
    bool dummyPlayers = false; 
    bool roadComplete = true; 

    GameState.set(int32(-1), 0);
    GameConfig.set(debug, dummyPlayers);
    MapConfig.set(5, 10, 8);
    RoadConfig.set(1, 0, 0);
    Bounds.set(0, 0, int32(-1), 1);
    Row.set(int32(-1));

    bytes32 worldColumn = Actions.getWorldColumnEntity();
    WorldColumn.set(worldColumn, true);
    Position.set(worldColumn, 0,-10,0);

    bytes32 carriage = Actions.getCarriageEntity();
    Carriage.set(carriage, true);
    Position.set(carriage, 0, -1, 0);

  }


  function createMile() public {

    console.log("creating mile");

    int32 mile = GameState.getMiles();
    bytes32 oldChunk = Actions.getChunkEntity(mile);

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
    Position.set(Actions.getCarriageEntity(), 0, ((mile+1) * height) + 1, 0);

    //create the chunk
    bytes32 newChunk = Actions.getChunkEntity(mile);
    Chunk.set(newChunk, mile, false, false,  0, 0);

  }

  function summonMile(bytes32 causedBy, bool summonAll) public {

    console.log("summonMile");
    console.log("lets go");

    //check if chunk is already spawned
    int32 mile = GameState.getMiles();
    bytes32 chunkEntity = Actions.getChunkEntity(mile);
    
    require(mile > -1, "fatal, mile not created");
    require(Chunk.getMile(chunkEntity) == mile, "fatal, mile number doesn't match");
    require(Chunk.getCompleted(chunkEntity) == false, "fatal, already completed");
    require(Chunk.getSpawned(chunkEntity) == false, "fatal, already spawned");

    //calculate new map bounds based on chunk
    //eventually maybe should use Bounds to calculate so we can add rest sections?

    (int32 left, int32 right, int32 up, int32 down) = Rules.getMileBounds(mile);
    int32 row = Row.get();
    uint256 difficulty = uint(uint32(mile % 5));

    if(summonAll) {
      while(row < up) {row = summonRow(causedBy, left, right, difficulty);}
    } else {
      //summon another row, until we have summoned all of them
      row = summonRow(causedBy, left, right, difficulty);
    }

    //complete the mile, set the new bounds
    if(row < up) {return;}

    mileIsReady(causedBy, mile, left, right, up, down, difficulty);

  }

  function mileIsReady(bytes32 causedBy, int32 mile, int32 left, int32 right, int32 up, int32 down, uint difficulty) private {

    console.log("mile ready");

    console.log("create puzzle");
    SystemSwitch.call(abi.encodeCall(IWorld(_world()).createMiliarium, (causedBy, right, up, down)));
    SystemSwitch.call(abi.encodeCall(IWorld(_world()).createStatuePuzzle, (causedBy, right, up, down)));
    SystemSwitch.call(abi.encodeCall(IWorld(_world()).createStatuePuzzle, (causedBy, right, up, down)));
    SystemSwitch.call(abi.encodeCall(IWorld(_world()).createTickers, (causedBy, right, up, down)));

    //set bounds 
    Bounds.set(left, right, up, down);

    //reset ticking entities
    // Entities.setEntities(new bytes32[](0));

    console.log("set chunk");
    bytes32 chunkEntity = Actions.getChunkEntity(mile);
    Chunk.set(chunkEntity, mile, true, false, 0, 0);
  }

  function summonRow(bytes32 causedBy, int32 left, int32 right, uint difficulty) public returns(int32 row) {

    console.log("summon row");

    row = Row.get();
    row++;

    spawnRow(causedBy, right, row, difficulty);
    spawnEmptyRoad(0,row);

    Row.set(row);
    return row;

  }
    
  function spawnRow(bytes32 causedBy, int32 width, int32 y, uint difficulty) private {
    
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

        if (noiseCoord <= 150) {
          terrainType = TerrainType.Tree;
        } else if (noiseCoord > 200 && noiseCoord <= 400) {
          terrainType = TerrainType.Rock;
        } else if (noiseCoord > 500 && noiseCoord <= 600 + difficulty * 5) {
          terrainType = TerrainType.HeavyBoy;
        } else if (noiseCoord > 700 && noiseCoord <= 750 + difficulty * 10) {
          if (Rules.onRoad(x, y)) { continue; }
          terrainType = TerrainType.HeavyHeavyBoy;
        } else if (noiseCoord > 900 && noiseCoord <= 950 + difficulty * 10) {
          if (Rules.onRoad(x, y)) { continue; }
          terrainType = TerrainType.Pillar;
        } 
        if(terrainType != TerrainType.None) {
          spawnTerrain(causedBy, x, y, terrainType);
        }
      } else {

        //NPCS
        NPCType npcType = NPCType.None;

        if (noiseCoord > 1000 && noiseCoord <= 1100 - difficulty * 20) {
          npcType = NPCType.Ox;
        } else if (difficulty > 0 && noiseCoord > 1100 && noiseCoord <= 1200 - difficulty * 10) {
          npcType = NPCType.Soldier;
        } else if(difficulty > 1 && noiseCoord > 1200 && noiseCoord < 1300 + difficulty * 10) {
          npcType = NPCType.Barbarian;
        } else if (difficulty > 2 && noiseCoord > 1400 && noiseCoord <= 1450 + difficulty * 10) {
          npcType = NPCType.BarbarianArcher;
        } 
        // else if (config.dummyPlayers && noiseCoord > 1500 && noiseCoord <= 1550) {
        //   npcType = NPCType.Player;
        // }

        if(npcType != NPCType.None) {
          SystemSwitch.call(abi.encodeCall(world.spawnNPC, (causedBy, x, y, npcType)));
        }
      }
    
    }
  }

  function procProgression(int32 width, int32 y) private {

  }

  function contemplateMile(bytes32 causedBy, int32 mileNumber) public {
    
    (int32 left, int32 right, int32 up, int32 down) = Rules.getMileBounds(mileNumber);

    uint noiseCoord = randomFromEntity(uint(uint32(down)), uint(uint32(up)), causedBy);
    int32 noiseTile = int32(uint32(noiseCoord));
    require(noiseTile >= down && noiseTile <= up, "out of range");

    bytes32 road = Actions.getRoadEntity(0, noiseTile);

    require(Road.getState(road) == uint32(RoadState.Paved), "not paved");

    SystemSwitch.call(abi.encodeCall(IWorld(_world()).giveRoadLottery, (road)));

    // for (int32 y = down; y <= up; y++) {
    //   for (int32 x = left; x <= right; x++) {
    //     bytes32 road = Actions.getRoadEntity(x,y);

    //     if (Road.getState(road) < uint32(RoadState.Paved)) { continue; }

    //     uint noiseCoord = randomCoord(0, 100, x, y);

    //     //TODO golf and fix
    //     if (noiseCoord < 5) {
    //       SystemSwitch.call(abi.encodeCall(IWorld(_world()).giveRoadLottery, (road)));
    //     }
    //   }
    // }
  }

  function spawnTerrain(bytes32 player, int32 x, int32 y, TerrainType tType) public {

    console.log("spawn terrain");

    IWorld world = IWorld(_world());
    bytes32 entity = getUniqueEntity();

    Health.set(entity,1);

    if (tType == TerrainType.Rock) {
      // Rock.set(entity, uint32(RockType.Raw));
      // Move.set(entity, uint32(MoveType.Obstruction));
      Rock.set(entity, uint32(RockType.Statumen));
      Move.set(entity, uint32(MoveType.Push));
      Weight.set(entity, 1);
    } else if (tType == TerrainType.Trap) {
      SystemSwitch.call(abi.encodeCall(world.spawnFlora, (player, entity, x, y, FloraType.Bramble)));
    } else if (tType == TerrainType.Tree) {
      SystemSwitch.call(abi.encodeCall(world.spawnFloraRandom, (player, entity, x, y)));
    } else if (tType == TerrainType.HeavyBoy) {
      Rock.set(entity, uint32(RockType.Heavy));
      Weight.set(entity, 2);
      Move.set(entity, uint32(MoveType.Push));
    } else if (tType == TerrainType.HeavyHeavyBoy) {
      Rock.set(entity, uint32(RockType.HeavyHeavy));
      Weight.set(entity, 3);
      Move.set(entity, uint32(MoveType.Push));
    } else if (tType == TerrainType.Pillar) {
      Rock.set(entity, uint32(RockType.Pillar));
      Weight.set(entity, 99);
      Move.set(entity, uint32(MoveType.Obstruction));
    } else if (tType == TerrainType.Road) {
      spawnFinishedRoad(player, x, y, RoadState.Paved);
    } else if (tType == TerrainType.Hole) {
      spawnShoveledRoad(player, x, y);
    } 

    
    //get rid of this hack pls
    if(tType != TerrainType.None && tType != TerrainType.Road && tType != TerrainType.Hole) {
      Position.set(entity, x, y, 0);
    }

  }

  //TODO fix this horrible thing,make it more robust
  function updateChunk(bytes32 causedBy) public {
    int32 currentMile = GameState.getMiles();
    bytes32 chunk = keccak256(abi.encode("Chunk", currentMile));

    uint32 pieces = Chunk.getRoads(chunk);
    int32 playHeight = MapConfig.getPlayHeight();
    uint32 roadWidth = RoadConfig.getWidth();

    pieces++;

    //road complete!
    if (pieces >= (roadWidth * uint32(playHeight))) {
      finishMile(causedBy, chunk, currentMile, pieces);
    } else {
      Chunk.set(chunk, currentMile, true, false, pieces, 0);
    }
  }

  function finishMile(bytes32 causedBy, bytes32 chunk, int32 currentMile, uint32 pieces) public {
    console.log("finish chunk");
    console.logInt(currentMile);

    Chunk.set(chunk, currentMile, true, true, pieces, block.number);
    contemplateMile(causedBy, currentMile);

    currentMile += 1;

    createMile();
  }

  function debugMile(bytes32 credit) public {
    IWorld world = IWorld(_world());

    (, int32 left, int32 right) = RoadConfig.get();
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

        bytes32 road = Actions.getRoadEntity(x,y);
        uint32 roadState = Road.getState(road);
        if(roadState >= uint32(RoadState.Paved)) continue;
        spawnFinishedRoad(credit, x, y, RoadState.Paved);

      }

    }

  }

  function spawnEmptyRoad(int32 x, int32 y) public {
    bytes32 entity = Actions.getRoadEntity(x,y);
    Road.set(entity, uint32(RoadState.None), 0, false);
    Position.set(entity, x, y, -1);
  }

  function spawnShoveledRoad(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());

    bytes32 entity = Actions.getRoadEntity(x,y);
    require(Road.getState(entity) == uint32(RoadState.None), "road");

    //TODO setState
    Road.set(entity, uint32(RoadState.Shoveled), 0, false);
    Move.set(entity, uint32(MoveType.Hole));
    Position.set(entity, x, y, 0);

    SystemSwitch.call(abi.encodeCall(world.giveRoadShoveledReward, (player)));
  }

  function spawnRoadFromPush(bytes32 causedBy, bytes32 pushed, bytes32 road, PositionData memory pos) public {

    bool pushedNPC = NPC.get(pushed) > 0;
    RoadState state = pushedNPC ? RoadState.Bones : RoadState.Paved;
    spawnFinishedRoad(causedBy, pos.x, pos.y, state);

  }

  function spawnFinishedRoad(bytes32 causedBy, int32 x, int32 y, RoadState state) public {
    IWorld world = IWorld(_world());
    require(Rules.onRoad(x, y), "off road");

    bytes32 road = Actions.getRoadEntity(x,y);
    Position.set(road, x, y, -1);
    //TODO setfilled to save gas
    // Road.setFilled(road, player);
    Road.set(road, uint32(state), causedBy, false);
    Move.set(road, uint32(MoveType.None));

    //reward the player
    SystemSwitch.call(abi.encodeCall(IWorld(_world()).giveRoadFilledReward, (causedBy)));

    updateChunk(causedBy);

  }

}
