// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";

import { GameState, GameConfig, GameConfigData, MapConfig, RoadConfig, Chunk, Bounds, Entities } from "../codegen/index.sol";
import { Road, Move, Player, Rock, Health, Carriage, Coinage, Weight, Stats, NPC, Linker, WorldColumn } from "../codegen/index.sol";
import { Position, PositionData, PositionTableId, Tree, Seeds, Row, Trigger } from "../codegen/index.sol";
import { TerrainType, RockType, RoadState, MoveType, NPCType, FloraType, ActionName} from "../codegen/common.sol";

import { Actions } from "../utility/actions.sol";
import { Rules } from "../utility/rules.sol";
import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { getUniqueEntity } from "@latticexyz/world-modules/src/modules/uniqueentity/getUniqueEntity.sol";
import { randomCoord, randomFromEntity, randomFromEntitySeed } from "../utility/random.sol";

contract TerrainSubsystem is System {
  

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
    uint difficulty = uint(uint32(mile % 5));

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

    // reset ticking entities
    Entities.set(new bytes32[](0));
    IWorld world = IWorld(_world());


    console.log("create puzzle");
    SystemSwitch.call(abi.encodeCall(world.createMiliarium, (causedBy, right, up, down)));
    SystemSwitch.call(abi.encodeCall(world.createStatuePuzzle, (causedBy, right, up, down)));
    SystemSwitch.call(abi.encodeCall(world.createTickers, (causedBy, right, up, down, mile)));
    SystemSwitch.call(abi.encodeCall(world.createProctor, (causedBy, true)));

    //set bounds 
    Bounds.set(left, right, up, down);

    console.log("set chunk");
    bytes32 chunkEntity = Actions.getChunkEntity(mile);
    Chunk.set(chunkEntity, mile, true, false, 0, 0);
  }


  function summonRow(bytes32 causedBy, int32 left, int32 right, uint difficulty) public returns(int32 row) {

    console.log("summon row");

    row = Row.get();
    row++;

    spawnRow(causedBy, right, row, difficulty);
    spawnProcRoad(0,row);

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

        if (noiseCoord <= 100) {
          terrainType = TerrainType.Tree;
        } else if (noiseCoord > 200 && noiseCoord <= 300 + difficulty * 20) {
          terrainType = TerrainType.Rock;
        } else if (noiseCoord > 500 && noiseCoord <= 600 + difficulty * 10) {
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

        if (noiseCoord > 1000 && noiseCoord <= 1050 - difficulty * 10) {
          npcType = NPCType.Ox;
        } else if (noiseCoord > 1100 && noiseCoord <= 1200 - difficulty * 10) {
          npcType = NPCType.Soldier;
        } else if(difficulty >= 0 && noiseCoord > 1500 && noiseCoord < 1550 + difficulty * 20) {
          npcType = NPCType.Barbarian;
        } else if (difficulty > 1 && noiseCoord > 1700 && noiseCoord <= 1750 + difficulty * 10) {
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

  function contemplateMile(bytes32 causedBy, int32 mileNumber) public {
    
    (int32 left, int32 right, int32 up, int32 down) = Rules.getMileBounds(mileNumber);
    SystemSwitch.call(abi.encodeCall(IWorld(_world()).giveProctorLottery, (causedBy)));

    //old system of awarding road reward to random road paving
    // uint noiseCoord = randomFromEntity(uint(uint32(down)), uint(uint32(up)), causedBy);
    // int32 noiseTile = int32(uint32(noiseCoord));
    // require(noiseTile >= down && noiseTile <= up, "out of range");
    // bytes32 road = Actions.getRoadEntity(0, noiseTile);
    // require(Road.getState(road) == uint32(RoadState.Paved), "not paved");
    // SystemSwitch.call(abi.encodeCall(IWorld(_world()).giveRoadLottery, (road)));

    //OTHER system of givnig random rewards for each road
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
      Move.set(entity, uint32(MoveType.Push));
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
    return;
    //NOT BEING USED ATM, FIX THIS LATER
    int32 currentMile = GameState.getMiles();
    bytes32 chunk = keccak256(abi.encode("Chunk", currentMile));

    uint32 pieces = Chunk.getRoads(chunk);
    int32 playHeight = MapConfig.getPlayHeight();
    uint32 roadWidth = RoadConfig.getWidth();

    pieces++;

    //road complete!
    if (false && pieces >= (roadWidth * uint32(playHeight))) {
      //NOT DOING THIS ANYMORE, THE PROCTOR FINISHES THE MILE
      finishMile(causedBy, chunk, currentMile, pieces);
    } else {
      Chunk.set(chunk, currentMile, true, false, pieces, 0);
    }
  }

  function callFinishMile(bytes32 causedBy) public {
    int32 currentMile = GameState.getMiles();
    bytes32 chunk = keccak256(abi.encode("Chunk", currentMile));
    uint32 pieces = Chunk.getRoads(chunk);

    finishMile(causedBy, chunk, currentMile, pieces);
  }

  function finishMile(bytes32 causedBy, bytes32 chunk, int32 currentMile, uint32 pieces) public {
    console.log("finish chunk");
    console.logInt(currentMile);

    Chunk.set(chunk, currentMile, true, true, pieces, block.number);
    contemplateMile(causedBy, currentMile);

    currentMile += 1;

    IWorld world = IWorld(_world());
    SystemSwitch.call(abi.encodeCall(world.createMile, ()));
    
  }

  function spawnProcRoad(int32 x, int32 y) public {
    bytes32 road = Actions.getRoadEntity(x,y);

    spawnEmptyRoad(road, x, y);
    
    // uint randomRoad = randomFromEntitySeed(0,100,road,uint256(uint32(y)));
    // if(randomRoad < 75) {
    //   spawnEmptyRoad(road, x, y);
    // } else if(randomRoad < 95){
    //   spawnShoveledRoad(road, x, y);
    // } else {
    //   spawnFinishedRoad(road, x, y, RoadState.Paved);
    // }
    
  }

  function spawnEmptyRoad(bytes32 causedBy, int32 x, int32 y) public {
    bytes32 road = Actions.getRoadEntity(x,y);
    Position.set(road, x, y, -1);
    Road.set(road, uint32(RoadState.None), 0, false);
  }

  function spawnShoveledRoad(bytes32 causedBy, int32 x, int32 y) public {
    IWorld world = IWorld(_world());

    bytes32 road = Actions.getRoadEntity(x,y);
    require(Road.getState(road) == uint32(RoadState.None), "road");

    //TODO setState
    Road.set(road, uint32(RoadState.Shoveled), 0, false);
    Move.set(road, uint32(MoveType.Hole));
    Position.set(road, x, y, 0);

    if(causedBy != road) {
      SystemSwitch.call(abi.encodeCall(world.giveRoadShoveledReward, (causedBy)));
    }

  }

  function spawnRoadFromPush(bytes32 causedBy, bytes32 pushed, bytes32 road, PositionData memory pos) public {

    bool pushedNPC = NPC.get(pushed) > 0;
    RoadState state = pushedNPC ? RoadState.Bones : RoadState.Paved;
    spawnFinishedRoad(causedBy, pos.x, pos.y, RoadState.Paved);

  }

  function spawnFinishedRoad(bytes32 causedBy, int32 x, int32 y, RoadState state) public {
    IWorld world = IWorld(_world());
    require(Rules.onRoad(x, y), "off road");
    require(state >= RoadState.Paved, "must be paved or bonementum");

    bytes32 road = Actions.getRoadEntity(x,y);
    Position.set(road, x, y, -1);
    //TODO setfilled to save gas
    // Road.setFilled(road, player);
    Road.set(road, uint32(state), causedBy, false);
    Move.set(road, uint32(MoveType.None));

    //reward the player
    if(causedBy != road) {
      SystemSwitch.call(abi.encodeCall(IWorld(_world()).giveRoadFilledReward, (causedBy)));
    }

    updateChunk(causedBy);

  }

}
