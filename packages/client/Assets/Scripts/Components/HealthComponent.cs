using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class HealthComponent : ValueComponent {

    public int Health {get{return health;}}
    public bool IsDead {get { return dead; } }
    [SerializeField] int health = 0;
    [SerializeField] bool dead;
    protected override IMudTable GetTable() {return new HealthTable();}
    protected override void PostInit() {
        base.PostInit();
        // Entity.Toggle(health > 0);
    }

    protected override float SetValue(IMudTable update) {return (int)((HealthTable)update).value;}
    protected override StatType SetStat(IMudTable update) {return StatType.Health;}

    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {
        base.UpdateComponent(table,newInfo);
        
        health = (int)(table as HealthTable).value;
        dead = health < 0 || newInfo.UpdateType == UpdateType.DeleteRecord;
    }
}
