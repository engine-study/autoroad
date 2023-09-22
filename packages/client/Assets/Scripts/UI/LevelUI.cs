using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using System;

public class LevelUI : SPWindowMUDComponent
{
    [Header("Level")]
    public SPButton levelButton;

    public override Type ComponentType() {return typeof(XPComponent);}



    public override void UpdateComponent() {
        base.UpdateComponent();

        levelButton.UpdateField((component as XPComponent).Level.ToString());

    }

}
