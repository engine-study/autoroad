// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { IWorld } from "../codegen/world/IWorld.sol";
import { Action, Position, PositionData } from "../codegen/index.sol";
import { ActionType } from "../codegen/common.sol";
import { Rules } from "./rules.sol";

library Actions {

  function setAction(bytes32 player, ActionType newAction, int32 x, int32 y) internal {
    Action.set(player, uint32(newAction), x, y, bytes32(0));
  }

  function setActionTargeted(bytes32 player, ActionType newAction, int32 x, int32 y, bytes32 target) internal {
    Action.set(player, uint32(newAction), x, y, target);
  }

  function deleteAtRequire(IWorld world, PositionData memory pos) internal {
    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,pos.x, pos.y, pos.layer);
    require(atPosition.length > 0, "Nothing to delete");
    Position.deleteRecord(atPosition[0]);
  }

  function deleteAt(IWorld world, PositionData memory pos) internal {
    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,pos.x, pos.y, pos.layer);
    if(atPosition.length > 0) Position.deleteRecord(atPosition[0]);
  }
  
  function getRoadEntity(int32 x, int32 y) internal pure returns(bytes32) {return keccak256(abi.encode("Road", x, y));}
  function getChunkEntity(int32 mile) internal pure returns(bytes32) {return keccak256(abi.encode("Chunk", mile));}
  function getCarriageEntity() internal pure returns(bytes32) {return keccak256(abi.encode("Carriage"));}

}