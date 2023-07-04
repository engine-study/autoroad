using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;

[CreateAssetMenu(fileName = "Stats", menuName = "MUD/Components/Stats", order = 1)]
public class StatsMUD : mud.Client.MUDComponent
{
    [Header("Stats")]
    public int health;
    public int attack;
    public int energy;

}
