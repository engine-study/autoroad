// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, RoadConfig, Bounds, Position, PositionTableId, PositionData, GameState, Move, Weight, Rock, Health } from "../codegen/index.sol";
import { Puzzles, Puzzle, Trigger, Linker, Miliarium } from "../codegen/index.sol";
import { TerrainType, PuzzleType, MoveType, RockType } from "../codegen/common.sol";

import { SystemSwitch } from "@latticexyz/world-modules/src/utils/SystemSwitch.sol";
import { Rules } from "../utility/rules.sol";
import { Actions } from "../utility/actions.sol";
import { random, randomFromEntitySeed } from "../utility/random.sol";

contract PuzzleSubsystem is System {
  function triggerPuzzles(bytes32 causedBy, bytes32 entity, PositionData memory pos) public {
    PuzzleType puzzleType = PuzzleType(Puzzle.getPuzzleType(entity));

    //we aren't a puzzle, skip this trigger
    if (puzzleType == PuzzleType.None) return;

    IWorld world = IWorld(_world());

    bytes32 target = Linker.get(entity);
    //search underground for triggers (-1)
    bytes32[] memory atPosition = Rules.getKeysAtPosition(world, pos.x, pos.y, -1);
    if (atPosition.length > 0 && atPosition[0] == target) {
      //success, freeze miliarium in place
      Move.set(entity, uint32(MoveType.Obstruction));
      //TODO set to single setter
      Puzzle.set(entity, uint32(puzzleType), true);
      SystemSwitch.call(abi.encodeCall(world.givePuzzleReward, (causedBy)));
    }
  }

  function createPuzzleOnMile(bytes32 causedBy) public {
    int32 playWidth = MapConfig.getPlayWidth();
    int32 up = Bounds.getUp();
    int32 down = Bounds.getDown();
    createStatuePuzzle(causedBy, playWidth, up, down);
  }

  function createRandomPuzzle(bytes32 causedBy, PuzzleType puzzle, int32 right, int32 up, int32 down) public {
    // IWorld world = IWorld(_world());
    PuzzleType puzzleType = PuzzleType(random(0, uint32(PuzzleType.Count)));
    console.log("create puzzle");

    //use mile number to spawn puzzles?
    // int32 mileNumber = GameState.getMiles();

    if (puzzleType == PuzzleType.Miliarium) {
      createMiliarium(causedBy, right, up, down);
    } else {
      createStatuePuzzle(causedBy, right, up, down);
    }
  }

  function createStatuePuzzle(bytes32 causedBy, int32 width, int32 up, int32 down) public {
    IWorld world = IWorld(_world());
    int32 puzzle = Puzzles.get();
    int32 roadSide = RoadConfig.getRight();

    bytes32 statue = Actions.getPuzzleEntity(puzzle, false);
    bytes32 trigger = Actions.getPuzzleEntity(puzzle, true);

    console.log("statue");

    PositionData memory pos = findEmptyPositionInArea(statue, width, up, down, 0, roadSide);

    //delete whatever was there and place puzzle
    Actions.deleteAt(world, pos);

    //spawn statue
    Position.set(statue, pos);
    Miliarium.set(statue, true);
    Weight.set(statue, 1);
    Move.set(statue, uint32(MoveType.Push));
    Rock.set(statue, uint32(RockType.Statuae));
    Health.set(statue, 1);
    Puzzle.set(statue, uint32(PuzzleType.Statuae), false);
    Linker.set(statue, trigger);

    console.log("trigger");

    //put triggers underground
    pos = findEmptyPositionInArea(trigger, width, up, down, -1, roadSide);
    pos.layer = int32(-1);

    Position.set(trigger, pos);
    Trigger.set(trigger, true);

    //increment puzzle count
    Puzzles.set(puzzle + 1);
  }

  function createMiliarium(bytes32 causedBy, int32 width, int32 up, int32 down) public {
    IWorld world = IWorld(_world());
    int32 puzzle = Puzzles.get();
    int32 roadSide = RoadConfig.getRight();

    bytes32 mil = Actions.getPuzzleEntity(puzzle, false);
    bytes32 trigger = Actions.getPuzzleEntity(puzzle, true);

    console.log("miliarium");

    PositionData memory pos = findEmptyPositionInArea(mil, width, up, down, 0, roadSide);

    //delete whatever was there and place puzzle
    Actions.deleteAt(world, pos);
    Position.set(mil, pos);
    Miliarium.set(mil, true);
    Weight.set(mil, 1);
    Move.set(mil, uint32(MoveType.Push));
    Rock.set(mil, uint32(RockType.Miliarium));
    Puzzle.set(mil, uint32(PuzzleType.Miliarium), false);
    Linker.set(mil, trigger);

    console.log("trigger");

    //put triggers underground
    pos = PositionData(roadSide + 1, down + 4, -1);

    Position.set(trigger, pos);
    Trigger.set(trigger, true);

    //increment puzzle count
    Puzzles.set(puzzle + 1);
  }

  //finds a position and deletes the object on it
  function findEmptyPositionInArea(
    bytes32 entity,
    int32 width,
    int32 up,
    int32 down,
    int32 layer,
    int32 roadSide
  ) public returns (PositionData memory pos) {
    IWorld world = IWorld(_world());
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
  ) public view returns (PositionData memory pos) {
    //spawn on right side
    pos.x = int32(uint32(randomFromEntitySeed(uint(uint32(roadSide)), uint(uint32(width)), entity, seed)));
    pos.y = int32(uint32(randomFromEntitySeed(uint(uint32(down)), uint(uint32(up)), entity, seed*10)));
    pos.layer = 0;

    console.log("get random");
    console.logInt(int(pos.x));
    console.logInt(int(pos.y));

    //switch what side of the road we spawn on
    if (randomFromEntitySeed(1, 10, entity, seed*100) > uint(5)) {
      pos.x = int32(-pos.x);
    }
  }
}
