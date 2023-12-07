// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { IWorld } from "../codegen/world/IWorld.sol";
import { Action, Position, PositionData, Health } from "../codegen/index.sol";
import { ActionName } from "../codegen/common.sol";
import { Rules } from "./rules.sol";

library Actions {

  function setAction(bytes32 player, ActionName newAction, int32 x, int32 y) internal {
    Action.set(player, uint32(newAction), x, y, bytes32(0));
  }

  function setActionTargeted(bytes32 player, ActionName newAction, int32 x, int32 y, bytes32 target) internal {
    Action.set(player, uint32(newAction), x, y, target);
  }

  function deleteAt(IWorld world, PositionData memory pos) internal {
    bytes32[] memory atPosition = Rules.getKeysAtPosition(world,pos.x, pos.y, pos.layer);
    if(atPosition.length == 0) return;
    Position.deleteRecord(atPosition[0]);
    Health.deleteRecord(atPosition[0]);

  }
  
  function getRoadEntity(int32 x, int32 y) internal pure returns(bytes32) {return keccak256(abi.encode("Road", x, y));}
  function getPuzzleEntity(int32 puzzleNumber, bool isTrigger) internal pure returns(bytes32) {
    if(isTrigger) return keccak256(abi.encode("Trigger", puzzleNumber));
    else return keccak256(abi.encode("Puzzle", puzzleNumber));
  }
  function getChunkEntity(int32 mile) internal pure returns(bytes32) {return keccak256(abi.encode("Chunk", mile));}
  function getCarriageEntity() internal pure returns(bytes32) {return keccak256(abi.encode("Carriage"));}
  function getWorldColumnEntity() internal pure returns(bytes32) {return keccak256(abi.encode("WorldColumn"));}

}