import { mudConfig } from "@latticexyz/world/register";
import { resolveTableId } from "@latticexyz/config";

export default mudConfig({

  systems: {

    MoveSubsystem: {
      name: "move",
      openAccess: false,
    },
    BehaviourSubsystem: {
      name: "behaviour",
      openAccess: false,
    },
    EntitySubsystem: {
      name: "entities",
      openAccess: false,
    },
    FloraSubsystem: {
      name: "flora",
      openAccess: false,
    },
    ItemSubsystem: {
      name: "item",
      openAccess: false,
    },
    PuzzleSubsystem: {
      name: "puzzle",
      openAccess: false,
    },
    TerrainSubsystem: {
      name: "terrain",
      openAccess: false,
    },
    RewardSubsystem: {
      name: "reward",
      openAccess: false,
    },
    SpawnSubsystem: {
      name: "spawn",
      openAccess: false,
    },
    ToolSubsystem: {
      name: "tool",
      openAccess: false,
    },
  },

  enums: {
    ActionName: ["None", "Idle", "Dead", "Mining", "Shoveling", "Stick", "Fishing", "Walking", "Buy", "Plant", "Push", "Chop", "Teleport", "Melee", "Hop", "Spawn", "Bow", "Swap", "Pocket", "Throw", "Destroy"],
    TerrainType: ["None", "Rock", "Trap", "Tree", "HeavyBoy", "HeavyHeavyBoy", "Pillar", "Road", "Hole"],
    NPCType: ["None", "Player", "Soldier", "Barbarian", "Ox", "BarbarianArcher", "Deer", "Taxman", "Shoveler", "Gargoyle"],
    RoadState: ["None", "Shoveled", "Statumen", "Rudus", "Nucleas", "Paved", "Bones"],
    RockType: ["None", "Raw", "Statumen", "Pavimentum", "Rudus", "Nucleus", "Miliarium", "Heavy", "HeavyHeavy", "Pillar", "Statuae", "Gargoyle", "Amphora"],
    MoveType: ["None", "Obstruction", "Hole", "Carry", "Push", "Trap", "Permanent"],
    FloraType: ["None", "Tree", "Oak", "Bramble"],
    PuzzleType: ["None", "Miliarium", "Bearer", "Statuae", "Count"],
    PaymentType: ["None", "Coins", "Gems", "Eth"],
    ArmorSet: ["None", "Armor", "Samurai", "Wizard", "Chibi", "Traitor", "Cityslicker", "TBD4", "TBD5", "Count"],
    EffectSet: ["None", "Fire", "Water", "Holy", "Woodland", "Static", "Nebula", "Stormy", "TBD4", "TBD5", "Count"],
    MaterialSet: ["None", "Lava", "Forest", "Obsidian", "Shiny", "Murkwood", "Crystal", "Angelic", "TBD5", "Count"],
    CosmeticType: ["None", "Head", "Robe", "Effect", "Material"],
  },

  tables: {

    GameConfig: {
      keySchema: {},
      valueSchema: {
        debug: "bool",
        dummyPlayers: "bool",
      },
    },

    Stats: {
      dataStruct: false,
      valueSchema: {
        startingMile: "int32",
      },
    },

    Name: {
      dataStruct: false,
      valueSchema: {
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
      valueSchema: {
        playWidth: "int32",
        playHeight: "int32",
        playSpawnWidth: "int32",
      },
    },

    RoadConfig: {
      //empty keySchema creates a singleton
      keySchema: {},
      dataStruct: false,
      valueSchema: {
        width: "uint32",
        left: "int32",
        right: "int32",
      },
    },

    Bounds: {
      keySchema: {},
      dataStruct: false,
      valueSchema: {
        left: "int32",
        right: "int32",
        up: "int32",
        down: "int32",
      },
    },

    GameState: {
      keySchema: {},
      dataStruct: false,
      valueSchema: {
        miles: "int32",
        unused: "int32",
      },
    },

    Chunk: {
      name: "Chunk",
      dataStruct: false,
      valueSchema: {
        mile: "int32",
        spawned: "bool",
        completed: "bool",
        roads: "uint32",
        blockCompleted: "uint256",

        // // dynamic list of people who have helped build the mile
        // entities: "bytes32[]",
        //   //dynamic list of people who have helped build the mile
        // contributors: "bytes32[]",
      },
    },

    Entities: {
      keySchema: {},
      valueSchema: {
        entities: "bytes32[]",
      },
    },

    //units
    NPC: "uint32",
    Soldier: "bool",
    Barbarian: "bool",
    Archer: "uint32",
    Seek: "uint32",
    Aggro: "uint32",
    Wander: "uint32",
    Fling: "uint32",
    Thrower: "uint32",
    Thief: "uint32",
    Cursed: "uint32",
    Animal: "bool",
    LastAction: "uint256",
    LastMovement: "uint256",

  //puzzle components try to be moved onto triggers (ie. Miliarli )
    Puzzle: { dataStruct: false, valueSchema: { puzzleType: "uint32", complete: "bool", solver:"bytes32"},},
    Linker: "bytes32",
    Trigger: "bool",
    Miliarium: "bool",

    Position: {
      name: "Position",
      valueSchema: {
        x: "int32",
        y: "int32",
        layer: "int32",
      },
    },

    //player state   
    Active: "bool",
    Player: "bool",
    Move: "uint32",
    
    //properties
    Damage: "int32",
    Health: "int32",
    Weight: "int32",
    
    //value
    Coinage: "int32",
    XP: "uint256",
    Gem: "int32",
    Eth: "uint256",
    Seeds: "uint32",
    Grapeleaf: "uint32",
    Conscription: "bool",

    //items
    Shovel: "bool",
    Pickaxe: "bool",
    Axe: "bool",
    Stick: "bool",
    Sword: "bool",
    Pocket: "bool",
    Carry: "bytes32",
    Bones: "bool",

    Head: {dataStruct: false, valueSchema: {index: "uint8", owned: "bool[]",},},
    Robe: {dataStruct: false, valueSchema: {index: "uint8", owned: "bool[]",},},
    Effect: {dataStruct: false, valueSchema: {index: "uint8", owned: "bool[]",},},
    Material: {dataStruct: false, valueSchema: {index: "uint8", owned: "bool[]",},},

    Scroll: "uint32",
    ScrollSwap: "uint32",
    FishingRod: "bool",
    Boots: {valueSchema: {minMove: "int32", maxMove: "int32",},},

    //unique objects
    EnumTest: {valueSchema: {minMove: "NPCType", maxMove: "uint8[]", intSmall: "int32[]", intBig: "int256[]", uintBig: "uint256[]"},},
    Rock: "uint32",
    Tree: "uint32",
    Log: "bool",
    Ox: "bool",

    Treasury: "uint256",


    //Behaviour 
    //Flee: "bool", (this will probably cause infinite loops) if a Seek chases a fleer

    Road: {
      name: "Road",
      dataStruct: false,
      valueSchema: {
        state: "uint32",
        filled: "bytes32",
        gem: "bool",
      },
    },

    Carriage: "bool",
    WorldColumn: "bool",
    Row: {  keySchema: {},  valueSchema: { value : "int32", },},
    Puzzles: {  keySchema: {},  valueSchema: { value : "int32", },},


    Action: {
      // offchainOnly: true,
      dataStruct: false,
      valueSchema: {
        action: "uint32",
        x: "int32",
        y: "int32",
        target: "bytes32",
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

  ],

});