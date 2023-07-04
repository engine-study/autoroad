// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0;

import { Script } from "forge-std/Script.sol";
import { console } from "forge-std/console.sol";
import { IWorld } from "../src/codegen/world/IWorld.sol";
import { GameState, MapConfig, Chunk, Obstruction, Position, Rock, Row } from "../src/codegen/Tables.sol";
import { positionToEntityKey } from "../src/utility/positionToEntityKey.sol";
import { RoadSystem } from "../src/systems/RoadSystem.sol";
import { MapSystem } from "../src/systems/MapSystem.sol";

contract PostDeploy is Script {

  function run(address worldAddress) external {
    console.log("Deployed world: ", worldAddress);
    IWorld world = IWorld(worldAddress);

    uint256 deployerPrivateKey = vm.envUint("PRIVATE_KEY");
    vm.startBroadcast(deployerPrivateKey);

  
    GameState.set(world, 0);
  
    world.createMap(worldAddress);
    world.createMile(uint32(0));
    // world.createMile(0);

    // //layer of ground
    // Map.set(abi.encode(0));
    
    // //layer of entities
    // Map.set(abi.encode(1));
    
    vm.stopBroadcast();
  }
}
