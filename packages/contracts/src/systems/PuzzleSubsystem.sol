// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, RoadConfig, Bounds, Position, PositionTableId, PositionData, GameState, Move, Weight, Rock } from "../codegen/Tables.sol";
import { Puzzle, Trigger, Miliarium } from "../codegen/Tables.sol";
import { TerrainType, PuzzleType, MoveType, RockType} from "../codegen/Types.sol";

import { MapSubsystem } from "./MapSubsystem.sol";
import { TerrainSubsystem } from "./TerrainSubsystem.sol";

import { random, randomFromEntitySeed } from "../utility/random.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";

contract PuzzleSubsystem is System {

  function triggerPuzzles(bytes32 causedBy, bytes32 entity, PositionData memory pos) public {

    PuzzleType puzzleType = PuzzleType(Puzzle.getPuzzleType(entity));

    //we aren't a puzzle, skip this trigger
    if(puzzleType == PuzzleType.None) return;

    if(puzzleType == PuzzleType.Miliarium) {
      //search underground for triggers (-1)
      bytes32[] memory atPosition = getKeysWithValue( PositionTableId, Position.encode(pos.x, pos.y, -1));
      if(atPosition.length > 0 && Trigger.get(atPosition[0]) == entity) {
        //success, freeze miliarium in place
        Move.set(entity, uint32(MoveType.Obstruction));
        IWorld(_world()).givePuzzleReward(causedBy);
        //TODO set to single setter
        Puzzle.set(entity, uint32(puzzleType), true);
      }
    }

  }

  function createPuzzleOnMile(bytes32 causedBy) public {
    int32 playWidth = MapConfig.getPlayWidth();
    int32 up = Bounds.getUp();
    int32 down = Bounds.getDown();
    createRandomPuzzle(causedBy, playWidth, up, down);
  }


  function createRandomPuzzle(bytes32 causedBy, int32 right, int32 up, int32 down ) public {
    
    IWorld world = IWorld(_world());

    PuzzleType puzzleType = PuzzleType(random(0, uint32(PuzzleType.Count)));

    console.log("create puzzle");

    //use mile number to spawn puzzles?
    // int32 mileNumber = GameState.getMiles();

    if (true || puzzleType == PuzzleType.Miliarium) {
        int32 roadRight = RoadConfig.getRight();
        createMiliarium(causedBy, right, up, down, roadRight );
    } else if(puzzleType == PuzzleType.Bearer) {
        // createBearer(causedBy);
    }

  }

  function createMiliarium(bytes32 causedBy, int32 width, int32 up, int32 down, int32 roadSide) public {

    IWorld world = IWorld(_world());

    bytes32 mil = world.getRoadEntity(-5, up);
    bytes32 trigger = world.getRoadEntity(5, up);

    console.log("miliarium");

    PositionData memory pos = findEmptyPositionInArea(mil, width, up, down, roadSide);
    
    //delete whatever was there and place puzzle
    world.deleteAt(pos.x, pos.y, pos.layer);
    Position.set(mil, pos);
    Miliarium.set(mil, true);
    Weight.set(mil, 1);
    Move.set(mil, uint32(MoveType.Push));
    Rock.set(mil, uint32(RockType.Miliarium));
    Puzzle.set(mil, uint32(PuzzleType.Miliarium), false);

    console.log("trigger");

    //put triggers underground
    pos = findEmptyPositionInArea(trigger, width, up, down, roadSide);
    pos.layer = int32(-1);

    Position.set(trigger, pos);
    Trigger.set(trigger, mil);

  }

  //finds a position and deletes the object on it
  function findEmptyPositionInArea(bytes32 entity, int32 width, int32 up, int32 down, int32 roadSide) public returns(PositionData memory pos) {

    console.log("find empty pos");

    bool isValid = false;
    uint attempts = 0;

    while(isValid == false) {

      isValid = true;
      pos = getRandomPositionNotRoad(entity, width, up, down, roadSide, attempts);

      //don't overwrite Puzzles or Triggers or Obstructions already on road
      bytes32[] memory atPosition = getKeysWithValue( PositionTableId, Position.encode(pos.x, pos.y, 0));
      if(atPosition.length > 0) { 
        //check for obstructions and puzzles
        MoveType move = MoveType(Move.get(atPosition[0]));
        PuzzleType puzzle = PuzzleType(Puzzle.getPuzzleType(atPosition[0]));
        if(puzzle != PuzzleType.None || move == MoveType.Obstruction) {
          isValid = false;
          console.log("mil blocked");
        }
      }
      
      //check for triggers if still valid
      if(isValid) {
        atPosition = getKeysWithValue( PositionTableId, Position.encode(pos.x, pos.y, -1));
        if(atPosition.length > 0) {
          bytes32 trigger = Trigger.get(atPosition[0]);
          if(trigger != bytes32(0)) {
            isValid = false;
            console.log("trigger blocked");
          }
        }
      }

      attempts++;
      require(attempts < 10, "ran out of valid positions");

    }

    return pos;

  }

  function getRandomPositionNotRoad(bytes32 causedBy, int32 width, int32 up, int32 down, int32 roadSide, uint seed) public view returns(PositionData memory pos) {
    
    //spawn on right side
    pos.x = int32(uint32(randomFromEntitySeed(uint(uint32(roadSide)), uint(uint32(width)), causedBy, seed )));
    pos.y = int32(uint32(randomFromEntitySeed(uint(uint32(down)), uint(uint32(up)), causedBy, seed )));
    pos.layer = 0;

    console.log("get random");
    console.logInt(int(pos.x));
    console.logInt(int(pos.y));

    //switch what side of the road we spawn on
    if(random(1,10) > uint(5)) {
      pos.x = int32(-pos.x);
    }
  }

}