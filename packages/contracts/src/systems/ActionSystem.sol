// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { State } from "../codegen/Tables.sol";
import { StateType } from "../codegen/Types.sol";
import { Position, PositionTableId, PositionData } from "../codegen/Tables.sol";
import { MoveSystem } from "../systems/MoveSystem.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";

contract ActionSystem is System {

  function action(StateType newState, int32 x, int32 y) public {
    IWorld world = IWorld(_world());

    if(newState == StateType.Idle) {
        
    } else if(newState == StateType.Mining) {
        world.mine(x,y);
    } else if(newState == StateType.Shoveling) {
        world.shovel(x,y);
    } else if(newState == StateType.Stick) {
        world.stick(x,y);
    } else if(newState == StateType.Fishing) {
        world.fish(x,y);
    } else if(newState == StateType.Walking) {
        world.moveSimple(x,y);
    } else if(newState == StateType.Buy) {

    } else if(newState == StateType.Plant) {
        world.plant(x,y);
    }

    enterState(newState);

  }

  function enterState(StateType newState) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    State.emitEphemeral(player, uint32(newState));
  }
}