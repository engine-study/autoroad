// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Position, PositionTableId, PositionData } from "../codegen/index.sol";
import { Player, Health, Tree, Seeds, Move, Axe, Road } from "../codegen/index.sol";
import { ActionType, TerrainType, FloraType, MoveType } from "../codegen/common.sol";

import { Rules } from "../utility/rules.sol";
import { Actions } from "../utility/actions.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { randomSeed, randomCoord} from "../utility/random.sol";
import { getUniqueEntity } from "@latticexyz/world-modules/src/modules/uniqueentity/getUniqueEntity.sol";
import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";

import { MoveSubsystem } from "./MoveSubsystem.sol";

contract FloraSubsystem is System {


  function spawnFloraRandom(bytes32 player, bytes32 entity, int32 x, int32 y) public {
    
    uint noiseCoord = randomCoord(0, 100, x, y);
    // console.log("noise ", noiseCoord);

    FloraType floraType = FloraType.None;

    if (noiseCoord < 10) {
      floraType = FloraType.Bramble;
    } else if (noiseCoord >= 10 && noiseCoord < 20) {
      floraType = FloraType.Oak;
    } else {
      floraType = FloraType.Tree;
    } 

    spawnFlora(player, entity, x, y, floraType);
  }

  function spawnFlora(bytes32 player, bytes32 entity, int32 x, int32 y, FloraType floraType ) public {

    if (floraType == FloraType.Bramble) {
      Health.set(entity, 1);
      Move.set(entity, uint32(MoveType.Trap));
    } else if (floraType == FloraType.Oak) {
      Health.set(entity, 3);
      Move.set(entity, uint32(MoveType.Obstruction));
    } else {
      Health.set(entity, 1);
      Move.set(entity, uint32(MoveType.Obstruction));
    } 

    Tree.set(entity, uint32(floraType));

  }


  function water(int32 x, int32 y) public {

  }

  function chop(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(Axe.get(player), "no Axe");
    require(Rules.canDoStuff(player), "hmm");

    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,x, y, 0);

    require(Rules.canInteract(player, Position.get(player), atPosition, 1), "bad interact");
    require(Tree.get(atPosition[0]) != uint32(FloraType.None), "no tree");

    Actions.setActionTargeted(player, ActionType.Chop, x, y, atPosition[0]);

    int32 health = Health.get(atPosition[0]);
    health--;

    if (health <= 0) {
      
      Health.set(atPosition[0], -1);
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
    require(Rules.canDoStuff(player), "hmm");
    uint32 seeds = Seeds.get(player);
    require(seeds > 0, "no seeds");

    require(Rules.onMap(x, y), "off map");
    // require(!Rules.onRoad(x, y), "on road");

    bytes32 roadEntity = Actions.getRoadEntity(x, y);
    require(Road.getState(roadEntity) == 0, "road here");

    PositionData memory pos = PositionData(x,y,0);
    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,x, y, 0);
    require(Rules.canInteractEmpty(player, Position.get(player), pos, atPosition, 1), "bad interact");

    Actions.setAction(player, ActionType.Plant, x, y);

    Seeds.set(player, seeds-1);
    SystemSwitch.call(abi.encodeCall(world.spawnTerrain, (player, x, y, TerrainType.Tree)));

  }
}
