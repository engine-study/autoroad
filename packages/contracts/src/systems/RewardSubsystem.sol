pragma solidity >=0.8.21;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { console } from "forge-std/console.sol";
import { Player, GameState, Road, Coinage, Gem, Conscription, XP, NPC, Soldier, Barbarian, Treasury} from "../codegen/index.sol";
import { NPCType } from "../codegen/common.sol";

contract RewardSubsystem is System {

  function killRewards(bytes32 causedBy, bytes32 target, bytes32 attacker) public {
    IWorld world = IWorld(_world());

    //reward the Player and NPC for their actions
    bool isBarbarian = Barbarian.get(target);
    if(isBarbarian) {
      //TODO can they double dip if attacker and causedBy are the same?
      if(NPC.get(causedBy) > 0 && causedBy != attacker) { giveKillReward(causedBy);}

      NPCType attackerType = NPCType(NPC.get(attacker));
      if(attackerType != NPCType.None) giveKillReward(attacker);
      return;
    }

    bool isSoldier = Soldier.get(target);
    if(isSoldier) {
      //do something bad to players for killing their own oldiers
      // if(NPC.get(causedBy) > 0 && causedBy != attacker) { doSomethingBad;}
      NPCType attackerType = NPCType(NPC.get(attacker));
      if(attackerType != NPCType.None) giveKillReward(attacker);
      return;
    }
    
  }

  function giveRoadLottery(bytes32 road) public {

    //TODO revert after unimud fix
    uint32 roadstate = Road.getState(road);
    bytes32 player = Road.getFilled(road);

    // Road.setGem(road, true);
    //set the road block has a gem
    Road.set(road, roadstate, player, true);

    giveGem(player, 1);

  }

  function givePuzzleReward(bytes32 player) public {
    giveCoins(player, Conscription.get(player) ? int32(50) : int32(25));
    giveXP(player, 20);
  }

  function giveKillReward(bytes32 player) public {
    int32 amount = Conscription.get(player) ? int32(50) : int32(25);
    giveCoins(player, amount);
    giveXP(player, 20);
  }

  function giveRoadFilledReward(bytes32 player) public {

    int32 coins = Coinage.get(player);
    int32 amount = Conscription.get(player) ? int32(20) : int32(10);

    giveCoins(player, amount);
    giveXP(player, 10);
  }

  function giveRoadShoveledReward(bytes32 player) public {
    int32 amount = Conscription.get(player) ? int32(10) : int32(5);
    giveCoins(player, amount);
    giveXP(player, 5);
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
