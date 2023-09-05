// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Action } from "../codegen/Tables.sol";
import { ActionType } from "../codegen/Types.sol";
import { Position, PositionTableId, PositionData, Health, Action, Militia } from "../codegen/Tables.sol";
import { MoveSubsystem } from "./MoveSubsystem.sol";
import { ActionSystem } from "./ActionSystem.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";

contract MilitiaSubsystem is System {

  function aggro(bytes32 player, bytes32 entity, PositionData memory playerPos, PositionData memory entityPos) public {

    console.log("aggro");
    if(Militia.get(entity) == false) return;

    IWorld world = IWorld(_world());
    int32 health = Health.get(player);
    if(health == 0) return;

    world.setAction(entity, ActionType.Melee, playerPos.x, playerPos.y);
    world.kill(entity, player, playerPos);
  }
  
}