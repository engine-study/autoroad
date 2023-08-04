using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using mud.Unity;
using IWorld.ContractDefinition;

public class ActionsMUD : MonoBehaviour
{
    public SPInteract ShovelAction;
    public SPInteract WalkAction;
    public SPInteract WalkBootAction;
    public SPInteract CarryAction;
    public SPInteract MeleeAction;
    public SPInteract PlantAction;
    public SPInteract FishAction;


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

        // distanceToPlayer = Vector3.Distance(newPos, position.Pos);
        
        //add the shovel action next to the player at empty spots
        bool shovelToggle = RoadConfigComponent.OnRoad((int)newPos.x, (int)newPos.z) && CursorMUD.Entity == null; //distanceToPlayer > .5f && distanceToPlayer <= 1f && 
        
        if(shovelToggle) {
            ShovelAction.transform.position = newPos;
        }

        player.Reciever.ToggleInteractableManual(shovelToggle, ShovelAction);
    }


    public void Melee() {
        
    }

    public void Fish() {
        
    }

    public void Shovel() {
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;

        Debug.Log("Shoveled: " + x + "," + y);
        
        //no way of optimistic spawning yet
        string entity = MUDHelper.Keccak256("Road", x,y);
        List<TxUpdate> updates = new List<TxUpdate>();
        updates.Add(TxManager.MakeOptimisticInsert<PositionComponent>(entity, x,y));
        updates.Add(TxManager.MakeOptimisticInsert<RoadComponent>(entity, 1, NetworkManager.LocalAddress));
        TxManager.Send<ShovelFunction>(updates, System.Convert.ToInt32(transform.position.x), System.Convert.ToInt32(transform.position.z));
    }

    public void Walk() {

    }

    public void WalkBoot() {

    }

    public void Plant() {

    }

}
