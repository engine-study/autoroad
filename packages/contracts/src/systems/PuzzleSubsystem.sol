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
import { findEmptyPositionInArea } from "../utility/grid.sol";

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
      Move.set(entity, uint32(MoveType.Permanent));
      //TODO set to single setter
      Puzzle.set(entity, uint32(puzzleType), true, causedBy);
      SystemSwitch.call(abi.encodeCall(world.givePuzzleReward, (causedBy)));
    }
  }

  function createPuzzleOnMile(bytes32 causedBy) public {
    int32 playWidth = MapConfig.getPlayWidth();
    int32 up = Bounds.getUp();
    int32 down = Bounds.getDown();
    createStatuePuzzle(causedBy, playWidth, up, down);
  }

  function createStatuePuzzle(bytes32 causedBy, int32 width, int32 up, int32 down) public {
    IWorld world = IWorld(_world());
    int32 puzzle = Puzzles.get();
    int32 roadSide = RoadConfig.getRight();

    bytes32 statue = Actions.getPuzzleEntity(puzzle, false);
    bytes32 trigger = Actions.getPuzzleEntity(puzzle, true);

    console.log("statue");

    PositionData memory pos = findEmptyPositionInArea(world, statue, width, up, down, 0, roadSide);

    //delete whatever was there and place puzzle
    Actions.deleteAt(world, pos);

    //spawn statue
    Position.set(statue, pos);
    Miliarium.set(statue, true);
    Weight.set(statue, 1);
    Move.set(statue, uint32(MoveType.Push));
    Rock.set(statue, uint32(RockType.Statuae));
    Health.set(statue, 1);
    Puzzle.set(statue, uint32(PuzzleType.Statuae), false, bytes32(0));
    Linker.set(statue, trigger);

    console.log("trigger");

    //put triggers underground
    pos = findEmptyPositionInArea(world, trigger, width, up, down, -1, roadSide);
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

    PositionData memory pos = findEmptyPositionInArea(world, mil, width, up, down, 0, roadSide);

    //delete whatever was there and place puzzle
    Actions.deleteAt(world, pos);
    Position.set(mil, pos);
    Miliarium.set(mil, true);
    Weight.set(mil, 1);
    Move.set(mil, uint32(MoveType.Push));
    Rock.set(mil, uint32(RockType.Miliarium));
    Puzzle.set(mil, uint32(PuzzleType.Miliarium), false, bytes32(0));
    Linker.set(mil, trigger);

    console.log("trigger");

    //put triggers underground
    pos = PositionData(roadSide + 1, down + 4, -1);

    Position.set(trigger, pos);
    Trigger.set(trigger, true);

    //increment puzzle count
    Puzzles.set(puzzle + 1);
  }

}
