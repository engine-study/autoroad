import { mudConfig, resolveTableId } from "@latticexyz/world/register";

export default mudConfig({

  systems: {

    MapSubsystem: {
      name: "map",
      openAccess: false,
    },
    MoveSubsystem: {
      name: "move",
      openAccess: false,
    },
    MilitiaSubsystem: {
      name: "militia",
      openAccess: false,
    },
    RoadSubsystem: {
      name: "road",
      openAccess: false,
    },
    RewardSubsystem: {
      name: "reward",
      openAccess: false,
    },
    FloraSubsystem: {
      name: "flora",
      openAccess: false,
    },
    EntitySubsystem: {
      name: "entities",
      openAccess: false,
    },

  },

  enums: {
    TerrainType: ["None", "Rock", "Mine", "Tree", "Player", "HeavyBoy", "HeavyHeavyBoy", "Pillar", "Ox", "Militia", "Road", "Ditch"],
    RoadState: ["None", "Shoveled", "Statumen", "Rudus", "Nucleas", "Paved", "Bones"],
    RockType: ["None", "Raw", "Statumen", "Pavimentum", "Rudus", "Nucleus"],
    ActionType: ["Idle", "Dead", "Mining", "Shoveling", "Stick", "Fishing", "Walking", "Buy", "Plant", "Push", "Chop", "Teleport", "Melee", "Hop", "Spawn"],
    AnimationType: ["Walk", "Hop", "Teleport", "Push"],
    MoveType: ["None", "Obstruction", "Hole", "Carry", "Push", "Trap"],
    FloraType: ["None", "Tree", "Oak", "Bramble"],
  },

  tables: {

    GameConfig: {
      keySchema: {},
      schema: {
        debug: "bool",
        dummyPlayers: "bool",
        roadComplete: "bool",
      },
    },

    Stats: {
      dataStruct: false,
      schema: {
        startingMile: "int32",
        kills: "int32",
        deaths: "int32",
        moves: "int32",
        mined: "int32",
        pushed: "int32",
        shoveled: "int32",
        completed: "int32",
      },
    },

    Name: {
      dataStruct: false,
      schema: {
        named: "bool",
        first: "uint32",
        middle: "uint32",
        last: "uint32",
      },
    },

    //map
    MapConfig: {
      //empty keySchema creates a singleton
      keySchema: {},
      dataStruct: false,
      schema: {
        playArea: "int32",
        spawnArea: "int32",
      },
    },

    Bounds: {
      keySchema: {},
      dataStruct: false,
      schema: {
        left: "int32",
        right: "int32",
        up: "int32",
        down: "int32",
      },
    },

    RoadConfig: {
      //empty keySchema creates a singleton
      keySchema: {},
      dataStruct: false,
      schema: {
        width: "uint32",
        height: "uint32",
        left: "int32",
        right: "int32",
      },
    },

    GameState: {
      keySchema: {},
      dataStruct: false,
      schema: {
        miles: "int32",
        playerCount: "int32",
      },
    },


    Chunk: {
      name: "Chunk",
      openAccess: false, // it's a subsystem now!
      dataStruct: false,
      schema: {
        completed: "bool",
        mile: "int32",
        pieces: "uint32",
        blockCompleted: "uint256",
        // // dynamic list of people who have helped build the mile
        // entities: "bytes32[]",
        //   //dynamic list of people who have helped build the mile
        // contributors: "bytes32[]",
      },
    },

    Entities: {
      dataStruct: false,
      schema: {
        width: "bytes32[]",
        height: "bytes32[]",
      },
    },

    Row: {
      dataStruct: false,
      schema: {
        segments: "uint32",
      },
    },

    //items
    Shovel: "bool",
    Pickaxe: "bool",
    Bones: "bool",
    Stick: "bool",
    Robe: "int32",
    Head: "int32",
    Scroll: "uint32",
    Coinage: "int32",
    Weight: "int32",

    Position: {
      name: "Position",
      schema: {
        x: "int32",
        y: "int32",
        layer: "int32",
      },
    },

    //player state   
    Active: "bool",
    Player: "bool",
    Move: "uint32",
    Carrying: "bytes32",
    FishingRod: "bool",
    Boots: {schema: {minMove: "int32", maxMove: "int32",},},

    //properties
    Damage: "int32",
    Health: "int32",
    Seeds: "uint32",
    Gem: "uint32",

    //unique objects
    Rock: "uint32",
    Boulder: "bool",
    Tree: "uint32",
    Log: "bool",
    Ox: "bool",
    Militia: "bool",
    Conscription: "bool",
    XP: "uint256",

    Road: {
      name: "Road",
      dataStruct: false,
      schema: {
        state: "uint32",
        filled: "bytes32",
        gem: "bool",
      },
    },

    Carriage: "bool",

    // Item: {
    //   dataStruct: false,
    //   schema: {
    //     name: "string",
    //     id: "uint32",
    //     equipped: "bool",
    //   },
    // },

    Action: {
      ephemeral: true,
      dataStruct: false,
      schema: {
        action: "uint32",
        x: "int32",
        y: "int32",
      },
    },

    Animation: {
      ephemeral: true,
      dataStruct: false,
      schema: {
        state: "uint32",
      },
    },

    GameEvent: {
      ephemeral: true,
      schema: {
        eventType: "string",
      },
    },


  },

  modules: [
    {
      name: "UniqueEntityModule",
      root: true,
    },
    {
      name: "KeysWithValueModule",
      root: true,
      args: [resolveTableId("Position")],
    },
    {
      name: "KeysWithValueModule",
      root: true,
      args: [resolveTableId("Chunk")],
    },
  ],

});