pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";
import { GameState, Road, Coinage, Gem, Conscription, XP} from "../codegen/Tables.sol";
import { ItemSystem } from "./ItemSystem.sol";

contract RewardSubsystem is System {

  function giveRoadReward(bytes32 road) public {

    //TODO revert after unimud fix
    uint32 roadstate = Road.getState(road);
    bytes32 player = Road.getFilled(road);
    // Road.setGem(road, true);
    Road.set(road, roadstate, player, true);

    giveGem(player, 1);
    giveCoins(player, 25);
    giveXP(player, 25);

  }

  function giveKilledBarbarianReward(bytes32 player) public {

    int32 amount = Conscription.get(player) ? int32(30) : int32(20);
    giveCoins(player, amount);
    giveXP(player, 50);
    
  }

  
  function giveRoadShoveledReward(bytes32 player) public {

    int32 amount = Conscription.get(player) ? int32(4) : int32(2);

    giveCoins(player, amount);
    giveXP(player, 10);

  }


  function giveRoadFilledReward(bytes32 player) public {

    int32 coins = Coinage.get(player);
    int32 amount = Conscription.get(player) ? int32(8) : int32(4);

    giveCoins(player, amount);
    giveXP(player, 25);
  }

  function giveCoins(bytes32 player, int32 amount) public {
    int32 coins = Coinage.get(player);
    Coinage.set(player, coins + amount);
  }

  function giveXP(bytes32 player, uint256 amount) public {
    uint xp = XP.get(player);
    XP.set(player, xp + amount);
  }

  function giveGem(bytes32 player, uint32 amount) public {
    uint32 gems = Gem.get(player) + amount;
    Gem.set(player, gems);
  }

}
