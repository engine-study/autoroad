
using System.Linq;
using UnityEngine;
using mudworld;
using mud;
using VisualDesignCafe.Nature.Materials.Editor;
using Unity.VisualScripting;
using WebSocketSharp;
using UnityEditor;

public class CosmeticComponent : ValueComponent
{
    [Header("Cosmetic")]
    public CosmeticType cosmetic;
    public BodyPart bodyLink;
    [SerializeField] GameObject[] cosmetics;
    [SerializeField] GaulItem[] items;

    [Header("Debug")]
    [SerializeField] int index;
    [SerializeField] bool[] owned;
    [SerializeField] protected PlayerMUD player;

    protected override void Init(SpawnInfo newInfo) {
        base.Init(newInfo);
        ToggleRequiredComponent(true, MUDWorld.GetManager<PlayerTable>().Prefab);

    }
    protected override void PostInit() {
        base.PostInit();

        player = Entity.GetMUDComponent<PlayerComponent>().PlayerScript;
        player.SetCosmetic(cosmetic, cosmetics[index]);
        
    }
    protected override void UpdateComponent(MUDTable update, UpdateInfo info) {
        base.UpdateComponent(update, info);
        index = (int)MUDTable.GetRecord(Entity.Key, MUDTableType)?.RawValue["index"];
        owned = ((object[])MUDTable.GetRecord(Entity.Key, MUDTableType)?.RawValue["ownership"]).Cast<bool>().ToArray();

        if(Loaded) {
            player.SetCosmetic(cosmetic, cosmetics[index]);
        }
    }
    
    protected override StatType SetStat(MUDTable update){ return StatType.None; }
    protected override float SetValue(MUDTable update) {return 1;}
    protected override string SetString(MUDTable update){ return "";}

}
