using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class StatsMUD : MUDComponent {
    [Header("Stats")]
    public int health;
    public int attack;
    public int energy;

    protected override void UpdateComponent(IMudTable table, UpdateEvent eventType) {
    }

}
