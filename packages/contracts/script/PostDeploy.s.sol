// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0;

import { Script } from "forge-std/Script.sol";
import { console } from "forge-std/console.sol";
import { IWorld } from "../src/codegen/world/IWorld.sol";
import { GameState, GameConfig, MapConfig, RoadConfig } from "../src/codegen/Tables.sol";
import { positionToEntityKey } from "../src/utility/positionToEntityKey.sol";
import { RoadSubsystem } from "../src/systems/RoadSubsystem.sol";
import { MapSubsystem } from "../src/systems/MapSubsystem.sol";

contract PostDeploy is Script {
  function run(address worldAddress) external {
    console.log("Deployed world: ", worldAddress);
    IWorld world = IWorld(worldAddress);

    uint256 deployerPrivateKey = vm.envUint("PRIVATE_KEY");
    vm.startBroadcast(deployerPrivateKey);

    bool debug = true; 
    bool dummyPlayers = true; 
    bool roadComplete = true; 

    //deploys the MapConfig
    GameState.set(world, int32(-1), 0);
    GameConfig.set(world, debug, dummyPlayers, roadComplete);
    MapConfig.set(world, 10, 13);
    RoadConfig.set(world, 3, 10, -1, 1);

    world.createMap(worldAddress);
    world.createMile(0);

    // some debug to check if our abiencode is working
    // abiTest(worldAddress);

    vm.stopBroadcast();
  }

  function abiTest(address worldAddress) public {
    bytes32 positionHash = keccak256(abi.encode(5, 10));
    bytes32 worldHash = keccak256(abi.encode(worldAddress));
    bytes32 combinedHash = keccak256(abi.encode(worldAddress, 5, 10));
    bytes32 stringInt = keccak256(abi.encode("hello", 5, 10));

    console.logBytes32(worldHash);
    console.logBytes32(positionHash);
    console.logBytes32(combinedHash);
    console.logBytes32(stringInt);
  }
}
