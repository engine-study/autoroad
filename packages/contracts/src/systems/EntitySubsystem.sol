// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0;
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { Player, Position, PositionTableId, PositionData, Entities, Bounds } from "../codegen/Tables.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { MilitiaSubsystem } from "./MilitiaSubsystem.sol";

contract EntitySubsystem is System {
  
  function createEntities(bytes32 chunkEntity, int32 playArea, uint32 roadHeight) public {
    //set entities arrays
    uint256 totalWidth = uint256(uint32(playArea) + uint32(playArea) + 1);
    bytes32[] memory width = new bytes32[](totalWidth);
    bytes32[] memory height = new bytes32[](uint256(roadHeight));
    Entities.set(chunkEntity, width, height);
  }

  function triggerEntities(PositionData memory center) public {

    bytes32[] memory entities = mooreEntities(center);
    for(uint i = 0; i < 9; i++) {
        bytes32 entity = entities[i];
        IWorld(_world()).aggro(entity);
    }

  }

  function mooreEntities(PositionData memory center) internal returns (bytes32[] memory) {

    uint256 index = 0;
    (int32 left, int32 right, int32 up, int32 down) = Bounds.get();

    bytes32[] memory neighbors = new bytes32[](9);

    int32 posX = 0;
    int32 posY = 0;

      for (int32 x = -1; x <= 1; x++) {

        posX = center.x + x;    

        if(posX < left || posX > right) {
          index++;  
          continue;
        } 

        for (int32 y = -1; y <= 1; y++) {

            posY = center.y + y;

            if(posY < down || posY > up) {
                index++;  
                continue;
            } else {
                
                bytes32[] memory atPos = getKeysWithValue( PositionTableId, Position.encode(posX, posY, 0));
                if(atPos.length > 0) {
                    neighbors[index] = atPos[0];
                }

                index++;
            }
        }
      }

      return neighbors;
  }
}