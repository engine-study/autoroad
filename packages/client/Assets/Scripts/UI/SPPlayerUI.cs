using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPlayerUI : MonoBehaviour
{
    public SPActionUI actions;

    void Awake() {
        SPEvents.OnLocalPlayerSpawn += SetupPlayer;
    }

    void OnDestroy() {
        SPEvents.OnLocalPlayerSpawn -= SetupPlayer;
    }

    void SetupPlayer() {
        actions.Setup(SPPlayer.LocalPlayer.Actor);
        SPCamera.SetFollow(SPPlayer.LocalPlayer.Root);
    }
}
