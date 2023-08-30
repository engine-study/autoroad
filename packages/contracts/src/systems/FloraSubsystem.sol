// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Position, PositionTableId, GameEvent } from "../codegen/Tables.sol";
import { Player, Health, Tree, Seeds } from "../codegen/Tables.sol";
import { TerrainType } from "../codegen/Types.sol";

import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { MoveSubsystem } from "./MoveSubsystem.sol";

contract FloraSubsystem is System {

 
  function water(int32 x, int32 y) public {

  }

  function chop(bytes32 player, int32 x, int32 y) public {
    IWorld world = IWorld(_world());
    require(world.canDoStuff(player), "hmm");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));

    require(world.canInteract(player, x, y, atPosition, 1), "bad interact");
    require(Tree.get(atPosition[0]), "no tree");

    int32 health = Health.get(atPosition[0]);
    health--;

    if (health <= 0) {
      
      Health.set(atPosition[0], health);
      Position.deleteRecord(atPosition[0]);

      uint32 seedCount = Seeds.get(player);
      Seeds.set(player, seedCount + 2);
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


    Seeds.set(player, seeds-1);
    world.spawnTerrain(x,y, TerrainType.Tree);

  }
}
