using UnityEngine;
using mudworld;
using mud;

public class PickaxeComponent : PropComponent {
 
    protected override MUDTable GetTable() {return new PickaxeTable();}

}
