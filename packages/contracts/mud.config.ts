import { mudConfig, resolveTableId } from "@latticexyz/world/register";

export default mudConfig({

  systems: {

    MapSystem: {
      name: "map",
      openAccess: false,
    },
    RoadSystem: {
      name: "road",
      openAccess: false,
    },

  },

  enums: {
    TerrainType: ["None", "Rock", "Mine", "Tree", "Player"],
    RoadState: ["None", "Shoveled", "Statumen", "Rudus", "Nucleas", "Paved", "Bones"],
    RockType: ["None", "Raw", "Statumen", "Pavimentum", "Rudus", "Nucleus"],
    StateType: ["Idle", "Dead", "Carrying"],
    MoveType: ["None", "Obstruction", "Shovel", "Carry", "Push"],
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
      dataStruct:false,
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
      schema: {
        completed: "bool",
        mile: "int32",
        // // dynamic list of people who have helped build the mile
        // entities: "bytes32[]",
        //   //dynamic list of people who have helped build the mile
        // contributors: "bytes32[]",
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
    Stick : "bool",
    Robe : "int32",
    Head : "int32",
    Scroll : "uint32",
    Coinage : "int32",

    Position: {
      name: "Position",
      schema: {
        x: "int32",
        y: "int32",
      },
    },

    //player state   
    Active: "bool",
    Player: "bool",
    State: "StateType",
    Move: "uint32",
    Carrying: "bytes32",
    Boots: "uint32",
    FishingRod: "bool",

    //properties
    Damage: "int32",
    Health: "int32",
    Seeds: "uint32",

    //unique objects
    Rock: "uint32",
    Tree: "bool",
    Log: "bool",

    Road: {
      name: "Road",
      dataStruct: false,
      schema: {
        state: "uint32",
        filled: "bytes32",
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

    GameEvent: {
      ephemeral: true,
      schema: {
        eventType: "string",
      },
    },


  },

  modules: [
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