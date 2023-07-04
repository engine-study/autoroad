// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0;

import { Script } from "forge-std/Script.sol";
import { console } from "forge-std/console.sol";
import { IWorld } from "../src/codegen/world/IWorld.sol";
import { GameState, MapConfig, RoadConfig, Chunk, Obstruction, Position, Rock, Row } from "../src/codegen/Tables.sol";
import { positionToEntityKey } from "../src/utility/positionToEntityKey.sol";
import { RoadSystem } from "../src/systems/RoadSystem.sol";
import { MapSystem } from "../src/systems/MapSystem.sol";

contract PostDeploy is Script {
  
  function run(address worldAddress) external {
    console.log("Deployed world: ", worldAddress);
    IWorld world = IWorld(worldAddress);

    uint256 deployerPrivateKey = vm.envUint("PRIVATE_KEY");
    vm.startBroadcast(deployerPrivateKey);

    GameState.set(0);

    //deploys the MapConfig
    bytes memory terrain = new bytes(0);
    MapConfig.set(13, 0, terrain);
    RoadConfig.set(5, 20);

    //do we pass the world as an argument? 
    // MapConfig.set(world, 13, 0, new bytes(0));
    // RoadConfig.set(world, 5, 20);

    // world.createMap(worldAddress);

    // world.createMile(0);
    // world.createMile(1);
    // world.createMile(2);
    // world.createMile(3);
    // world.createMile(4);

    vm.stopBroadcast();
  }
}
