// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Damage, Position, Player, Health, GameState, Bounds } from "../codegen/Tables.sol";
import { Seeds } from "../codegen/Tables.sol";
import { GameEvent } from "../codegen/Tables.sol";
// import { Item } from "../codegen/Tables.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";

contract TreeSystem is System {
    function plant(int32 x, int32 y) public {

    }
}