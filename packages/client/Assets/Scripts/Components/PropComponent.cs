using UnityEngine;
using System.Collections;
using mudworld;
using mud;

public abstract class PropComponent : ValueComponent {

    [Header("Prop")]
    public SPAnimationProp propPrefab;
    PositionSync pos;

    protected override StatType SetStat(IMudTable update){ return StatType.None; }
    protected override float SetValue(IMudTable update) {return 1;}
    protected override string SetString(IMudTable update){ return "";}
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

        if(SpawnInfo.Source == SpawnSource.InGame) {
            pos = Entity.GetRootComponent<PositionSync>();
            if (pos == null) { Debug.LogError("No position sync", this); return; }

            Entity.StartCoroutine(SpawnProp());
        }
    }

    IEnumerator SpawnProp() {

        Debug.Log($"{gameObject.name} + Spawning", this);
        
        yield return new WaitForSeconds(.5f);

        string prefab = "Prefabs/PropBuyEffect";

        SPResourceJuicy propEffect = SPResourceJuicy.SpawnResource(prefab, pos.Target, pos.Target.position + Vector3.up, Random.rotation);
        
        SPAnimationProp propForEffect = Instantiate(propPrefab, propEffect.transform.position, propEffect.transform.rotation, propEffect.transform);
        SPRotate rotate = propForEffect.gameObject.AddComponent<SPRotate>();
        rotate.space = Space.Self;
        rotate.rotateSpeed = Vector3.up * 360f;

        propEffect.SendResource();
        
    }

}
