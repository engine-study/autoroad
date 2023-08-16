// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Position, Player, Health, GameState, Bounds } from "../codegen/Tables.sol";
import { Move, State, Carrying, Bones, Name, Stats, GameEvent, Coinage } from "../codegen/Tables.sol";
import { PositionTableId } from "../codegen/Tables.sol";
import { MoveType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { MapSubSystem } from "../systems/MapSubSystem.sol";
import { randomCoord } from "../utility/random.sol";

contract SpawnSystem is System {
    
  function name(uint32 firstName, uint32 middleName, uint32 lastName) public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));
    (bool hasName,,,) = Name.get(entity);
    
    require(!hasName, "already has name");
    require(firstName < 36, "first name");
    require(middleName < 1025, "middle name");
    require(lastName < 1734, "last name");
    
    Name.set(entity, true, firstName, middleName, lastName);

  }

  function spawn(int32 x, int32 y) public {

    bytes32 entity = addressToEntityKey(address(_msgSender()));
    bool playerExists = Player.get(entity);

    if(playerExists) {
      require(Health.get(entity) == -1, "not dead, can't respawn");
    }

    (,,int32 up, int32 down ) = Bounds.get();
    (int32 playWidth, int32 spawnWidth) = MapConfig.get();

    require(x > playWidth && x <= spawnWidth, "x outside spawn");
    require(y <= up && y >= down, "y outside of spawn");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));
    require(atPosition.length < 1, "occupied");

    spawnPlayer(x, y, entity, false);
  }

  function spawnBotAdmin(int32 x, int32 y, bytes32 entity) public {
    Name.set(entity, true, uint32(randomCoord(0, 35, x,y)), uint32(randomCoord(0, 1024, x,y+1)), uint32(randomCoord(0, 1733, x,y+2) ));  
    spawnPlayer(x,y,entity, true);
  }

  function spawnPlayer(int32 x, int32 y, bytes32 entity, bool isBot) private {

    bool playerExists = Player.get(entity);

    if(!playerExists) {
      Player.set(entity, true);
      Coinage.set(entity, 0);

      (int32 miles, int32 players) = GameState.get();
      Stats.set(entity, miles, 0, 0, 0, 0, 0, 0, 0);

      if(!isBot) {
        GameState.set(miles, players+1);
        // GameState.setPlayerCount(playerCount + 1);
      }

    }

    Health.set(entity, 3);
    Move.set(entity, uint32(MoveType.Push));
    Position.set(entity, x, y, 0);
  }

  function destroyPlayerAdmin() public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));

    require(Player.get(entity), "already destroyed");

    Player.deleteRecord(entity);
    Position.deleteRecord(entity);
    Health.set(entity, -1);
    Move.deleteRecord(entity);
  }

}