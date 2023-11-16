// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;

import { Script } from "forge-std/Script.sol";
import { console } from "forge-std/console.sol";
import { IWorld } from "../src/codegen/world/IWorld.sol";
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


  // function randomTest(address worldAddress) public {

  //   console.log("0 - 1");
  //   console.logInt(int(random(0,1)));
  //   console.logInt(int(random(0,1)));
  //   console.logInt(int(random(0,1)));
  //   console.logInt(int(random(0,1)));
  //   console.logInt(int(random(0,1)));
  //   console.logInt(int(random(0,1)));
  //   console.logInt(int(random(0,1)));
  //   console.logInt(int(random(0,1)));
  //   console.logInt(int(random(0,1)));

  //   console.log("0 - 2");
  //   console.logInt(int(random(0,2)));
  //   console.logInt(int(random(0,2)));
  //   console.logInt(int(random(0,2)));
  //   console.logInt(int(random(0,2)));
  //   console.logInt(int(random(0,2)));
  //   console.logInt(int(random(0,2)));
  //   console.logInt(int(random(0,2)));
  //   console.logInt(int(random(0,2)));
  //   console.logInt(int(random(0,2)));

  // }
}
