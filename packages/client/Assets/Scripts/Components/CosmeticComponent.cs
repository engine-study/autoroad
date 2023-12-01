using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using mudworld;
using mud;
using UnityEditor;
using IWorld.ContractDefinition;

public class CosmeticComponent : ValueComponent
{
    public Renderer Cosmetic {get{return r;}}
    public GameObject Go {get{return Cosmetic?.gameObject;}}

    [Header("Cosmetic")]
    public CosmeticType cosmetic;
    public PlayerBody bodyLink;
    public string EnumName;

    [Header("Debug")]
    [SerializeField] int index;
    [SerializeField] bool[] owned;
    [SerializeField] GaulItem[] items;
    [SerializeField] protected PlayerMUD player;
    [SerializeField] Renderer r;
    public static CosmeticComponent [] Cosmetics;
    public Array enumValues;

    public static async void UpdateCosmetic(CosmeticType cosmetic, int index) {
        Array enumValues = Cosmetics[(int)cosmetic].enumValues;
        Debug.Log($"Setting {cosmetic} to {enumValues.GetValue(index)}");

        await TxManager.SendDirect<DressupFunction>(Convert.ToByte(cosmetic), Convert.ToUInt64(index));
    }

    protected override void Init(SpawnInfo newInfo) {
        base.Init(newInfo);

        ToggleRequiredComponent(true, MUDWorld.GetManager<PlayerTable>().Prefab);

        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        items = Resources.LoadAll<GaulItem>("Data/Store");
        slots = new List<InventorySlot>();

        for(int i = 0; i < items.Length ; i++) {
            if(items[i].bodyPart != bodyLink){continue;}
            slots.Add(new InventorySlot(){item = items[i]});
        }

    }
    protected override void PostInit() {
        base.PostInit();

        player = Entity.GetMUDComponent<PlayerComponent>().PlayerScript;
        player.SetCosmetic(bodyLink, Go);

        Type enumType = Type.GetType(EnumName);
        if (enumType == null || !enumType.IsEnum) {
            Debug.LogError("Invalid enum type " + EnumName, this);
            return;
        }     

        enumValues = Enum.GetValues(enumType);  

        if(Cosmetics == null) {Cosmetics = new CosmeticComponent[Enum.GetValues(typeof(CosmeticType)).Length];}

        if(Entity.Key == NetworkManager.LocalKey) {
            Cosmetics[(int)cosmetic] = this;
        }    
    }

    protected override void UpdateComponent(MUDTable update, UpdateInfo info) {
        base.UpdateComponent(update, info);
        index = (int)(uint)MUDTable.GetRecord(Entity.Key, MUDTableType)?.RawValue["index"];
        owned = ((object[])MUDTable.GetRecord(Entity.Key, MUDTableType)?.RawValue["owned"]).Cast<bool>().ToArray();

        slots[index].amount = owned[index] ? 1 : 0;

        r = transform.GetChild(index).GetComponentInChildren<Renderer>(true);
        visualPrefab = Go;

        if(Loaded) {
            player.SetCosmetic(bodyLink, Go);
        }

    }

    
    protected override StatType SetStat(MUDTable update){ return StatType.None; }
    protected override float SetValue(MUDTable update) {return 0;}
    protected override string SetString(MUDTable update){ return "";}

}
