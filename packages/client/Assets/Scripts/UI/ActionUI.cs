using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUI : PhaseUI
{
    [Header("Action")]
    public GameObject worldUI;
    public StatUI stats;
    public InfoUI info;

    public override void ToggleWindow(bool toggle)
    {
        base.ToggleWindow(toggle);

        worldUI.SetActive(toggle);
        SPCamera.SetFOVGlobal(5f);

    }

}
