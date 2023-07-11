using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsMUD : MonoBehaviour
{
    public SPInteract ShovelAction;
    public SPInteract MineAction;
    public SPInteract PushAction;
    public SPInteract WalkAction;


    SPPlayer player;
    SPActor actor;

    void Awake()
    {
        player = GetComponentInParent<SPPlayer>();
        player.OnPostInit += Init;

    }

    void Init()
    {
        if (!player.IsLocalPlayer)
            return;

        player.Reciever.ToggleInteractableManual(true, ShovelAction);


    }

}
