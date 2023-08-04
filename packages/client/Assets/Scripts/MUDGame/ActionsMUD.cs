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

        distanceToPlayer = Vector3.Distance(newPos, position.Pos);
        bool inReach = distanceToPlayer <= 1f;

        //add the shovel action next to the player at empty spots
        bool shovelToggle = inReach && RoadConfigComponent.OnRoad((int)newPos.x, (int)newPos.z) && CursorMUD.Entity == null; //distanceToPlayer > .5f && distanceToPlayer <= 1f && 
        if(shovelToggle) {
            ShovelAction.transform.position = newPos;
        }

        player.Reciever.ToggleInteractableManual(shovelToggle, ShovelAction);
    
        bool plantToggle = inReach && SeedsComponent.LocalCount > 0 && BoundsComponent.OnBounds((int)newPos.x, (int)newPos.z) && CursorMUD.Entity == null; //distanceToPlayer > .5f && distanceToPlayer <= 1f && 
        if(plantToggle) {
            PlantAction.transform.position = newPos;
        }

        player.Reciever.ToggleInteractableManual(plantToggle, PlantAction);

    }


    public void Melee() {
        
    }

    public void Fish() {
        
    }

    public void Shovel() {

        int x = (int)ShovelAction.transform.position.x;
        int y = (int)ShovelAction.transform.position.z;

        Debug.Log("Shoveled: " + x + "," + y);
        
        string entity = MUDHelper.Keccak256("Road", x,y);
        List<TxUpdate> updates = new List<TxUpdate>();
        updates.Add(TxManager.MakeOptimisticInsert<PositionComponent>(entity, x,y));
        updates.Add(TxManager.MakeOptimisticInsert<RoadComponent>(entity, 1, NetworkManager.LocalAddress));
        TxManager.Send<ShovelFunction>(updates, System.Convert.ToInt32(x), System.Convert.ToInt32(y));

    }

    public void Walk() {

    }

    public void WalkBoot() {

    }

    public void Plant() {
        
        int x = (int)PlantAction.transform.position.x;
        int y = (int)PlantAction.transform.position.z;

        // string entity = MUDHelper.Keccak256("Terrain", x,y);
        // List<TxUpdate> updates = new List<TxUpdate>();
        // updates.Add(TxManager.MakeOptimisticInsert<PositionComponent>(entity, x,y));
        // updates.Add(TxManager.MakeOptimisticInsert<TreeComponent>(entity, 1, NetworkManager.LocalAddress));

        TxManager.Send<PlantFunction>(System.Convert.ToInt32(x), System.Convert.ToInt32(y));
    }

}
