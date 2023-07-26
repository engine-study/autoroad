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
    RoadState: ["None", "Shoveled", "Statumen", "Rudus", "Nucleas", "Paved"],
    RockType: ["None", "Raw", "Statumen", "Pavimentum", "Rudus", "Nucleus"],
    StateType: ["Idle", "Dead", "Carrying"],
    MoveType: ["None", "Obstruction", "Shovel", "Carry", "Push"],
  },

  tables: {

    GameConfig: {
      keySchema: {},
      schema: {
        dummyPlayers: "bool",
        stressTest: "bool",
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
        width: "uint32",
        height: "uint32",
        terrain: "bytes",
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

    Position: {
      name: "Position",
      schema: {
        x: "int32",
        y: "int32",
      },
    },

    //player state   
    Player: "bool",
    State: "StateType",
    Move: "uint32",
    Carrying: "bytes32",
    Pavement: "bool",

    //properties
    Damage: "int32",
    Health: "int32",

    //unique objects
    Rock: "uint32",
    Road: "uint32",

    Tree: "bool",



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