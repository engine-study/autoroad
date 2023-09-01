// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Action } from "../codegen/Tables.sol";
import { ActionType } from "../codegen/Types.sol";
import { Position, PositionTableId, PositionData } from "../codegen/Tables.sol";
import { MoveSubsystem } from "../systems/MoveSubsystem.sol";
import { FloraSubsystem } from "../systems/FloraSubsystem.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";

contract ActionSystem is System {

  function action(ActionType newAction, int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    IWorld world = IWorld(_world());

    if(newAction == ActionType.Idle) {
        
    } else if(newAction == ActionType.Mining) {
        world.mine(player, x, y);
    } else if(newAction == ActionType.Shoveling) {
        world.shovel(player, x, y);
    } else if(newAction == ActionType.Stick) {
        world.stick(player, x, y);
    } else if(newAction == ActionType.Fishing) {
        world.fish(player, x, y);
    } else if(newAction == ActionType.Walking) {
        world.moveSimple(player, x, y);
    } else if(newAction == ActionType.Buy) {

    } else if(newAction == ActionType.Plant) {
        world.plant(player, x, y);
    } else if(newAction == ActionType.Push) {
        world.push(player, x, y);
    } else if(newAction == ActionType.Chop) {
        world.chop(player, x, y);
    } else if(newAction == ActionType.Teleport) {
        world.teleportScroll(player, x, y);
    } else if(newAction == ActionType.Melee) {
        world.melee(player, x, y);
    } 

    setAction(player, newAction, x, y);

  }

  function setAction(bytes32 player, ActionType newAction, int32 x, int32 y) public {
    Action.emitEphemeral(player, uint32(newAction),x,y);
  }
  
}