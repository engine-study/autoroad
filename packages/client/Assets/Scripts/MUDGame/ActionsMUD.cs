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
    [EnumNamedArray( typeof(StateType) )]
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
            if(equipment[i] == null) continue;
            equipment[i].transform.position = newPos;
            player.Reciever.ToggleInteractableManual(equipment[i].CanUse(), equipment[i].Interact);
        }
    }

    public void StateToAction(StateType newState, Vector3 position) {

        //turn player to face position
        ((ControllerMUD)player.Controller).SetLookRotation(position);
        
        //do some work to find the action here
        Equipment stateEquipment = equipment[(int)newState];

        if(stateEquipment != null) {
            stateEquipment.UseState(true);
        }

    }

}
