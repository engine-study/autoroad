using UnityEngine;
using mudworld;
using mud;

public class SwordComponent : PropComponent {
    protected override IMudTable GetTable() {return new SwordTable();}
}
