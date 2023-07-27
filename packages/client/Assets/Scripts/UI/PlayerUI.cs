using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
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

    }

    
}
