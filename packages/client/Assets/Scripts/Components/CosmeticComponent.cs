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

    protected override void Init(SpawnInfo newInfo) {
        base.Init(newInfo);
        ToggleRequiredComponent(true, MUDWorld.FindTable<PlayerComponent>().Prefab);

    }
    protected override void PostInit() {
        base.PostInit();

        Cosmetic head = Entity.GetMUDComponent<PlayerComponent>().PlayerScript.headCosmetic;
        head.SetNewBody(body);

        Cosmetic robeCosmetic = Entity.GetMUDComponent<PlayerComponent>().PlayerScript.bodyCosmetic;
        robeCosmetic.SetNewBody(body);
        
    }

    
    protected override StatType SetStat(IMudTable update){ return StatType.None; }
    protected override float SetValue(IMudTable update) {return 1;}
    protected override string SetString(IMudTable update){ return "";}

}
