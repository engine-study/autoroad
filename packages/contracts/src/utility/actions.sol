
// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { Action } from "../codegen/index.sol";
import { ActionType } from "../codegen/common.sol";

library Actions {
  function setAction(bytes32 player, ActionType newAction, int32 x, int32 y) internal {
    Action.set(player, uint32(newAction), x, y, bytes32(0));
  }
}