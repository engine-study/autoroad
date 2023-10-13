// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Action, Name, Player, Health} from "../codegen/Tables.sol";
import { ActionType, PaymentType } from "../codegen/Types.sol";
import { Position, PositionTableId, PositionData } from "../codegen/Tables.sol";

import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";

import { MoveSubsystem } from "../systems/MoveSubsystem.sol";
import { ToolSubsystem } from "../systems/ToolSubsystem.sol";
import { MapSubsystem } from "../systems/MapSubsystem.sol";
import { SpawnSubsystem } from "../systems/SpawnSubsystem.sol";
import { FloraSubsystem } from "../systems/FloraSubsystem.sol";
import { TerrainSubsystem } from "../systems/TerrainSubsystem.sol";

contract ActionSystem is System {

  function name(uint32 firstName, uint32 middleName, uint32 lastName) public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));
    bool hasName = Name.getNamed(entity);
    
    require(!hasName, "already has name");
    require(firstName < 36, "first name");
    require(middleName < 1025, "middle name");
    require(lastName < 1734, "last name");
    
    Name.set(entity, true, firstName, middleName, lastName);

  }

  function resetPlayer() public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());
    PositionData memory pos = Position.get(entity);
    world.kill(entity, entity, entity, pos);

  } 

  function spawn(int32 x, int32 y) public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());
    bool playerExists = Player.get(entity);

    if (playerExists) { require(Health.get(entity) == -1, "not dead, can't respawn");}
    require(world.onSpawn(x,y), "out of spawn");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y, 0));
    require(atPosition.length < 1, "occupied");

    world.spawnPlayer(entity, x, y, false);
  }

  function megaSummon() public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());
    world.summonMile(player, true);
  }

  function helpSummon() public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());
    world.summonMile(player, false);
  }

  function buy(uint32 id, PaymentType payment) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());
    world.buyItem(player, player, id, payment);
    PositionData memory pos = Position.get(player);
    setAction(player, ActionType.Buy, pos.x, pos.y);
  }

  function action(ActionType newAction, int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());

    if (newAction == ActionType.Idle) {} 
    else if (newAction == ActionType.Mining) {
      world.mine(player, x, y);
      return;
    } else if (newAction == ActionType.Shoveling) {
      world.shovel(player, x, y);
      return;
    } else if (newAction == ActionType.Stick) {
      world.stick(player, x, y);
      return;
    } else if (newAction == ActionType.Fishing) {
      world.fish(player, x, y);
      return;
    } else if (newAction == ActionType.Walking) {
      world.moveSimple(player, x, y);
      return;
    } else if (newAction == ActionType.Buy) {
      require(false, "Not setup.");
    } else if (newAction == ActionType.Plant) {
      world.plant(player, x, y);
      return;
    } else if (newAction == ActionType.Push) {
      world.push(player, x, y);
      return;
    } else if (newAction == ActionType.Chop) {
      world.chop(player, x, y);
      return;
    } else if (newAction == ActionType.Teleport) {
      world.teleportScroll(player, x, y);
      return;
    } else if (newAction == ActionType.Melee) {
      world.melee(player, x, y);
      return;
    }

    require(false, "No action found.");

  }

  function setActionTargeted(bytes32 player, ActionType newAction, int32 x, int32 y, bytes32 target) public {
    Action.emitEphemeral(player, uint32(newAction), x, y, target);
  }

  function setAction(bytes32 player, ActionType newAction, int32 x, int32 y) public {
    Action.emitEphemeral(player, uint32(newAction), x, y, bytes32(0));
  }
}
