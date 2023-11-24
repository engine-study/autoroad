// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { PositionData } from "../codegen/index.sol";

//if called twice in the same block this will return the same number, call only for once in a while events
// function getPseudorandom() public view returns (uint256) {
//   uint256 randomNumber = uint256(keccak256(abi.encodePacked(block.timestamp, block.difficulty, block.number)));
//   return randomNumber;
// }



function random(uint minNumber,uint maxNumber) view returns (uint amount) {
     amount = uint(keccak256(abi.encodePacked(block.timestamp, msg.sender, block.number))) % (maxNumber-minNumber);
     amount = amount + minNumber;
     return amount;
} 

//todo create seed based off something harder to simulate
function randomFromEntity(uint minNumber,uint maxNumber, bytes32 entity) view returns (uint amount) {
     amount = uint(keccak256(abi.encodePacked(block.timestamp, msg.sender, block.number, entity))) % (maxNumber-minNumber);
     amount = amount + minNumber;
     return amount;
} 

function randomFromEntitySeed(uint minNumber,uint maxNumber, bytes32 entity, uint seed) view returns (uint amount) {
     amount = uint(keccak256(abi.encodePacked(block.timestamp, msg.sender, block.number, entity, seed))) % (maxNumber-minNumber);
     amount = amount + minNumber;
     return amount;
} 

function randomSeed(uint minNumber,uint maxNumber, uint seed) view returns (uint amount) {
     amount = uint(keccak256(abi.encodePacked(block.timestamp, msg.sender, block.number, seed))) % (maxNumber-minNumber);
     amount = amount + minNumber;
     return amount;
} 

function randomCoord(uint minNumber, uint maxNumber, int32 x, int32 y) view returns (uint amount) {
     amount = uint(keccak256(abi.encodePacked(x, y, block.timestamp, msg.sender, block.number))) % (maxNumber-minNumber);
     amount = amount + minNumber;
     return amount;
} 

function randomCoordSeed(uint minNumber, uint maxNumber, int32 x, int32 y, uint seed) view returns (uint amount) {
     amount = uint(keccak256(abi.encodePacked(x, y, block.timestamp, msg.sender, block.number, seed))) % (maxNumber-minNumber);
     amount = amount + minNumber;
     return amount;
} 

function randomDirection(bytes32 entity, int32 x, int32 y, uint seed ) view returns(PositionData memory vector) {
     uint amount = uint(keccak256(abi.encodePacked(entity, x, y, block.timestamp, msg.sender, block.number, seed))) % (4);
     if(amount == 0) {
          vector = PositionData(1,0,0);
     } else if(amount == 1) {
          vector = PositionData(0,-1,0);
     } else if(amount == 2) {
          vector = PositionData(-1,0,0);
     } else {
          vector = PositionData(0,1,0);
     }

     return vector;     
}