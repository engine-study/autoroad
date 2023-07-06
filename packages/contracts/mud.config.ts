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
    RoadState: ["None", "Shoveled", "Filled", "Paved"],
    RockType: ["Raw", "Statumen", "Rudus", "Nucleus", "Pavimentum"],
  },

  tables: {

    GameConfig: {
      keySchema: {},
      schema: {
        dummyPlayers: "bool",
        stressTest: "bool",
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

    RoadConfig: {
      //empty keySchema creates a singleton
      keySchema: {},
      dataStruct: false,
      schema: {
        width: "uint32",
        height: "uint32",
      },
    },

    GameState: {
      keySchema: {},
      dataStruct: false,
      schema: {
        miles: "uint32",
      },
    },

    Chunk: {
      openAccess: false, // it's a subsystem now!
      schema: {
        completed: "bool",
        mileNumber: "uint32",
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

    Road: "RoadState",

    //items
    Shovel: "bool",
    Pickaxe: "bool",

    //properties
    Obstruction: "bool",
    Shovelable: "bool",

    Position: {
      name: "Position",
      schema: {
        x: "int32",
        y: "int32",
      },
    },

    //player state   
    Player: "bool",
    Damage: "uint32",
    Health: "uint32",

    //properties
    Infinite: "bool",

    //unique objects
    Rock: {
      name: "Rock",
      dataStruct: false,
      schema: {
        rockType: "RockType",
      },
    },

    Tree: "bool",



  },
  modules: [
    {
      name: "KeysWithValueModule",
      root: true,
      args: [resolveTableId("Position")],
    },

  ],
});