using UnityEngine;
using mudworld;
using mud;

public class StickComponent : PropComponent {

    protected override IMudTable GetTable() {return new StickTable();}
}
