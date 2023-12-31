// SPDX-License-Identifier: MIT
pragma solidity >=0.8.21;
import { IWorld } from "../codegen/world/IWorld.sol";
import { MapConfig, RoadConfig, Bounds, Weight, Conscription } from "../codegen/index.sol";
import { Chunk, Position, PositionTableId, PositionData, Carriage, Move, Player, Health, Puzzle, NPC, LastAction, LastMovement } from "../codegen/index.sol";
import { MoveType } from "../codegen/common.sol";

import { withinManhattanMinimum } from "./grid.sol";

import { getKeysWithValue } from "@latticexyz/world-modules/src/modules/keyswithvalue/getKeysWithValue.sol";
import { PackedCounter } from "@latticexyz/store/src/PackedCounter.sol";

library Rules {
  function onSpawn(int32 x, int32 y) internal view returns (bool) {
    int32 up = Bounds.getUp();
    // int32 down = Bounds.getDown();
    int32 playWidth = MapConfig.getPlayWidth();
    int32 spawnWidth = MapConfig.getPlaySpawnWidth();
    return (x < int32(-playWidth) && x >= int32(-spawnWidth)) && (y <= up && y >= 0);
  }

  function getKeysAtPosition(IWorld world, int32 x, int32 y, int32 layer) internal view returns (bytes32[] memory) {
    (bytes memory staticData, PackedCounter encodedLengths, bytes memory dynamicData) = Position.encode(x, y, layer);
    //TODO system switch
    return getKeysWithValue(world, PositionTableId, staticData, encodedLengths, dynamicData);
  }

  function onRoad(int32 x, int32 y) internal view returns (bool) {
    // bound to map
    return x >= RoadConfig.getLeft() && x <= RoadConfig.getRight();
  }

  function getMileBounds(int32 mile) internal view returns (int32 left, int32 right, int32 up, int32 down) {
    right = MapConfig.getPlayWidth();
    left = int32(-right);

    int32 playHeight = MapConfig.getPlayHeight();
    down = mile * playHeight;
    up = down + playHeight + int32(-1);
  }

  //the coordinate exists inside the current and previous miles excluding spawn zone
  function onMap(int32 x, int32 y) internal view returns (bool) {
    // bound to map
    (int32 left, int32 right, int32 up, int32 down) = Bounds.get();
    return x >= int32(left) && x <= right && y <= up && y >= 0;
  }

  //the coordinate exists inside ALL CURRENT AND PREVIOUS MILES and INSIDE THE SPAWN ZONES
  function onWorld(int32 x, int32 y) internal view returns (bool) {
    int32 up = Bounds.getUp();
    int32 spawnWidth = MapConfig.getPlaySpawnWidth();
    return x >= int32(-spawnWidth) && x <= spawnWidth && y <= up && y >= 0;
  }

  function requirePlayer(bytes32 entity) internal view returns (bool) {
    require(Conscription.get(entity), "bot bot bot");
  }

  function canDoStuff(bytes32 entity) internal view returns (bool) {
    //TODO add game pausing global
    if (Health.get(entity) < 1) return false;
    return true;
  }

  function hasTicked(bytes32 entity) internal view returns (bool) {
    return LastMovement.get(entity) >= block.number;
  }

  function isTired(bytes32 entity) internal view returns (bool) {
    return LastAction.get(entity) >= block.number;
  }

  function requireInteractable(
    bytes32 player,
    PositionData memory playerPos,
    bytes32[] memory entities,
    uint distance
  ) internal {
    require(canInteract(player, playerPos, entities, distance), "bad interact");
  }

  function abs(int x) internal pure returns (int) {
    return x >= 0 ? x : -x;
  }

  function requirePushable(bytes32[] memory at) internal view {
    require(at.length > 0, "empty");
    require(isPushable(at[0]), "not push");
  }

  function isPushable(bytes32 pushable) internal view returns (bool) {
    uint32 move = Move.get(pushable);
    return move == uint32(MoveType.Push);
  }

  function canSquish(bytes32 incoming, bytes32 target) internal view returns (bool) {
    return NPC.get(incoming) == 0 && Weight.get(incoming) > Weight.get(target);
  }

  function canWalkOn(MoveType moveAt) internal pure returns (bool) {
    return (moveAt == MoveType.None || moveAt == MoveType.Trap);
  }

  function canCrush(MoveType moveAt) internal pure returns (bool) {
    return moveAt != MoveType.Obstruction && moveAt != MoveType.Permanent;
  }

  function canPlaceOn(MoveType moveAt) internal pure returns (bool) {
    return (moveAt == MoveType.None || moveAt == MoveType.Hole || moveAt == MoveType.Trap);
  }

  function requireIsFairGame(bytes32 entity) internal view returns (bool) {
    require(Player.get(entity) == false, "Cannot pocket player");
    require(Puzzle.getPuzzleType(entity) == 0, "Cannot pocket puzzle");
    require(Move.get(entity) != uint32(MoveType.Hole), "Cannot pocket hole");
    return true;
  }

  //can block projectiles and movement
  function canBlock(MoveType moveAt) internal pure returns (bool) {
    return moveAt != MoveType.None && moveAt != MoveType.Hole;
  }

  function requireCanPlaceOn(bytes32[] memory at) internal view {
    if (at.length == 0) return;
    MoveType move = MoveType(Move.get(at[0]));
    require(canPlaceOn(move), "cannot place on");
  }

  function requireOnMap(bytes32 at, PositionData memory pos) internal view {
    require(onMapOrSpawn(at, pos), "off world");
  }

  function onMapOrSpawn(bytes32 at, PositionData memory pos) internal view returns (bool) {
    if (Player.get(at)) {
      return onWorld(pos.x, pos.y);
    } else {
      return onMap(pos.x, pos.y);
    }
  }

  function isPushableNotEmpty(bytes32[] memory at) internal view returns (bool) {
    if (at.length == 0) return false;
    MoveType move = MoveType(Move.get(at[0]));
    if(move == MoveType.Obstruction || move == MoveType.Permanent) return false;
    return move == MoveType.Push;
  }

  function canProjectileCross(
    bytes32 player,
    PositionData memory from,
    PositionData memory to,
    uint distance
  ) internal view returns (bool) {
    // checks that the position is below the min and maximum distance and is not diagonal
    require(from.x == to.x || from.y == to.y, "cannot move diagonally ");
    require(withinManhattanMinimum(to, from, distance), "too far or too close");
    return true;
  }

  function requireLegalMove(
    bytes32 player,
    PositionData memory from,
    PositionData memory to,
    uint distance
  ) internal view returns (bool) {
    // checks that the position is below the min and maximum distance and is not diagonal
    require(from.x == to.x || from.y == to.y, "cannot move diagonally ");
    require(distance == 0 || withinManhattanMinimum(to, from, distance), "too far or too close");
    return true;
  }

  function canInteract(
    bytes32 player,
    PositionData memory playerPos,
    bytes32[] memory entities,
    uint distance
  ) internal returns (bool) {
    require(entities.length > 0, "empty position");
    //todo check off grid?
    //TODO this should be a parameter, not get()
    PositionData memory entityPos = Position.get(entities[0]);
    require(requireLegalMove(player, playerPos, entityPos, distance), "Bad move");

    return true;
  }

  function canInteractEmpty(
    bytes32 player,
    PositionData memory playerPos,
    PositionData memory entityPos,
    bytes32[] memory entities,
    uint distance
  ) internal view returns (bool) {
    require(entities.length == 0, "not empty");
    // checks that positions are where they should be, also that the entities actually have positions
    require(requireLegalMove(player, entityPos, playerPos, distance), "too far or too close");
    return true;
  }
}
