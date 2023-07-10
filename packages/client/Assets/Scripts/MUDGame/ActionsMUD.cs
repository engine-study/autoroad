using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsMUD : MonoBehaviour
{
    public SPAction ShovelAction;
    public SPAction MineAction;
    public SPAction PushAction;
    public SPAction WalkAction;

    public List<SPInteract> interacts;
    SPPlayer player;
    SPActor actor;

    void Awake()
    {
        player = GetComponent<SPPlayer>();
        player.OnPostInit += Init;

    }

    void Init()
    {
        if (!player.IsLocalPlayer)
            return;

        SPInteract interactable = new SPInteract();
        interactable.go = gameObject;
        
        interactable.action = ShovelAction;

        player.Reciever.ToggleInteractableManual(true, interactable);

        interacts.Add(interactable);

    }

    void Update()
    {

    }

}
