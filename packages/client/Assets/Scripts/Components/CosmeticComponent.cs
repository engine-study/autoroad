using UnityEngine;
using mudworld;
using mud;

public enum CosmeticType {None, Head, Body, Effect}
public class CosmeticComponent : ValueComponent
{
    [Header("Cosmetic")]
    public CosmeticType cosmetic;
    public GameObject body;
    [SerializeField] GaulItem[] bodies;

    [Header("Debug")]
    [SerializeField] protected int costumeIndex;
    [SerializeField] protected PlayerMUD player;

    protected override void Init(SpawnInfo newInfo) {
        base.Init(newInfo);
        ToggleRequiredComponent(true, MUDWorld.FindTable<PlayerComponent>().Prefab);

    }
    protected override void PostInit() {
        base.PostInit();

        player = Entity.GetMUDComponent<PlayerComponent>().PlayerScript;
        player.SetCosmetic(cosmetic, body);
        
    }

    
    protected override StatType SetStat(IMudTable update){ return StatType.None; }
    protected override float SetValue(IMudTable update) {return 1;}
    protected override string SetString(IMudTable update){ return "";}

}
