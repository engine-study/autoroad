using UnityEngine;
using mudworld;
using mud;

public class AxeComponent : PropComponent {
 
    protected override IMudTable GetTable() {return new AxeTable();}

}
