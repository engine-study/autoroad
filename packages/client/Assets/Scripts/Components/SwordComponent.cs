using UnityEngine;
using mudworld;
using mud;

public class SwordComponent : PropComponent {
    protected override MUDTable GetTable() {return new SwordTable();}
}
