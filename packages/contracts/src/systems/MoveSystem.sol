// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;
import { console } from "forge-std/console.sol";
import { IWorld } from "../codegen/world/IWorld.sol";
import { System } from "@latticexyz/world/src/System.sol";
import { RoadConfig, MapConfig, Damage, Position, Player, Health, GameState, Bounds } from "../codegen/Tables.sol";
import { Road, Pavement, Move, State, Carrying, Rock, Tree, Bones, Name, Stats, GameEvent, Coinage } from "../codegen/Tables.sol";
import { PositionTableId, PositionData } from "../codegen/Tables.sol";
import { RoadState, RockType, MoveType, StateType } from "../codegen/Types.sol";
import { getKeysWithValue } from "@latticexyz/world/src/modules/keyswithvalue/getKeysWithValue.sol";
import { addressToEntityKey } from "../utility/addressToEntityKey.sol";
import { lineWalkPositions, withinManhattanDistance, withinChessDistance, getDistance, withinManhattanMinimum } from "../utility/grid.sol";
import { MapSystem } from "../systems/MapSystem.sol";
import { RoadSystem } from "../systems/RoadSystem.sol";
import { randomCoord } from "../utility/random.sol";

contract MoveSystem is System {
  //push
  function push(int32 x, int32 y, int32 pushX, int32 pushY) public {

    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(canDoStuff(player),"hmm");    
    
    PositionData memory startPos = Position.get(player);
    
    IWorld world = IWorld(_world());
    require(world.onMap(pushX, pushY), "off grid");
    require(withinManhattanDistance(PositionData(x, y), PositionData(pushX, pushY), 1), "too far to push");

    bytes32[] memory atPush = getKeysWithValue(PositionTableId, Position.encode(x, y));

    require(atPush.length > 0, "trying to push an empty spot");
    uint32 move = Move.get(atPush[0]);
    require(move == uint32(MoveType.Push), "pushing a non-pushable object");

    bytes32[] memory atDestination = getKeysWithValue(PositionTableId, Position.encode(pushX, pushY));

    assert(atPush.length < 2);

    bool canPush = true;

    //check if we are pushing rocks into a road ditch
    if (atDestination.length > 0) {
      //check if there is an obstruction
      bool obstruction = Move.get(atDestination[0]) != 0;
      require(obstruction == false, "pushing into an occupied spot");

      uint32 roadInt = Road.get(atDestination[0]);
      //there is a road here
      if (roadInt != 0) {
        require(roadInt == uint32(RoadState.Shoveled), "Road already full");

        //this has now become a fill()
        canPush = false;
        fill(atPush[0], atDestination[0]);
      }
    } else {}

    //move push object before then setting the pusher to the new position
    if (canPush) {
      Position.set(atPush[0], pushX, pushY);
    }

    //and then player (which ovewrites where the push object was)
    Position.set(player, x, y);
    // int32 stat = Stats.getPushed(player);
    // Stats.setPushed(player, stat + 1);

  }

  function fill(bytes32 filler, bytes32 hole) private {
    uint32 roadInt = Road.get(hole);

    // roadInt++;
    //go directly to finished road for now
    roadInt = uint32(RoadState.Paved);
    Road.set(hole, roadInt);

    //ROAD COMPLETE!!!
    if (roadInt == uint32(RoadState.Paved)) {
      // Pavement.set(keccak256(abi.encode("Pavement", x, y)), true);
      Position.deleteRecord(filler);
      Position.deleteRecord(hole);
    }

    int32 coins = Coinage.get(filler);
    Coinage.set(filler, coins + 5);

    // int32 stat = Stats.getCompleted(filler);
    // Stats.setCompleted(filler, stat + 1);

  }

  function canDoStuff(bytes32 player) private returns (bool) {
    require(Health.get(player) > 0, "we dead");
    return true;
  }

  function canInteract(bytes32 player, int32 x, int32 y, bytes32[] memory entities, uint distance) private returns (bool) {

    require(entities.length > 0, "empty position");

    PositionData memory playerPos = Position.get(player);
    PositionData memory entityPos = Position.get(entities[0]);

    // checks that positions are where they should be, also that the entities actually have positions
    require(withinManhattanMinimum(entityPos, playerPos, distance), "too far or too close");

    return true;
  }

  function mine(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(canDoStuff(player),"hmm");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));

    require(canInteract(player, x, y, atPosition, 1),"bad interact");

    uint32 rockState = Rock.get(atPosition[0]);

    require(rockState > uint32(RockType.None), "Rock not found or none");
    require(rockState < uint32(RockType.Nucleus), "Rock ground to a pulp");

    //increment the rock state
    rockState += 1;

    Rock.set(atPosition[0], rockState);

    //give rocks that are mined a pushable component
    if (rockState == uint32(RockType.Statumen)) {
      Move.set(atPosition[0], uint32(MoveType.Push));
    }
    //become shovelable once we are broken down enough
    else if (rockState == uint32(RockType.Rudus)) {
      Position.deleteRecord(atPosition[0]);
      // Move.set(atPosition[0], uint32(MoveType.Shovel));
    }

    // int32 stat = Stats.getMined(player);
    // Stats.setMined(player, stat + 1);

  }

  function chop(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(canDoStuff(player),"hmm");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));

    require(canInteract(player, x, y, atPosition, 1),"bad interact");
    require(Tree.get(atPosition[0]), "no tree");

    int32 health = Health.get(atPosition[0]);
    health--;

    if (health == 0) {
      Tree.deleteRecord(atPosition[0]);
      Position.deleteRecord(atPosition[0]);
      Health.deleteRecord(atPosition[0]);
      Move.deleteRecord(atPosition[0]);
    } else {
      Health.set(atPosition[0], health);
    }
  }

  function carry(int32 carryX, int32 carryY) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(canDoStuff(player),"hmm");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(carryX, carryY));

    // Position
    require(atPosition.length > 0, "trying to carry an empty spot");
    uint32 move = Move.get(atPosition[0]);
    require(move == uint32(MoveType.Carry), "non-carry object");

    Carrying.set(player, atPosition[0]);

    // Position.deleteRecord()
    // bytes32[] memory atPushPosition = getKeysWithValue(PositionTableId, Position.encode(pushX, pushY));
    // require(atPushPosition.length != 1, "pushing into an occupied spot");
  }

  function drop(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(canDoStuff(player),"hmm");

    bytes32 carrying = Carrying.get(player);
    require(carrying != 0, "Not carrying anything");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));

    if (atPosition.length == 0) {
      //drop rocks back on ground
    } else {
      //check to see if we can fill hole
    }
  }

  function shovel(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(canDoStuff(player),"hmm");

    IWorld world = IWorld(_world());
    require(world.onMap(x, y), "off grid");
    require(world.onRoad(x, y), "off road");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));

    require(atPosition.length < 1, "trying to dig an occupied spot");
    require(Pavement.get(keccak256(abi.encode("Pavement", x, y))) == false, "Digging on pavement");

    bytes32 roadEntity = keccak256(abi.encode("Road", x, y));

    Road.set(roadEntity, uint32(RoadState.Shoveled));
    Position.set(roadEntity, x, y);

    // int32 stat = Stats.getShoveled(player);
    // Stats.setShoveled(player, stat + 1);

  }

  function melee(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(canDoStuff(player),"hmm");

    PositionData memory pos = PositionData(x,y);
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));
    require(atPosition.length > 0, "attacking an empty spot");
    require(withinManhattanDistance(pos, Position.get(player), 1), "too far to attack");

    int32 health = Health.get(atPosition[0]);
    require(health > 0, "this thing on?");

    GameEvent.emitEphemeral(player,"melee");

    health--;

    if(health <= 0) {
      kill(player, atPosition[0], pos);
    } else {
      Health.set(atPosition[0], health);
    }

  }

  function kill(bytes32 attacker, bytes32 target, PositionData memory pos) private {

    //credit attacker with kill
    // int32 stat = Stats.getKills(attacker);
    // Stats.setKills(attacker, stat + 1);

    //kill target
    Health.set(target, -1);
    Position.deleteRecord(target);

    // int32 stat2 = Stats.getDeaths(target);
    // Stats.setDeaths(target, stat2 + 1);

    //spawn bones
    // bytes32 bonesEntity = keccak256(abi.encode("Bones", pos.x, pos.y));
    // Bones.set(bonesEntity, true);
    // Position.set(bonesEntity, pos);
    // Move.set(bonesEntity, uint32(MoveType.Push));

  }

  function teleport(int32 x, int32 y) public {
    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(canDoStuff(player),"hmm");        
    
    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));
    require(atPosition.length < 1, "occupied");
    Position.set(player, PositionData(x, y));
  }

  function moveFrom(int32 x, int32 y) public {

    IWorld world = IWorld(_world());

    bytes32 player = addressToEntityKey(address(_msgSender()));
    require(canDoStuff(player),"hmm");    
    
    PositionData memory startPos = Position.get(player);

    require(startPos.x == x || startPos.y == y, "cannot move diagonally ");
    require(startPos.x != x || startPos.y != y, "moving in place");

    PositionData memory endPos = PositionData(x, y);
    require(getDistance(startPos,endPos) == 5, "move distance is not 5");

    // get all the positions in the line we are walking
    PositionData[] memory positions = lineWalkPositions(startPos, endPos);

    // iterate over all the positions we move over, stop at the first blockage
    //START index at 1, ignoring our own position
    for (uint i = 1; i < positions.length; i++) {

      bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(positions[i].x, positions[i].y));
      assert(atPosition.length < 2);

      //if we hit an object or at the end of our walk, move to that position
      if (atPosition.length > 0 || world.onWorld(positions[i].x, positions[i].y) == false) {
        require(i > 1, "nowhere to move");
        Position.set(player, positions[i - 1]);
        return;
      } else if (i == positions.length - 1) {
        Position.set(player, positions[i]);
        return;
      }
    }

    require(false, "No available place to move");

    // int32 stat = Stats.getMoves(player);
    // Stats.setMoves(player, stat + 1);

  }

  function name(uint32 firstName, uint32 middleName, uint32 lastName) public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));
    (bool hasName,,,) = Name.get(entity);
    require(!hasName, "already has name");
    require(firstName < 36, "first name");
    require(middleName < 1025, "middle name");
    require(lastName < 1734, "last name");
    require(!Player.get(entity), "already spawned");
    Name.set(entity, true, firstName, middleName, lastName);

  }

  function spawn(int32 x, int32 y) public {

    bytes32 entity = addressToEntityKey(address(_msgSender()));
    bool playerExists = Player.get(entity);

    if(playerExists) {
      require(Health.get(entity) == -1, "not dead, can't respawn");
    }

    (,,int32 up, int32 down ) = Bounds.get();
    (int32 playWidth, int32 spawnWidth) = MapConfig.get();

    require(x > playWidth && x <= spawnWidth, "x outside spawn");
    require(y <= up && y >= down, "y outside of spawn");

    bytes32[] memory atPosition = getKeysWithValue(PositionTableId, Position.encode(x, y));
    require(atPosition.length < 1, "occupied");

    spawnPlayer(x, y, entity, false);
  }

  function spawnBot(int32 x, int32 y, bytes32 entity) public {
    Name.set(entity, true, uint32(randomCoord(0, 35, x,y)), uint32(randomCoord(0, 1024, x,y+1)), uint32(randomCoord(0, 1733, x,y+2) ));  
    spawnPlayer(x,y,entity, true);
  }

  function spawnPlayer(int32 x, int32 y, bytes32 entity, bool isBot) private {

    bool playerExists = Player.get(entity);

    if(!playerExists) {
      Player.set(entity, true);
      Coinage.set(entity, 0);

      (int32 miles, int32 players) = GameState.get();
      Stats.set(entity, miles, 0, 0, 0, 0, 0, 0, 0);
      if(!isBot) {
        GameState.set(miles, players+1);
        // GameState.setPlayerCount(playerCount + 1);
      }
    }

    Health.set(entity, 3);
    Move.set(entity, uint32(MoveType.Push));
    Position.set(entity, x, y);


  }

  function destroyPlayer() public {
    bytes32 entity = addressToEntityKey(address(_msgSender()));

    Name.deleteRecord(entity);
    Player.deleteRecord(entity);
    Health.deleteRecord(entity);
    Move.deleteRecord(entity);
    Position.deleteRecord(entity);
  }


  function abs(int x) private pure returns (int) {
    return x >= 0 ? x : -x;
  }
}
