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

    GameState.set(world, 0);

    //deploys the MapConfig
    bytes memory terrain = new bytes(5);
    MapConfig.set(world, 13, 0, terrain);
    RoadConfig.set(world, 5, 20);

    //do we pass the world as an argument?
    MapConfig.set(world, 13, 0, new bytes(0));
    RoadConfig.set(world, 5, 20);

    world.createMap(worldAddress);

    world.createMile(0);
    world.createMile(1);
    world.createMile(2);
    world.createMile(3);
    world.createMile(4);

    bytes32 positionHash = keccak256(abi.encode(5, 10));
    bytes32 worldHash = keccak256(abi.encode(worldAddress));

    console.log("world: ", bytes32ToString(worldHash));
    console.log("position: ", bytes32ToString(positionHash));

    vm.stopBroadcast();
  }

  function bytes32ToString(bytes32 _bytes32) public pure returns (string memory) {
    uint8 i = 0;
    bytes memory bytesArray = new bytes(64);
    for (i = 0; i < bytesArray.length; i++) {
      uint8 _f = uint8(_bytes32[i / 2] & 0x0f);
      uint8 _l = uint8(_bytes32[i / 2] >> 4);

      bytesArray[i] = toByte(_f);
      i = i + 1;
      bytesArray[i] = toByte(_l);
    }
    return string(bytesArray);
  }

  function toByte(uint8 _uint8) public pure returns (bytes1) {
    if (_uint8 < 10) {
      return bytes1(_uint8 + 48);
    } else {
      return bytes1(_uint8 + 87);
    }
  }
}
