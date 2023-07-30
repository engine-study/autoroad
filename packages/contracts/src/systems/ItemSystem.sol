// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Damage, Position, Player, Health, GameState, Bounds } from "../codegen/Tables.sol";
import { GameEvent } from "../codegen/Tables.sol";
// import { Item } from "../codegen/Tables.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";


contract ItemSystem is System {

  function buy(uint32 item) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));

    GameEvent.emitEphemeral(player, "buy");
  }

  function manifest(uint32 item) public returns(bool) {

    if(item == 0) {
        
    }

    return false;
  }

}