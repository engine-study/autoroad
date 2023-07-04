// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { System } from "@latticexyz/world/src/System.sol";
import { MapConfig, Position, PositionTableId, Player, PositionData } from "../codegen/Tables.sol";
import { Player, Rock, Obstruction, Tree } from "../codegen/Tables.sol";
import { TerrainType, ObjectType } from "../codegen/Types.sol";

import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { positionToEntityKey } from "../utility/positionToEntityKey.sol";
import { lineWalkPositions } from "../utility/grid.sol";

contract SpawnSystem is System {

    function spawn(int32 x, int32 y, TerrainType tType) public {

        bytes32 entity = positionToEntityKey(x, y);
        
        Position.set(entity, x, y);
        Obstruction.set(entity, true);

        if(tType == TerrainType.Rock) {
            Rock.set(entity, 5, ObjectType.Statumen);
        } else if(tType == TerrainType.Tree) {
            Tree.set(entity, true);
        } else if(tType == TerrainType.Mine) {

        }
    }

}