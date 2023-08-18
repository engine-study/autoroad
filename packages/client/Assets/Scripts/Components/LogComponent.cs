using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using IWorld.ContractDefinition;
public class LogComponent : MUDComponent {

    [Header("Tree")]
    public GameObject treeRoot;
    public ParticleSystem fx_hit, fx_fall;
    public AudioClip[] sfx_hits, sfx_falls;
    HealthComponent health;
    PositionComponent pos;
    Rigidbody rb;

    [Header("Debug")]
    public bool treeState;
    bool lastState = false;
    int lastHealth = -999;

    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Log");
    }

    protected override void InitDestroy() {
        base.InitDestroy();

    }

    protected override IMudTable GetTable() {return new LogTable();}
    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateInfo newInfo)
    {

    }
}
