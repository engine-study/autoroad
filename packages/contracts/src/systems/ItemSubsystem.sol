// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { Player, Action, Conscription, Weight } from "../codegen/index.sol";
import { Coinage, Gem, Eth, XP, Scroll, Stick, Pickaxe, Axe, Sword, Robe, Head, Effect, Material, Boots, FishingRod, ScrollSwap, Seeds, Pocket } from "../codegen/index.sol";
import { PaymentType, CosmeticType, ArmorSet, EffectSet, MaterialSet } from "../codegen/common.sol";

import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
// import { GaulItems } from "../data/GaulItems.sol";

contract ItemSubsystem is System {
  
  function sendCoins(int32 amount) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
  }

  function createItemMapping() public {
    
  } 

  function buyItem(bytes32 sender, bytes32 seller, uint32 id, PaymentType payment) public {
    addToInventory(sender, id, payment);
  }

  function addToInventory(bytes32 player, uint32 id, PaymentType payment) private {

    int32 price = 0;

    //TODO instead have this read from some preset array

    //ITEMS
    if(id < 100) {

      if (id == 0) { //stick
        pay(player, 75, 0, 0, payment, 0);
        Stick.set(player, true);
      } 
      // else if (id == 1) { //robe
      //   pay(player, 0, 1, 0, payment, 0);

      // } 
      // else if (id == 2) { //pickelhaube
      //   pay(player, 0, 1, 0, payment, 0);

      // } 
      else if (id == 3) { //fishing rod
        pay(player, 50, 0, 0, payment, 0);
        FishingRod.set(player, true);
      } 
      else if (id == 4) { //grapeleaf
        pay(player, 100, 0, 0, payment, 0);
        int32 weight = Weight.get(player);
        Weight.set(player, weight - 1);
      } 
      else if (id == 5) { //scroll
        pay(player, 5, 0, 0, payment, 0);
        uint32 scrolls = Scroll.get(player);
        Scroll.set(player, scrolls + 1);
      } 
      else if (id == 6) { //conscription
        pay(player, 0, 0, 10000000000000000, payment, 0);
        Conscription.set(player, true);
      } 
      else if (id == 7) { //loincloth
        
      } 
      else if (id == 8) { //seed
        pay(player, 10, 0, 0, payment, 0);
        uint32 seeds = Seeds.get(player);
        Seeds.set(player, seeds+1);
      } 
      else if (id == 9) { //swapscroll
        pay(player, 10, 0, 0, payment, 0);
        uint32 scrolls = ScrollSwap.get(player);
        ScrollSwap.set(player, scrolls + 1);
      } 
      else if (id == 12) { //pickaxe
        pay(player, 10, 0, 0, payment, 0);
        Pickaxe.set(player, true);
      } 
      else if (id == 14) { //sword
        pay(player, 100, 0, 0, payment, 0);
        Sword.set(player, true);
      } 
      else if (id == 15) { //boots
        // pay(player, 100, 0, 0, payment, 0);
        // Boots.set(player, 1, 3);
      } 
      else if (id == 16) { //axe
        pay(player, 10, 0, 0, payment, 0);
        Axe.set(player, true);
      }
      else if (id == 17) { //pocket
        pay(player, 200, 0, 0, payment, 0);
        Pocket.set(player, true);
      }
    
    
    } 
    
    // HEAD
    else if(id < 200) {
      pay(player, 0, 1, 10000000000000000, payment, 0);
      buyCosmetic(player, CosmeticType.Head, id-200);
    }

    // ROBE
    else if(id < 300) {
      pay(player, 0, 1, 10000000000000000, payment, 0);
      buyCosmetic(player, CosmeticType.Robe, id-300);
    }
    // EFFECTS
    else if(id < 400) {
      pay(player, 0, 1, 10000000000000000, payment, 0);
      buyCosmetic(player, CosmeticType.Effect, id-400);
    }
    // MATERIALS
    else if(id < 500) {
      pay(player, 0, 1, 10000000000000000, payment, 0);
      buyCosmetic(player, CosmeticType.Material, id-500);
    }

  }

  function buyCosmetic(bytes32 player, CosmeticType cosmetic, uint index) public {

    bool[] memory ownership;
    
    if(cosmetic == CosmeticType.Head) {

      require(index < uint(ArmorSet.Count));
      ownership = Head.getOwned(player);
      require(ownership[index] == false, "Already have");

      ownership[index] = true;
      Head.set(player, uint8(index), ownership);
    
    } else if(cosmetic == CosmeticType.Robe) {

      require(index < uint(ArmorSet.Count));
      ownership = Robe.getOwned(player);
      require(ownership[index] == false, "Already have");

      ownership[index] = true;
      Robe.set(player, uint8(index), ownership);
    
    } else if(cosmetic == CosmeticType.Effect) {

      require(index < uint(EffectSet.Count));
      ownership = Effect.getOwned(player);
      require(ownership[index] == false, "Already have");

      ownership[index] = true;
      Effect.set(player, uint8(index), ownership);
    
    } else if(cosmetic == CosmeticType.Material) {

      require(index < uint(MaterialSet.Count));
      ownership = Material.getOwned(player);
      require(ownership[index] == false, "Already have");

      ownership[index] = true;
      Material.set(player, uint8(index), ownership);
    
    }

  }

  function pay(bytes32 account, int32 coinPrice, int32 gemPrice, uint256 ethPrice, PaymentType paymentType, int32 minLevel) private {
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

  function withdrawEth(bytes32 player, uint256 amount) private {
    require(amount > 0, "amount is zero or negative");
    // require(false, "not available atm");
    // require(gems >= amount, "not enough gems");

  }


  function canBuy(int32 price, int32 gems, int32 eth) public returns(bool) {

  }

  function manifest(uint32 item) public returns (bool) {
    if (item == 0) {}

    return false;
  }
}
