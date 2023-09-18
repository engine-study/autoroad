// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, RoadConfig, Bounds, Position, PositionTableId, PositionData, GameState } from "../codegen/Tables.sol";
import { Puzzle, Trigger, Miliarium } from "../codegen/Tables.sol";
import { TerrainType, PuzzleType } from "../codegen/Types.sol";

import { MapSubsystem } from "./MapSubsystem.sol";

import { random, randomCoord } from "../utility/random.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { getUniqueEntity } from "@latticexyz/world/src/modules/uniqueentity/getUniqueEntity.sol";

contract PuzzleSubsystem is System {

  function createRandomPuzzle(bytes32 causedBy) public {
    
    IWorld world = IWorld(_world());

    PuzzleType puzzleType = PuzzleType(random(0, uint32(PuzzleType.Count)));
    int32 mileNumber = GameState.getMiles();
    int32 roadRight = RoadConfig.getRight();

    int32 playWidth = MapConfig.getPlayWidth();
    int32 up = Bounds.getUp();
    int32 down = Bounds.getDown();

    if (puzzleType == PuzzleType.Miliarium) {
        createMiliarium(causedBy, roadRight, playWidth, up, down );
    } else if(puzzleType == PuzzleType.Bearer) {
        // createBearer(causedBy);
    }

  }

  function createMiliarium(bytes32 causedBy, int32 roadSide, int32 width, int32 up, int32 down) public {

    bytes32 mil = getUniqueEntity();
    bytes32 trigger = getUniqueEntity();

    Position.set(mil, findEmptyPositionInArea(width, roadSide, up, down));
    Position.set(trigger, findEmptyPositionInArea(width, roadSide, up, down));

  }

  //finds a position and deletes the object on it
  function findEmptyPositionInArea(int32 width, int32 roadSide, int32 up, int32 down) public returns(PositionData memory pos) {

    bool isValid = false;

    while(isValid == false) {

      pos = getRandomPositionNotRoad(width, roadSide, down, up);

      //don't overrwrite other puzzles
      bytes32[] memory atPosition = getKeysWithValue( PositionTableId, Position.encode(pos.x, pos.y, 0));
      if(atPosition.length > 0) { isValid = Puzzle.get(atPosition[0]) != bytes32(0); }
      else {isValid = true;}

    }

    return pos;

  }

  function getRandomPositionNotRoad(int32 width, int32 roadSide, int32 up, int32 down) public view returns(PositionData memory pos) {

    //spawn on right side
    pos.x = int32(uint32(random(uint32(roadSide), uint32(width))));
    pos.y = int32(uint32(random(uint32(down), uint32(up))));

    //switch what side of the road we spawn on
    if(random(0,2) > 1) {
      pos.x = int32(-pos.x);
    }
  }

}
