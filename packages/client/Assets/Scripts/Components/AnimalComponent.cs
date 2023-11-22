using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalComponent : MoverComponent
{
    
    protected override void PostInit() {
        base.PostInit();

        Entity.SetName("Deer");

    }
}
