// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, RoadConfig, Bounds, Position, PositionTableId, PositionData, GameState, Move, Weight } from "../codegen/Tables.sol";
import { Puzzle, Trigger, Miliarium } from "../codegen/Tables.sol";
import { TerrainType, PuzzleType, MoveType } from "../codegen/Types.sol";

import { MapSubsystem } from "./MapSubsystem.sol";

import { random, randomFromEntitySeed } from "../utility/random.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { getUniqueEntity } from "@latticexyz/world/src/modules/uniqueentity/getUniqueEntity.sol";

contract PuzzleSubsystem is System {

  function triggerPuzzles(bytes32 causedBy, bytes32 entity, PositionData memory pos) public {

    //we aren't a puzzle, skip this trigger
    if(Puzzle.get(entity) == 0) return;

    //search underground for triggers (-1)
    bytes32[] memory atPosition = getKeysWithValue( PositionTableId, Position.encode(pos.x, pos.y, -1));
    if(atPosition.length > 0 && Trigger.get(atPosition[0]) == entity) {
      //success, freeze miliarium in place
      Move.set(entity, uint32(MoveType.Obstruction));
      IWorld(_world()).givePuzzleReward(causedBy);
    }

  }

  function createPuzzleOnMile(bytes32 causedBy) public {
    int32 playWidth = MapConfig.getPlayWidth();
    int32 up = Bounds.getUp();
    int32 down = Bounds.getDown();
    createRandomPuzzle(causedBy, playWidth, up, down);
  }

  function createRandomPuzzle(bytes32 causedBy, int32 playWidth, int32 up, int32 down ) public {
    
    IWorld world = IWorld(_world());

    PuzzleType puzzleType = PuzzleType(random(0, uint32(PuzzleType.Count)));
    int32 roadRight = RoadConfig.getRight();

    //use mile number to spawn puzzles?
    // int32 mileNumber = GameState.getMiles();

    if (true || puzzleType == PuzzleType.Miliarium) {
        createMiliarium(causedBy, playWidth, up, down, roadRight );
    } else if(puzzleType == PuzzleType.Bearer) {
        // createBearer(causedBy);
    }

  }

  function createMiliarium(bytes32 causedBy, int32 width, int32 up, int32 down, int32 roadSide) public {

    IWorld world = IWorld(_world());

    bytes32 mil = getUniqueEntity();
    bytes32 trigger = getUniqueEntity();

    PositionData memory pos = findEmptyPositionInArea(mil, width, up, down, roadSide);
    
    //delete whatever was there and place puzzle
    world.deleteAt(pos.x, pos.y, pos.layer);
    Position.set(mil, pos);
    Miliarium.set(mil, true);
    Weight.set(mil, 1);
    Move.set(mil, uint32(MoveType.Push));
    Puzzle.set(mil, uint32(PuzzleType.Miliarium));

    //put triggers underground
    pos = findEmptyPositionInArea(trigger, width, up, down, roadSide);
    pos.layer = int32(-1);

    Position.set(trigger, pos);
    Trigger.set(trigger, mil);

  }

  //finds a position and deletes the object on it
  function findEmptyPositionInArea(bytes32 entity, int32 width, int32 up, int32 down, int32 roadSide) public returns(PositionData memory pos) {

    bool isValid = false;
    uint attempts = 0;

    while(isValid == false) {

      isValid = true;
      pos = getRandomPositionNotRoad(entity, width, up, down, roadSide, attempts);

      //don't overwrite Puzzles or Triggers or Obstructions already on road
      bytes32[] memory atPosition = getKeysWithValue( PositionTableId, Position.encode(pos.x, pos.y, 0));
      if(atPosition.length > 0) { 
        //check for obstructions and puzzles
        isValid = Puzzle.get(atPosition[0]) == 0 && Move.get(atPosition[0]) != uint32(MoveType.Obstruction); 
      }
      
      //check for triggers if still valid
      if(isValid) {
        bytes32[] memory atTriggerPosition = getKeysWithValue( PositionTableId, Position.encode(pos.x, pos.y, -1));
        if(atTriggerPosition.length > 0) {isValid = Trigger.get(atTriggerPosition[0]) == bytes32(0);}
      }

      attempts++;
      require(attempts < 10, "ran out of valid positions");

    }

    return pos;

  }

  function getRandomPositionNotRoad(bytes32 causedBy, int32 width, int32 up, int32 down, int32 roadSide, uint seed) public view returns(PositionData memory pos) {

    //spawn on right side
    pos.x = int32(uint32(randomFromEntitySeed(uint32(roadSide), uint32(width), causedBy, seed )));
    pos.y = int32(uint32(randomFromEntitySeed(uint32(down), uint32(up), causedBy, seed )));
    pos.layer = 0;
    //switch what side of the road we spawn on
    if(random(1,10) > 5) {
      pos.x = int32(-pos.x);
    }
  }

}
