// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { MapConfig, Position, PositionTableId, PositionData } from "../codegen/index.sol";
import { Puzzles, Puzzle, Trigger, Linker, Miliarium } from "../codegen/index.sol";
import { TerrainType, PuzzleType, MoveType, RockType } from "../codegen/common.sol";
import { Rules } from "./rules.sol";
import { random, randomFromEntitySeed } from "./random.sol";

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

function addPosition(PositionData memory pos, PositionData memory add) pure returns (PositionData memory) {
  return PositionData(pos.x + add.x, pos.y + add.y, add.layer);
}

function subtractPosition(PositionData memory pos, PositionData memory minus) pure returns (PositionData memory) {
  return PositionData(pos.x - minus.x, pos.y - minus.y, minus.layer);
}

function getVectorNormalized(PositionData memory start, PositionData memory end) pure returns (PositionData memory) {
  //get the sign of the delta (positive or negative)
  PositionData memory vector = subtractPosition(end, start);
  vector.x = vector.x == 0 ? int32(0) : (vector.x > 0 ? int32(1) : int32(-1));
  vector.y = vector.y == 0 ? int32(0) : (vector.y > 0 ? int32(1) : int32(-1));
  return vector;
}

function withinManhattanMinimum(PositionData memory start, PositionData memory end, uint distance) pure returns (bool) {
  distance += 1;
  uint d = abs(end.x - start.x) + abs(end.y - start.y);
  return d < distance && d > 0;
}

function withinManhattanDistance(
  PositionData memory start,
  PositionData memory end,
  uint distance
) pure returns (bool) {
  distance += 1;
  return abs(end.x - start.x) + abs(end.y - start.y) < distance;
}

function withinChessDistance(PositionData memory start, PositionData memory end, uint distance) pure returns (bool) {
  distance += 1;
  return abs(end.x - start.x) < distance && abs(end.y - start.y) < distance;
}

function getDistance(PositionData memory start, PositionData memory end) pure returns (uint) {
  return abs(end.x - start.x) + abs(end.y - start.y);
}

function lineWalkPositions(PositionData memory start, PositionData memory end) pure returns (PositionData[] memory) {
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
    positions[index] = PositionData(xIndex, start.y, 0);
    index++;
    xIndex += signX;
  }

  while (yIndex != end.y + signY && deltaY != 0) {
    positions[index] = PositionData(start.x, yIndex, 0);
    index++;
    yIndex += signY;
  }

  return positions;
}

function abs(int x) pure returns (uint) {
  return uint(x >= 0 ? x : -x);
}

//finds a position and deletes the object on it
function findEmptyPositionInArea(
  IWorld world,
  bytes32 entity,
  int32 width,
  int32 up,
  int32 down,
  int32 layer,
  int32 roadSide
) view returns (PositionData memory pos) {
  console.log("find empty pos");

  bool isValid = false;
  uint attempts = 0;

  while (isValid == false) {
    require(attempts < 10, "ran out of valid positions");
    isValid = true;

    pos = getRandomPositionNotRoad(entity, width, up, down, roadSide, attempts);

    if (layer == 0) {
      //don't overwrite Puzzles already on road
      bytes32[] memory atPosition = Rules.getKeysAtPosition(world, pos.x, pos.y, 0);
      if (atPosition.length > 0) {
        //check for puzzles
        PuzzleType puzzle = PuzzleType(Puzzle.getPuzzleType(atPosition[0]));
        if (puzzle != PuzzleType.None) {
          console.log("puzzle blocked");
          isValid = false;
          attempts++;
          continue;
        }
      }
    } else {
      //don't overwrite triggers already on road
      if (Rules.onRoad(pos.x, pos.y)) {
        console.log("road blocked");
        isValid = false;
        attempts++;
        continue;
      }

      //check for triggers if still valid
      bytes32[] memory atPosition = Rules.getKeysAtPosition(world, pos.x, pos.y, -1);
      if (atPosition.length > 0) {
        if (Trigger.get(atPosition[0])) {
          console.log("trigger blocked");
          isValid = false;
          attempts++;
          continue;
        }
      }
    }
  }

  return pos;
}

function getRandomPositionNotRoad(
  bytes32 entity,
  int32 width,
  int32 up,
  int32 down,
  int32 roadSide,
  uint seed
) view returns (PositionData memory pos) {
  //spawn on right side
  pos.x = int32(uint32(randomFromEntitySeed(uint(uint32(roadSide)), uint(uint32(width)), entity, seed)));
  pos.y = int32(uint32(randomFromEntitySeed(uint(uint32(down)), uint(uint32(up)), entity, seed * 10)));
  pos.layer = 0;

  console.log("get random");
  console.logInt(int(pos.x));
  console.logInt(int(pos.y));

  //switch what side of the road we spawn on
  if (randomFromEntitySeed(1, 10, entity, seed * 100) > uint(5)) {
    pos.x = int32(-pos.x);
  }
}

//ignore the center position
function neumanNeighborhoodOuter(
  PositionData memory center,
  int32 distance
) pure returns (PositionData[] memory) {
  uint length = uint((uint32(distance) * 4));
  uint index = 0;
  PositionData[] memory neighbors = new PositionData[](length);

  for (int32 x = int32(-distance); x <= distance; x++) {
    if (x == 0) continue;
    neighbors[index] = PositionData(center.x + x, center.y, 0);
    index++;
  }

  for (int32 y = int32(-distance); y <= distance; y++) {
    //don't cross over centre twice
    if (y == 0) continue;
    neighbors[index] = PositionData(center.x, center.y + y, 0);
    index++;
  }

  return neighbors;
}

function neumanNeighborhood(PositionData memory center, int32 distance) pure returns (PositionData[] memory) {
  uint length = uint((uint32(distance) * 4) + 1);
  uint index = 0;
  PositionData[] memory neighbors = new PositionData[](length);

  for (int32 x = int32(-distance); x <= distance; x++) {
    neighbors[index] = PositionData(center.x + x, center.y, 0);
    index++;
  }

  for (int32 y = int32(-distance); y <= distance; y++) {
    //don't cross over centre twice
    if (y == 0) continue;
    neighbors[index] = PositionData(center.x, center.y + y, 0);
    index++;
  }

  return neighbors;
}

function mooreNeighborhood(PositionData memory center) pure returns (PositionData[] memory) {
  PositionData[] memory neighbors = new PositionData[](9);
  uint256 index = 0;

  for (int32 x = -1; x <= 1; x++) {
    for (int32 y = -1; y <= 1; y++) {
      neighbors[index] = PositionData(center.x + x, center.y + y, 0);
      index++;
    }
  }

  return neighbors;
}

function activeEntities(IWorld world, PositionData[] memory positions) view returns (bytes32[] memory) {
  // console.log("activeEntities");
  bytes32[] memory neighbors = new bytes32[](positions.length);
  for (uint i = 0; i < positions.length; i++) {
    bytes32[] memory entities = Rules.getKeysAtPosition(world, positions[i].x, positions[i].y, 0);
    if (entities.length > 0) {
      neighbors[i] = entities[0];
    }
  }

  return neighbors;
}
