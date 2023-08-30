// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { State } from "../codegen/Tables.sol";
import { StateType } from "../codegen/Types.sol";
import { Position, PositionTableId, PositionData } from "../codegen/Tables.sol";
import { MoveSubsystem } from "../systems/MoveSubsystem.sol";
import { FloraSubsystem } from "../systems/FloraSubsystem.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";

contract ActionSystem is System {

  function action(StateType newState, int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());

    if(newState == StateType.Idle) {
        
    } else if(newState == StateType.Mining) {
        world.mine(player, x, y);
    } else if(newState == StateType.Shoveling) {
        world.shovel(player, x, y);
    } else if(newState == StateType.Stick) {
        world.stick(player, x, y);
    } else if(newState == StateType.Fishing) {
        world.fish(player, x, y);
    } else if(newState == StateType.Walking) {
        world.moveSimple(player, x, y);
    } else if(newState == StateType.Buy) {

    } else if(newState == StateType.Plant) {
        world.plant(player, x, y);
    } else if(newState == StateType.Push) {
        world.push(player, x, y);
    } else if(newState == StateType.Chop) {
        world.chop(player, x, y);
    } else if(newState == StateType.Teleport) {
        world.teleportScroll(player, x, y);
    } else if(newState == StateType.Melee) {
        world.melee(player, x, y);
    } 

    enterState(player, newState, x, y);

  }

  function enterState(bytes32 player, StateType newState, int32 x, int32 y) private {
    State.emitEphemeral(player, uint32(newState),x,y);
  }
}