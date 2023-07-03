import { mudConfig, resolveTableId } from "@latticexyz/world/register";

export default mudConfig({

  enums: {
    TerrainType: ["None", "Excavated", "Filled", "Pavement", "Rock", "Mine"],
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

    Mile: {
      dataStruct: false,
      schema: {
        width: "uint32",
        height: "uint32",
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

    //roadwork
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

    //unique objects
    Rock: {
      name: "Rock",
      schema: {
        size: "int32",
        rockType: "ObjectType",
      },
    },



  },
  modules: [
    {
      name: "KeysWithValueModule",
      root: true,
      args: [resolveTableId("Position")],
    },

  ],
});