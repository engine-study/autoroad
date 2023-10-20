using UnityEngine;
using mudworld;
using mud;

public class PickaxeComponent : PropComponent {
 
    protected override IMudTable GetTable() {return new PickaxeTable();}

}
