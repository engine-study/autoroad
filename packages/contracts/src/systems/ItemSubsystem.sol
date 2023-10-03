// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Position, PositionData, Player, Health, GameState, Bounds, Action } from "../codegen/Tables.sol";
import { Coinage, Gem, Eth, XP, Scroll, Stick, Robe, Head, Boots, FishingRod } from "../codegen/Tables.sol";
import { ActionType, PaymentType } from "../codegen/Types.sol";

import { getKeysWithValue } from "@latticexyz/world-modules/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { GaulItems } from "../data/GaulItems.sol";

contract ItemSubsystem is System {
  
  function sendCoins(int32 amount) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
  }

  function createItemMapping() public {
    
  } 

  function buyItem(bytes32 sender, bytes32 seller, uint32 id, PaymentType payment) public {
    int32 coins = Coinage.get(sender);
    require(coins > 0, "probably not enough coins");
    addToInventory(sender, id, payment);
  }

  function addToInventory(bytes32 player, uint32 id, PaymentType payment) private {

    int32 price = 0;

    //TODO instead have this read from some preset array

    //ITEMS
    if(id < 100) {

      if (id == 0) { //stick
        pay(player, 100, 0, 0, payment, 0);
        Stick.set(player, true);
      } 
      else if (id == 1) { //robe
        pay(player, 100, 0, 0, payment, 0);
        Robe.set(player, 0);
      } 
      else if (id == 2) { //pickelhaube
        pay(player, 100, 0, 0, payment, 0);
        Head.set(player, 0);
      } 
      else if (id == 3) { //fishing rod
        pay(player, 50, 0, 0, payment, 0);
        FishingRod.set(player, true);
      } 
      else if (id == 4) { //boots
        pay(player, 50, 0, 0, payment, 0);
        Boots.set(player, 1, 3);
      } 
      else if (id == 5) { //scroll
        pay(player, 5, 0, 0, payment, 0);
        uint32 scrolls = Scroll.get(player);
        Scroll.set(player, scrolls + 1);
      } 
    
    } 
    
    //OUTFITS
    else if(id < 200) {
      if (id == 100) { //leather
        pay(player, 0, 1, 1, payment, 0);

      } 
      else if (id == 100) { //etc
        pay(player, 0, 1, 1, payment, 0);

      } 
    }

    //LIMITED TIME
    else if(id < 300) {

    }
  }

  function pay(bytes32 account, int32 coinPrice, int32 gemPrice, int32 ethPrice, PaymentType paymentType, int32 minLevel) private {
    require(paymentType != PaymentType.None, "no payment type set");

    int32 level = 1;
    require(level >= minLevel, "not leveled");

    if(paymentType == PaymentType.Coins) {
      require(coinPrice != 0, "cannot pay with coins");
      withdrawCoins(account, coinPrice);
    } else if(paymentType == PaymentType.Gems) {
      require(gemPrice != 0, "cannot pay with gems");
      withdrawGems(account, gemPrice);
    } else if(paymentType == PaymentType.Eth) {
      require(ethPrice != 0, "cannot pay with eth");
      withdrawEth(account, ethPrice);
    } else {
      require(false, "exception");
    }

  }

  function withdrawCoins(bytes32 player, int32 amount) private {
    require(amount > 0, "amount is zero or negative");
    int32 coins = Coinage.get(player);
    require(coins >= amount, "not enough coins");
    Coinage.set(player, coins - amount);
  }

  function withdrawGems(bytes32 player, int32 amount) private {
    require(amount > 0, "amount is zero or negative");
    int32 gems = Gem.get(player);
    require(gems >= amount, "not enough gems");
    Gem.set(player, gems - amount);
  }

  function withdrawEth(bytes32 player, int32 amount) private {
    require(amount > 0, "amount is zero or negative");
    require(false, "not available atm");
    // require(gems >= amount, "not enough gems");

  }


  function canBuy(int32 price, int32 gems, int32 eth) public returns(bool) {



  }

  function manifest(uint32 item) public returns (bool) {
    if (item == 0) {}

    return false;
  }
}
