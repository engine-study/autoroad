using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostGameUI : PhaseUI
{
    public override void ToggleWindow(bool toggle)
    {
        base.ToggleWindow(toggle);

        SPCamera.SetFOVGlobal(10f);

    }
}
