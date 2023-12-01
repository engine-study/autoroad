
using System.Linq;
using UnityEngine;
using mudworld;
using mud;
using VisualDesignCafe.Nature.Materials.Editor;
using Unity.VisualScripting;
using WebSocketSharp;

public enum CosmeticType {None, Head, Body, Effect}
public class CosmeticComponent : ValueComponent
{
    [Header("Cosmetic")]
    public CosmeticType cosmetic;
    public GameObject body;
    [SerializeField] bool[] owned;
    [SerializeField] GaulItem[] bodies;

    [Header("Debug")]
    [SerializeField] protected int costumeIndex;
    [SerializeField] protected PlayerMUD player;

    protected override void Init(SpawnInfo newInfo) {
        base.Init(newInfo);
        ToggleRequiredComponent(true, MUDWorld.GetManager<PlayerTable>().Prefab);

    }
    protected override void PostInit() {
        base.PostInit();

        player = Entity.GetMUDComponent<PlayerComponent>().PlayerScript;
        player.SetCosmetic(cosmetic, body);
        
    }
    protected override void UpdateComponent(MUDTable update, UpdateInfo info) {
        base.UpdateComponent(update, info);
        owned = ((object[])MUDTable.GetRecord(Entity.Key, MUDTableType)?.RawValue["value"]).Cast<bool>().ToArray();
    }
    
    protected override StatType SetStat(MUDTable update){ return StatType.None; }
    protected override float SetValue(MUDTable update) {return 1;}
    protected override string SetString(MUDTable update){ return "";}

}
