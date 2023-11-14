using UnityEngine;
using mudworld;
using mud;

public class StickComponent : PropComponent {

    protected override MUDTable GetTable() {return new StickTable();}
}
