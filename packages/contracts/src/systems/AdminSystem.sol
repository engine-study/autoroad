// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Player, Position, Health, Move, GameState, Coinage, PositionData, PositionTableId } from "../codegen/Tables.sol";
import { TerrainType, NPCType } from "../codegen/Types.sol";
import { TerrainSubsystem } from "./TerrainSubsystem.sol";
import { NPCSubsystem } from "./NPCSubsystem.sol";
import { PuzzleSubsystem } from "./PuzzleSubsystem.sol";
import { RewardSubsystem } from "./RewardSubsystem.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";

contract AdminSystem is System {

  //TODO, SET ADMIN
  function isAdmin(bytes32 player) public pure returns (bool) {
    return true;
  }

  function spawnTerrainAdmin(int32 x, int32 y, TerrainType terrainType) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).spawnTerrain(player, x, y, terrainType);
  }

  function spawnNPCAdmin(int32 x, int32 y, NPCType npcType) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).spawnNPC(player, x, y, npcType);
  }

  function killPlayerAdmin(int32 x, int32 y) public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));
    require(isAdmin(entity), "not admin");    
    bytes32[] memory atPosition = IWorld(_world()).getKeysAtPosition ( x, y, 0 );

    if(atPosition.length == 0) return;
    IWorld(_world()).kill(entity, atPosition[0], entity, PositionData(x,y,0));
  }

  function destroyPlayerAdmin() public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));
    require(isAdmin(entity), "not admin");
    require(Player.get(entity), "already destroyed");

    Player.deleteRecord(entity);
    Position.deleteRecord(entity);
    Health.set(entity, -1);
    Move.deleteRecord(entity);
  }

  function deleteAdmin(int32 x, int32 y, int32 layer) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).deleteAt(x,y, layer);
  }

  function spawnPuzzleAdmin() public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).createPuzzleOnMile(player);
  }

  function spawnMileAdmin() public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld(_world()).createMile();
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

  function addGemXP(int32 amount) public {
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
