using UnityEngine;
using System.Collections;
using mudworld;
using mud;

public abstract class PropComponent : ValueComponent {

    [Header("Prop")]
    public SPAnimationProp propPrefab;


    protected override StatType SetStat(MUDTable update){ return StatType.None; }
    protected override float SetValue(MUDTable update) {return 1;}
    protected override string SetString(MUDTable update){ return "";}
    protected override void Init(SpawnInfo spawnInfo) {
        base.Init(spawnInfo);

        if(propPrefab) {
            visualPrefab = propPrefab.gameObject;
            visualOffset = Vector3.up * 2f;
        }

        //add player as required component
        TableManager table = MUDWorld.GetManager<PlayerTable>();
        if(table == null) {Debug.LogError("Could not find table " + typeof(PlayerComponent).Name); return;}
        if (!RequiredTypes.Contains(table.Prefab.GetType())) {
            // Debug.Log(gameObject.name + " Adding our , thisrequired component.", gameObject);
            RequiredTypes.Add(table.Prefab.GetType());
        }
    }

}
