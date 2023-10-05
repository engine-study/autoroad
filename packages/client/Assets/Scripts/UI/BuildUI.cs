using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
public class BuildUI : PhaseUI
{
    [Header("Build")]
    public GameObject worldUI;
    public StatUI stats;
    public InfoUI info;
    public MUDEntity targetEntity;


    public override void ToggleWindow(bool toggle)
    {
        base.ToggleWindow(toggle);

        worldUI.SetActive(toggle);
        SPCamera.SetFOVGlobal(5f);
    }

    public override void UpdatePhase() {
        targetEntity.transform.position = CursorMUD.GridPos;
    }
}
