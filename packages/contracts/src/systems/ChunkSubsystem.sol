// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";

import { GameState, GameConfig, GameConfigData, MapConfig, RoadConfig, Chunk, Bounds, Entities } from "../codegen/index.sol";
import { Road, Move, Player, Rock, Health, Carriage, Coinage, Weight, Stats, NPC, Linker, WorldColumn } from "../codegen/index.sol";
import { Position, PositionData, PositionTableId, Tree, Seeds, Row, Trigger } from "../codegen/index.sol";
import { TerrainType, RockType, RoadState, MoveType, NPCType, FloraType, ActionName } from "../codegen/common.sol";

import { Actions } from "../utility/actions.sol";
import { Rules } from "../utility/rules.sol";
import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { getUniqueEntity } from "@latticexyz/world-modules/src/modules/uniqueentity/getUniqueEntity.sol";
import { randomCoord, randomFromEntity, randomFromEntitySeed } from "../utility/random.sol";

contract ChunkSubsystem is System {
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
    Position.set(worldColumn, 0, -10, 0);

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

    require(mile == -1 || Chunk.getCompleted(oldChunk) == true, "fatal, mile not complete");

    mile++;

    console.log("new mile");
    console.logInt(mile);

    //TODO simple setter
    GameState.set(mile, 0);
    // GameState.setMiles(mile);

    //move carriage to top of mile
    int32 height = MapConfig.getPlayHeight();
    Position.set(Actions.getCarriageEntity(), 0, ((mile + 1) * height) + 1, 0);

    //create the chunk
    bytes32 newChunk = Actions.getChunkEntity(mile);
    Chunk.set(newChunk, mile, false, false, 0, 0);
  }

  function createProctor(bytes32 causedBy, bool mileLink) public {
    IWorld world = IWorld(_world());
    int32 mile = GameState.getMiles();
    (, , int32 up, int32 down) = Rules.getMileBounds(mile);

    bytes32 proctor = Actions.getProctorEntity();

    //spawn if it doesnt exist yet
    PositionData memory spawnPos = PositionData(0, down, 0);
    Actions.deleteAt(world, spawnPos);

    if (NPC.get(proctor) == 0) {
      SystemSwitch.call(
        abi.encodeCall(world.spawnNPCWithEntity, (causedBy, proctor, spawnPos.x, spawnPos.y, NPCType.Proctor))
      );
    } else {
      Health.set(proctor, 1);
      Position.set(proctor, spawnPos);
      Actions.setAction(proctor, ActionName.Spawn, spawnPos.x, spawnPos.y);
    }

    if (mileLink) {
      bytes32 proctorTrigger = Actions.getRoadEntity(0, up);
      //link proctor to end of road
      Linker.set(proctor, proctorTrigger);
      Trigger.set(proctorTrigger, true);
    }
  }

  function debugProctor(bytes32 causedBy) public {
    IWorld world = IWorld(_world());

    int32 mile = GameState.getMiles();
    (int32 left, int32 right, int32 up, int32 down) = Rules.getMileBounds(mile);

    bytes32 proctor = Actions.getProctorEntity();
    PositionData memory procPos = Position.get(proctor);
    PositionData memory endPos = PositionData(0, up, 0);

    Actions.deleteAt(world, endPos);
    bytes32[] memory entities = new bytes32[](0);
    SystemSwitch.call(
      abi.encodeCall(world.moveTo, (causedBy, proctor, procPos, endPos, entities, ActionName.Teleport))
    );
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
        bytes32 road = Actions.getRoadEntity(x, y);
        uint32 roadState = Road.getState(road);
        if (roadState >= uint32(RoadState.Paved)) continue;
        SystemSwitch.call(abi.encodeCall(world.spawnFinishedRoad, (credit, x, y, RoadState.Paved)));
      }
    }
  }
}
