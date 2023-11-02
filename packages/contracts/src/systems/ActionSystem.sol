// SPDX-License-Identifier: MIT
pragma solidity >=0.8;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { IBaseWorld } from "@latticexyz/world/src/codegen/interfaces/IBaseWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Action, Name, Player, Health} from "../codegen/index.sol";
import { ActionType, PaymentType } from "../codegen/common.sol";
import { Position, PositionTableId, PositionData } from "../codegen/index.sol";

import { Actions } from "../utility/actions.sol";
import { Rules } from "../utility/rules.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";

contract ActionSystem is System {

  function sup() public {
    console.log("sup");
  }

  function name(uint32 firstName, uint32 middleName, uint32 lastName) public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));

    bool hasName = Name.getNamed(entity);
    require(!hasName, "already has name");
    
    require(firstName < 143, "first name");
    require(middleName < 36, "middle name");
    require(lastName < 1025, "last name");
    
    Name.set(entity, true, firstName, middleName, lastName);

  }

  function resetPlayer() public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());
    PositionData memory pos = Position.get(entity);
    SystemSwitch.call(abi.encodeCall(world.kill, (entity, entity, entity, pos)));

  } 

  function spawn(int32 x, int32 y) public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());
    bool playerExists = Player.get(entity);

    if (playerExists) { require(Health.get(entity) == -1, "not dead, can't respawn");}
    require(Rules.onSpawn(x,y), "out of spawn");

    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,x, y, 0);
    require(atPosition.length < 1, "occupied");

    SystemSwitch.call(abi.encodeCall(world.spawnPlayer, (entity, x, y, false)));
  }

  function megaSummon() public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());
    SystemSwitch.call(abi.encodeCall(world.summonMile, (player, true)));
  }

  function helpSummon() public {
    bytes32 player = addressToEntityKey(address(_msgSender()));

    console.log("callSummonMile");
    IWorld world = IWorld(_world());
    // SystemSwitch.call("terrain", abi.encodeCall(MoveSubsystem.summonMile, (player, false)));
    //return abi.decode(SystemSwitch.call(SYSTEM_ID, abi.encodeCall(UniqueEntitySystem.getUniqueEntity, ())), (bytes32));
    SystemSwitch.call(abi.encodeCall(world.summonMile, (player, false)));
  }

  function buy(uint32 id, PaymentType payment) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());
    SystemSwitch.call(abi.encodeCall(world.buyItem, (player, player, id, payment)));
    PositionData memory pos = Position.get(player);
    Actions.setAction(player, ActionType.Buy, pos.x, pos.y);
  }

  function action(ActionType newAction, int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());
    
    require(Rules.canDoStuff(player), "hmm");

    if (newAction == ActionType.Idle) {} 
    else if (newAction == ActionType.Mining) {
      SystemSwitch.call(abi.encodeCall(world.mine, (player, x, y)));
      return;
    } else if (newAction == ActionType.Shoveling) {
      SystemSwitch.call(abi.encodeCall(world.shovel, (player, x, y)));
      return;
    } else if (newAction == ActionType.Stick) {
      SystemSwitch.call(abi.encodeCall(world.stick, (player, x, y)));
      return;
    } else if (newAction == ActionType.Fishing) {
      SystemSwitch.call(abi.encodeCall(world.fish, (player, x, y)));
      return;
    } else if (newAction == ActionType.Walking) {
      SystemSwitch.call(abi.encodeCall(world.moveSimple, (player, x, y)));
      return;
    } else if (newAction == ActionType.Buy) {
      require(false, "Not setup.");
    } else if (newAction == ActionType.Plant) {
      SystemSwitch.call(abi.encodeCall(world.plant, (player, x, y)));
      return;
    } else if (newAction == ActionType.Push) {
      SystemSwitch.call(abi.encodeCall(world.push, (player, x, y)));
      return;
    } else if (newAction == ActionType.Chop) {
      SystemSwitch.call(abi.encodeCall(world.chop, (player, x, y)));
      return;
    } else if (newAction == ActionType.Teleport) {
      SystemSwitch.call(abi.encodeCall(world.teleportScroll, (player, x, y)));
      return;
    } else if (newAction == ActionType.Melee) {
      SystemSwitch.call(abi.encodeCall(world.melee, (player, x, y)));
      return;
    }

    require(false, "No action found.");

  }
}
