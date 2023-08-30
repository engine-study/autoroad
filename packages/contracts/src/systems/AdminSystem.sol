// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { GameState, Coinage } from "../codegen/Tables.sol";
import { TerrainType } from "../codegen/Types.sol";
import { RoadSubsystem } from "./RoadSubsystem.sol";
import { RewardSubsystem } from "./RewardSubsystem.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";

contract AdminSystem is System {
  //TODO, SET ADMIN
  function isAdmin(bytes32 player) public pure returns (bool) {
    return true;
  }

  function spawnTerrainAdmin(int32 x, int32 y, TerrainType tType) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).spawnTerrain(player, x, y, tType);
  }

  function deleteAdmin(int32 x, int32 y, int32 layer) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).deleteAt(x,y, layer);
  }

  function spawnMileAdmin() public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    int32 nextMile = GameState.getMiles() + 1;
    IWorld(_world()).createMile(nextMile);
  }

  function finishMileAdmin() public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).debugMile(player);
  }

  function spawnFinishedRoadAdmin(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).spawnDebugRoad(player, x, y);
  }

  function spawnShoveledRoadAdmin(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).spawnShoveledRoad(player, x, y);
  }

  function addCoinsAdmin(int32 amount) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).giveCoins(player, amount);
  }
  
  function addXPAdmin(uint256 amount) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).giveXP(player, amount);
  }

  function addGemXP(uint32 amount) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).giveGem(player, amount);
  }

  function teleportAdmin(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).teleport(player, x, y);
  }
}
