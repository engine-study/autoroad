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
    float distanceToPlayer = 999f;

    void Awake()
    {
        player = GetComponentInParent<SPPlayer>();
        player.OnPostInit += Init;
    }

    void OnDestroy() {

        player.OnPostInit -= Init;
        CursorMUD.OnGridPosition -= AddGridActions;
    }

    void Init()
    {
        gameObject.SetActive(player.IsLocalPlayer);

        if (!player.IsLocalPlayer)
            return;

        CursorMUD.OnGridPosition += AddGridActions;

    }

    //add actions base on what we encounter on the grid
    void AddGridActions(Vector3 newPos) {

        distanceToPlayer = Vector3.Distance(newPos, player.transform.position);
        
        //add the shovel action next to the player at empty spots
        bool shovelToggle = distanceToPlayer > .5f && distanceToPlayer <= 1f && BoundsComponent.InBounds((int)newPos.x, (int)newPos.z) && CursorMUD.Entity == null;
        if(shovelToggle) {
            ShovelAction.transform.position = newPos;
        }
        player.Reciever.ToggleInteractableManual(shovelToggle, ShovelAction);

      

    }

}
