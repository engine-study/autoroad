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
    SpawnSystem: {
      name: "spawn",
      openAccess: false, // it's a subsystem now!
    },

  },

  enums: {
    TerrainType: ["None", "Excavated", "Filled", "Pavement", "Rock", "Mine", "Tree"],
    ObjectType: ["Axe", "Statumen", "Rudus", "Nucleus", "Pavimentum"],
  },

  tables: {

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
      dataStruct: false,
      openAccess: false, // it's a subsystem now!
      schema: {
        completed: "bool",
        mileNumber: "uint32",

        //dynamic list of people who have helped build the mile
        entities: "bytes32[]",

          //dynamic list of people who have helped build the mile
        contributors: "bytes32[]",
      },
    },

    Row: {
      dataStruct: false,
      schema: {
        segments: "uint32",
      },
    },

    Road: {
      dataStruct: false,
      schema: {
        contributors: "bytes32[]",
      },
    },

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
      schema: {
        size: "int32",
        rockType: "ObjectType",
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