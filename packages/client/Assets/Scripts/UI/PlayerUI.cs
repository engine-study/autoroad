using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public SPActionUI actions;
    public Image actionWheel;

    public SPWindowPosition actionPosition;
    void Awake() {
        SPEvents.OnLocalPlayerSpawn += SetupPlayer;
    }

    void OnDestroy() {
        SPEvents.OnLocalPlayerSpawn -= SetupPlayer;
    }

    void SetupPlayer() {
        
        actionPosition.SetFollow(SPPlayer.LocalPlayer.Root);
        actions.Setup(SPPlayer.LocalPlayer.Actor);

        SPCamera.SetFollow(SPPlayer.LocalPlayer.Root);
    }

    
}
