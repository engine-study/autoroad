using UnityEngine;
using mudworld;
using mud;

public abstract class PropComponent : ValueComponent {

    [Header("Prop")]
    public SPAnimationProp propPrefab;

    protected override void Init(SpawnInfo spawnInfo) {
        base.Init(spawnInfo);

        //add player as required component
        TableManager table = MUDWorld.FindTable<PlayerComponent>();
        if(table == null) {Debug.LogError("Could not find table " + typeof(PlayerComponent).Name); return;}
        if (!RequiredTypes.Contains(table.Prefab.GetType())) {
            // Debug.Log(gameObject.name + " Adding our , thisrequired component.", gameObject);
            RequiredTypes.Add(table.Prefab.GetType());
        }
    }

    protected override void PostInit() {
        base.PostInit();

        propPrefab.gameObject.SetActive(false);

        PlayerMUD player = Entity.GetMUDComponent<PlayerComponent>().PlayerScript;
        player.Animator.SetDefaultProp(propPrefab);
        player.Animator.ToggleProp(true, propPrefab);

    }

    protected override StatType SetStat(IMudTable update){ return StatType.None; }
    protected override float SetValue(IMudTable update) {return 1;}
    protected override string SetString(IMudTable update){ return "";}

}
