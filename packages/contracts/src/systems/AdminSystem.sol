// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadSubSystem } from "./RoadSubSystem.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";

contract AdminSystem is System {

    function isAdmin(bytes32 player) public returns(bool) {
        return true;
    }
  function spawnFinishedRoadAdmin(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld world = IWorld(_world());
    world.spawnFinishedRoad(x, y);
  }

  function spawnShoveledRoadAdmin(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(isAdmin(player), "not admin");
    IWorld world = IWorld(_world());
    world.spawnShoveledRoad(x, y);
  }
}
