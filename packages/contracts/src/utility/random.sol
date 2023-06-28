// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0;

// //if called twice in the same block this will return the same number, call only for once in a while events
// function getPseudorandom() public view returns (uint256) {
//   uint256 randomNumber = uint256(keccak256(abi.encodePacked(block.timestamp, block.difficulty, block.number)));
//   return randomNumber;
// }
