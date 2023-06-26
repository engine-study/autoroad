using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : PhaseUI
{

    public override void ToggleWindow(bool toggle)
    {
        base.ToggleWindow(toggle);

        SPCamera.SetFOVGlobal(15f);
        // SPUIBase.Camera.fo

    }
}
