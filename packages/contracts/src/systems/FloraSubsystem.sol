// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Position, PositionTableId } from "../codegen/Tables.sol";
import { Player, Health, Tree, Seeds, Move } from "../codegen/Tables.sol";
import { ActionType, TerrainType, FloraType, MoveType } from "../codegen/Types.sol";
import { MoveSubsystem } from "./MoveSubsystem.sol";

import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { randomSeed, randomCoord} from "../utility/random.sol";
import { getUniqueEntity } from "@latticexyz/world/src/modules/uniqueentity/getUniqueEntity.sol";

contract FloraSubsystem is System {

  function spawnFlora(bytes32 player, bytes32 entity, int32 x, int32 y) public {

    uint noiseCoord = randomCoord(0, 100, x, y);
    // console.log("noise ", noiseCoord);

    FloraType floraType = FloraType.None;

    if (noiseCoord < 10) {
      floraType = FloraType.Bramble;
      Health.set(entity, 1);
      Move.set(entity, uint32(MoveType.Trap));
    } else if (noiseCoord >= 10 && noiseCoord < 20) {
      floraType = FloraType.Oak;
      Health.set(entity, 3);
      Move.set(entity, uint32(MoveType.Obstruction));
    } else {
      floraType = FloraType.Tree;
      Health.set(entity, 1);
      Move.set(entity, uint32(MoveType.Obstruction));
    } 

    Tree.set(entity, uint32(floraType));

  }

  function water(int32 x, int32 y) public {

  }

  function chop(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.canDoStuff(player), "hmm");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));

    require(world.canInteract(player, x, y, atPosition, 1), "bad interact");
    require(Tree.get(atPosition[0]) != uint32(FloraType.None), "no tree");

    world.setAction(player, ActionType.Chop, x, y);

    int32 health = Health.get(atPosition[0]);
    health--;

    if (health <= 0) {
      
      Health.set(atPosition[0], health);
      Position.deleteRecord(atPosition[0]);

      uint32 seedCount = Seeds.get(player);
      Seeds.set(player, seedCount + 1);
      // uint32 newSeeds = uint32(randomSeed(0,2,uint(seedCount)));
      // if(newSeeds > 0) {Seeds.set(player, seedCount + newSeeds);}

      //randomly spawn a log
      //kill a player if it falls on them


    } else {
      Health.set(atPosition[0], health);
    }

    
  }

  
  function plant(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.canDoStuff(player), "hmm");
    require(!world.onRoad(x, y), "on road");
    uint32 seeds = Seeds.get(player);
    require(seeds > 0, "no seeds");

    bytes32[] memory atRoad = getKeysWithValue(PositionTableId, Position.encode(x, y, -1));
    require(atRoad.length == 0, "road here");
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));
    require(world.canInteractEmpty(player, x, y, atPosition, 1), "bad interact");

    world.setAction(player, ActionType.Plant, x, y);

    Seeds.set(player, seeds-1);
    world.spawnTerrain(player, x, y, TerrainType.Tree);

  }
}
