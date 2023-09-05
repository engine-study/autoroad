// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Action } from "../codegen/Tables.sol";
import { ActionType } from "../codegen/Types.sol";
import { Position, PositionTableId, PositionData } from "../codegen/Tables.sol";
import { MoveSubsystem } from "../systems/MoveSubsystem.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";

contract MilitiaSubsystem is System {

  function aggro(bytes32 entity) public {

  }
  
}