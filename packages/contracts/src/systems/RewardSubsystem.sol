pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";
import { Player, GameState, Road, Coinage, Gem, Conscription, XP, NPC, Soldier, Barbarian} from "../codegen/Tables.sol";
import { NPCType } from "../codegen/Types.sol";
import { ItemSubsystem } from "./ItemSubsystem.sol";

contract RewardSubsystem is System {

  function killRewards(bytes32 causedBy, bytes32 target, bytes32 attacker) public {
    IWorld world = IWorld(_world());

    //reward the Player and NPC for their actions
    uint32 attackerType = NPC.get(attacker);
    uint32 targetType = NPC.get(target);

    if(targetType == uint32(NPCType.Barbarian)) {
      if(Player.get(causedBy)) { world.giveKilledBarbarianReward(causedBy);}
      if(attackerType == uint32(NPCType.Soldier)) world.giveKilledBarbarianReward(attacker);
    }
  }

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

  function givePuzzleReward(bytes32 player) public {
    giveGem(player, 1);
    giveCoins(player, Conscription.get(player) ? int32(50) : int32(25));
    giveXP(player, 100);
  }

  function giveKilledBarbarianReward(bytes32 player) public {

    int32 amount = Conscription.get(player) ? int32(20) : int32(10);
    giveCoins(player, amount);
    giveXP(player, 50);

  }

  function giveRoadShoveledReward(bytes32 player) public {
    int32 amount = Conscription.get(player) ? int32(10) : int32(5);

    giveCoins(player, amount);
    giveXP(player, 10);

  }

  function giveRoadFilledReward(bytes32 player) public {

    int32 coins = Coinage.get(player);
    int32 amount = Conscription.get(player) ? int32(30) : int32(15);

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

  function giveGem(bytes32 player, int32 amount) public {
    int32 gems = Gem.get(player) + amount;
    Gem.set(player, gems);
  }

}
