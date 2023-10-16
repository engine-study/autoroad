// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;

import { Script } from "forge-std/Script.sol";
import { console } from "forge-std/console.sol";
import { IWorld } from "../src/codegen/world/IWorld.sol";
import { TerrainSubsystem } from "../src/systems/TerrainSubsystem.sol";
import { random } from "../src/utility/random.sol";

contract PostDeploy is Script {
  function run(address worldAddress) external {
    console.log("Deployed world: ", worldAddress);
    IWorld world = IWorld(worldAddress);

    uint256 deployerPrivateKey = vm.envUint("PRIVATE_KEY");
    vm.startBroadcast(deployerPrivateKey);

    //deploys the MapConfig
    world.sup();
    world.createWorld();
    world.createMile();

    // some debug to check if our abiencode is working
    // abiTest(worldAddress);
    // randomTest(worldAddress);
    
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

  function randomTest(address worldAddress) public {

    console.log("0 - 1");
    console.logInt(int(random(0,1)));
    console.logInt(int(random(0,1)));
    console.logInt(int(random(0,1)));
    console.logInt(int(random(0,1)));
    console.logInt(int(random(0,1)));
    console.logInt(int(random(0,1)));
    console.logInt(int(random(0,1)));
    console.logInt(int(random(0,1)));
    console.logInt(int(random(0,1)));

    console.log("0 - 2");
    console.logInt(int(random(0,2)));
    console.logInt(int(random(0,2)));
    console.logInt(int(random(0,2)));
    console.logInt(int(random(0,2)));
    console.logInt(int(random(0,2)));
    console.logInt(int(random(0,2)));
    console.logInt(int(random(0,2)));
    console.logInt(int(random(0,2)));
    console.logInt(int(random(0,2)));

  }
}
