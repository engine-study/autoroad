using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using mudworld;
using mud;
using UnityEditor;

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
    protected override float SetValue(MUDTable update) {return 1;}
    protected override string SetString(MUDTable update){ return "";}

}
