using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using mud.Unity;
using IWorld.ContractDefinition;
using Nethereum.Contracts;
using Cysharp.Threading.Tasks;

public enum StateType {Idle, Dead, Mining, Shoveling, Stick, Fishing, Walking, Buy, Plant}
public class ActionsMUD : MonoBehaviour
{
    [Header("Equipment")]
    [SerializeField] List<Equipment> equipment;

    // [Header("Interactable")]
    // public SPInteract ShovelAction;
    // public SPInteract WalkAction;
    // public SPInteract WalkBootAction;
    // public SPInteract CarryAction;
    // public SPInteract MeleeAction;
    // public SPInteract PlantAction;
    // public SPInteract FishAction;


    PlayerMUD player;
    PositionComponent position;
    SPActor actor;
    float distanceToPlayer = 999f;


    void Awake() {

        player = GetComponentInParent<PlayerMUD>();
        player.OnPostInit += Init;
        
    }

    public void ToggleEquipment(bool toggle, Equipment newEqipment) {

        if(toggle) {
            equipment.Add(newEqipment);
            newEqipment.SetSender(actor);
        } else {
            equipment.Remove(newEqipment);
        }

    }

    void OnDestroy() {

        if(player) {
            player.OnPostInit -= Init;
            (player.Controller as ControllerMUD).OnFinishedMove -= AddPositionActions;
        }

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

        
        //setup player actor references
        actor = player.Actor;

        gameObject.transform.parent = null;
        gameObject.transform.position = Vector3.zero;

        position = player.Position;

        //add equipment
        Equipment [] newEquipemnt = GetComponentsInChildren<Equipment>();
        foreach(Equipment e in newEquipemnt) {
            ToggleEquipment(true, e);
        }


        position.OnUpdated += AddPositionActions;
        (player.Controller as ControllerMUD).OnFinishedMove += AddPositionActions;
        CursorMUD.OnGridPosition += AddGridActions;

    }

    //everytime player updates position we should recalculate the actions
    void AddPositionActions() {
        AddGridActions(CursorMUD.GridPos);
    }

    //add actions base on what we encounter on the grid
    void AddGridActions(Vector3 newPos) {

        for(int i = 0; i < equipment.Count; i++) {
            equipment[i].transform.position = newPos;
            player.Reciever.ToggleInteractableManual(equipment[i].CanUse(), equipment[i].Interact);
        }

        // distanceToPlayer = Vector3.Distance(newPos, position.Pos);
        // bool inReach = distanceToPlayer <= 1f;

        // //add the shovel action next to the player at empty spots
        // bool shovelToggle = inReach && RoadConfigComponent.OnRoad((int)newPos.x, (int)newPos.z) && CursorMUD.Entity == null; //distanceToPlayer > .5f && distanceToPlayer <= 1f && 
        // if(shovelToggle) {
        //     ShovelAction.transform.position = newPos;
        // }

        // player.Reciever.ToggleInteractableManual(shovelToggle, ShovelAction);
    
        // bool plantToggle = inReach && SeedsComponent.LocalCount > 0 && BoundsComponent.OnBounds((int)newPos.x, (int)newPos.z) && CursorMUD.Entity == null; //distanceToPlayer > .5f && distanceToPlayer <= 1f && 
        // if(plantToggle) {
        //     PlantAction.transform.position = newPos;
        // }

        // player.Reciever.ToggleInteractableManual(plantToggle, PlantAction);

    }

}
