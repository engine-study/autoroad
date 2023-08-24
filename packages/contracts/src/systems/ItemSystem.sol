// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Damage, Position, Player, Health, GameState, Bounds } from "../codegen/Tables.sol";
import { Coinage, Scroll, Stick, Robe, Head, Boots, FishingRod } from "../codegen/Tables.sol";
import { GameEvent } from "../codegen/Tables.sol";
// import { Item } from "../codegen/Tables.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";

contract ItemSystem is System {
  function buy(uint32 item) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));

    GameEvent.emitEphemeral(player, "buy");
  }
  
  function sendCoins(int32 amount) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
  }

  function buyScroll() public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    int32 coins = Coinage.get(player);

    require(coins >= 5, "not enough coins");

    withdraw(player, coins, 5);

    uint32 scrolls = Scroll.get(player);
    Scroll.set(player, scrolls + 1);
  }

  function buyCosmetic(uint32 id) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    int32 coins = Coinage.get(player);

    require(coins >= 5, "probably not enough coins");

    int32 price = 0;

    //TODO instead have this read from some preset array
    
    //stick
    if (id == 0) {
      price = 10;
    } 
    //robe
    if (id == 1) {
      price = 250;
    } 
    //pickelhaube
    if (id == 2) {
      price = 99;
    } 
    //fishing rod
    if (id == 3) {
      price = 25;
    } 
    //boots
    if (id == 4) {
      price = 50;
    } 
    

    require(price > 0, "no item found");
    require(coins >= price, "need more coins");

    withdraw(player, coins, price);
    addToInventory(player, id);
  }

  function withdraw(bytes32 player, int32 coins, int32 amount) private {
    require(amount > 0, "amount is zero or negative");
    Coinage.set(player, coins - amount);
  }

  function addToInventory(bytes32 player, uint32 id) private {
    if (id == 0) {
      Stick.set(player, true);
    }
    if (id == 1) {
      Robe.set(player, 0);
    }
    if (id == 2) {
      Head.set(player, 0);
    }
    if (id == 3) {
      FishingRod.set(player, true);
    }
    if (id == 4) {
      Boots.set(player, 1, 3);
    }
  }

  function manifest(uint32 item) public returns (bool) {
    if (item == 0) {}

    return false;
  }
}
