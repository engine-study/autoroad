pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";
import { GameState, Road, Coinage, Gem } from "../codegen/Tables.sol";
import { ItemSystem } from "./ItemSystem.sol";

contract RewardSubsystem is System {

  function giveRoadReward(bytes32 road) public {
    bytes32 player = Road.getFilled(road);
    uint32 gems = Gem.get(player) + 1;
    Gem.set(player, gems);
    // Road.setGem(road, true);
    //TODO revert after unimud fix
    uint32 roadstate = Road.getState(road);
    Road.set(road, roadstate, player, true);
  }

  function giveCoins(bytes32 player, int32 amount) public {
    int32 coins = Coinage.get(player);
    Coinage.set(player, coins + amount);
  }
}
