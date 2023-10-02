using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud;

public class HealthComponent : MUDComponent {

    public int Health {get{return health;}}
    public bool IsDead {get { return dead; } }
    [SerializeField] int health = 0;
    [SerializeField] bool dead;
    protected override IMudTable GetTable() {return new HealthTable();}
    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {
        health = (int)(table as HealthTable).value;
        dead = health < 0 || newInfo.UpdateType == UpdateType.DeleteRecord;
    }
}
