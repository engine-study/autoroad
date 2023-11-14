using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mudworld;
using mud;

public class StatsMUD : MUDComponent {
    [Header("Stats")]
    public int health;
    public int attack;
    public int energy;

    protected override MUDTable GetTable() {return new StatsTable();}
    protected override void UpdateComponent(MUDTable table, UpdateInfo newInfo) {
    }

}
