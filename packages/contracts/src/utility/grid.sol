// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { MapConfig, Position, PositionTableId, PositionData } from "../codegen/Tables.sol";

// //returns the positions from a point plus an added vector
// function fromToLinePositions(
//   PositionData memory start,
//   PositionData memory delta
// ) public pure returns (PositionData[] memory) {
//   //check
//   int32 moveDelta = delta.x != 0 ? delta.x : delta.y;
//   int32 sign = moveDelta >= 0 ? 1 : -1;
//   uint32 steps = moveDelta * sign;

//   PositionData[] memory positions = new PositionData[](steps);

//   for (uint32 i = 1; i < steps + 1; i++) {
//     positions[i - 1] = PositionData(start.x + i, start.y);
//   }

//   return positions;
// }

function lineWalkPositions(
  PositionData memory start,
  PositionData memory end
) pure returns (PositionData[] memory) {
  //get the change of x and y
  int32 deltaX = end.x - start.x;
  int32 deltaY = end.y - start.y;

  //get the sign of the delta (positive or negative)
  int32 signX = deltaX >= 0 ? int32(1) : int32(-1);
  int32 signY = deltaY >= 0 ? int32(1) : int32(-1);

  //choose either an x or y line walk
  int32 change = deltaX != 0 ? deltaX : deltaY;
  int32 sign = deltaX != 0 ? signX : signY;

  //create an array of all the positions we will traverse (multiple by sign so its always a positive value)
  uint32 arraySize = uint32((change + sign) * sign);
  PositionData[] memory positions = new PositionData[](arraySize);

  //start index of walk
  int32 xIndex = start.x;
  int32 yIndex = start.y;
  uint32 index = 0;

  while (xIndex != end.x + signX && deltaX != 0) {
    positions[index] = PositionData(xIndex, start.y);
    index++;
    xIndex += signX;
  }

  while (yIndex != end.y + signY && deltaY != 0) {
    positions[index] = PositionData(start.x, yIndex);
    index++;
    yIndex += signY;
  }

  return positions;
}
