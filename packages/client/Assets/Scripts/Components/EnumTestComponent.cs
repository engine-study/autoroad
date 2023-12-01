using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class EnumTestComponent : MUDComponent
{
    public uint MinMove;
    public uint[] MaxMove;
    public int[] IntSmall;
    public System.Numerics.BigInteger[] IntBig;
    public System.Numerics.BigInteger[] UintBig;

     protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {

        EnumTestTable table = (EnumTestTable)update;
        
        MinMove = (uint)table.MinMove;
        MaxMove = table.MaxMove;
        IntSmall = table.IntSmall;
        IntBig = table.IntBig;
        UintBig = table.UintBig;

    }

}
