// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, Damage, Position, PositionTableId, Player, PositionData, Health } from "../codegen/Tables.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../addressToEntityKey.sol";

contract MoveSystem is System {

  function push(int32 x, int32 y, int32 pushX, int32 pushY) public {

    bytes32 player = addressToEntityKey(address(_msgSender()));
    PositionData memory startPos = Position.get(player);

    (uint32 width, uint32 height,) = MapConfig.get();
    require(x < int32(width) && x > -1 && y < int32(height) && y > -1, "moving off grid");
    require(pushX < int32(width) && pushX > -1 && pushY < int32(height) && pushY > -1, "pushing off grid");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));
    require(atPosition.length == 1, "trying to push an empty spot"); 
    bytes32[] memory atPushPosition = getKeysWithValue(PositionTableId, Position.encode(pushX, pushY));
    require(atPushPosition.length != 1, "pushing into an occupied spot"); 

    //move push object first
    Position.set(atPosition[0],pushX,pushY);

    //and then player (which ovewrites where the push object was)
    Position.set(player,x,y);

  }


  function moveFrom(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    PositionData memory startPos = Position.get(player);

    (uint32 width, uint32 height,) = MapConfig.get();
    if(x >= int32(width)) {x = int32(width) -1;} 
    else if(x < 0) {x = 0;}

    if(y >= int32(height)) {y = int32(height) - 1;} 
    else if(y < 0) {y = 0;}

    require(startPos.x == x || startPos.y == y, "cannot move diagonally ");
    require(startPos.x != x || startPos.y != y, "cannot move in place");

    // get all the positions in the line we are walking
    PositionData[] memory positions = traversedPositions(startPos, PositionData(x, y));

    // iterate over all the positions we move over, stop at the first blockage
    for (uint i = 0; i < positions.length; i++) {
      PositionData memory pos = positions[i];
      bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(pos.x, pos.y));

      //if we hit an object or at the end of our walk, move to that position
      if (atPosition.length == 1 && i > 0) {

        Position.set(player, positions[i - 1]);
        return;

      } else if(i == positions.length - 1) {

        Position.set(player, positions[i]);
        return;

      }
    }

    require(false, "No available place to move");
  }

  function move(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    // check if there is a player at the position
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));
    require(atPosition.length == 0, "position occupied");

    Position.set(player, x, y);
  }

  function spawn(int32 x, int32 y) public {
    bytes32 playerEntity = addressToEntityKey(address(_msgSender()));
    require(!Player.get(playerEntity), "already spawned");

    Player.set(playerEntity, true);
    Position.set(playerEntity, x, y);
    Health.set(playerEntity, 100);
    Damage.set(playerEntity, 10);
  }

  function traversedPositions(
    PositionData memory start,
    PositionData memory end
  ) internal pure returns (PositionData[] memory) {
    int32 changeX = end.x - start.x;
    int32 changeY = end.y - start.y;

    int32 signX = changeX >= 0 ? int32(1) : int32(-1);
    int32 signY = changeY >= 0 ? int32(1) : int32(-1);

    int32 change = changeX != 0 ? changeX : changeY;
    int32 sign = changeX != 0 ? signX : signY;

    uint32 arraySize = uint32((change + sign) * sign);
    PositionData[] memory positions = new PositionData[](arraySize);

    int32 xIndex = start.x;
    int32 yIndex = start.y;

    uint256 index = 0;

    while (xIndex != end.x + signX && changeX != 0) {
      positions[index] = PositionData(xIndex, start.y);
      index++;
      xIndex += signX;
    }

    while (yIndex != end.y + signY && changeY != 0) {
      positions[index] = PositionData(start.x, yIndex);
      index++;
      yIndex += signY;
    }

    return positions;
  }

  function abs(int x) private pure returns (int) {
    return x >= 0 ? x : -x;
  }

}
