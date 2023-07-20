using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class HealthComponent : MUDComponent {

    public int health;
    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {
        health = (int)(table as HealthTable).value;
    }
}
