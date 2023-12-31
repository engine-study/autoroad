using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mudworld;
using mud;

public class HealthComponent : ValueComponent {

    public int Health {get{return health;}}
    public bool IsDead {get { return dead; } }
    [SerializeField] int health = 0;
    [SerializeField] bool dead;
    protected override MUDTable GetTable() {return new HealthTable();}
    protected override void PostInit() {
        base.PostInit();
        // Entity.Toggle(health > 0);
    }

    protected override float SetValue(MUDTable update) {return (int)((HealthTable)update).Value;}
    protected override StatType SetStat(MUDTable update) {return StatType.Health;}

    protected override void UpdateComponent(MUDTable table, UpdateInfo newInfo) {
        base.UpdateComponent(table,newInfo);
        
        health = (int)(table as HealthTable).Value;
        dead = health < 0 || newInfo.UpdateType == UpdateType.DeleteRecord;
    }
}
