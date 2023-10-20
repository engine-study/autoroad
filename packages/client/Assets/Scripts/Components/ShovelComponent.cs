using UnityEngine;
using mudworld;
using mud;

public class ShovelComponent : PropComponent {
 
    protected override IMudTable GetTable() {return new ShovelTable();}

}
