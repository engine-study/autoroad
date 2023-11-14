using UnityEngine;
using mudworld;
using mud;

public class ShovelComponent : PropComponent {
 
    protected override MUDTable GetTable() {return new ShovelTable();}

}
