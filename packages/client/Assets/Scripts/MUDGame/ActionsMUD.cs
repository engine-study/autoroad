using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsMUD : MonoBehaviour
{
    public SPInteract ShovelAction;
    public SPInteract MineAction;
    public SPInteract PushAction;
    public SPInteract WalkAction;


    PlayerMUD player;
    PositionComponent position;
    SPActor actor;
    float distanceToPlayer = 999f;
    Vector3 cursorPosition;
    Vector3 playerPosition;


    void Awake()
    {
        player = GetComponentInParent<PlayerMUD>();
        player.OnPostInit += Init;
    }

    void OnDestroy() {

        player.OnPostInit -= Init;
        CursorMUD.OnGridPosition -= AddGridActions;

        if(position) {
            position.OnUpdated -= AddPositionActions;
        }
    }

    void Init()
    {
        gameObject.SetActive(player.IsLocalPlayer);

        if (!player.IsLocalPlayer)
            return;

        gameObject.transform.parent = null;
        gameObject.transform.position = Vector3.zero;

        position = player.Position;

        position.OnUpdated += AddPositionActions;
        CursorMUD.OnGridPosition += AddGridActions;

    }

    //everytime player updates position we should recalculate the actions
    void AddPositionActions() {
        AddGridActions(cursorPosition);
    }

    //add actions base on what we encounter on the grid
    void AddGridActions(Vector3 newPos) {

        distanceToPlayer = Vector3.Distance(newPos, position.Pos);
        
        //add the shovel action next to the player at empty spots
        bool shovelToggle = distanceToPlayer > .5f && distanceToPlayer <= 1f && BoundsComponent.InBounds((int)newPos.x, (int)newPos.z) && CursorMUD.Entity == null;
        if(shovelToggle) {
            ShovelAction.transform.position = newPos;
        }
        player.Reciever.ToggleInteractableManual(shovelToggle, ShovelAction);
    }

}
