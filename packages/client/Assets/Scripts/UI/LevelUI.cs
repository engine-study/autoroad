using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using System;
using TMPro;

public class LevelUI : SPWindowMUDComponent
{
    [Header("Level")]
    public TextMeshProUGUI levelButton;

    public override Type ComponentType() {return typeof(XPComponent);}



    public override void UpdateComponent() {
        base.UpdateComponent();

        levelButton.text = (component as XPComponent).Level.ToString();

    }

}
