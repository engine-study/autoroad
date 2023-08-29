using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using mud.Unity;
using IWorld.ContractDefinition;
using Nethereum.Contracts;
using Cysharp.Threading.Tasks;

public enum StateType {Idle, Dead, Mining, Shoveling, Stick, Fishing, Walking, Buy, Plant, Push, Chop, Teleport, Melee}
public class ActionsMUD : MonoBehaviour
{
    [EnumNamedArray( typeof(StateType) )]
    [SerializeField] List<Equipment> baseEquipment;

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
    float distanceToPlayer = 999f;


    void Awake() {

        player = GetComponentInParent<PlayerMUD>();
        player.OnPostInit += Init;
        
    }

    public void ToggleEquipment(bool toggle, Equipment newEquipment) {

        if(toggle && !equipment.Contains(newEquipment)) {
            newEquipment.SetActor(player.Actor);
            equipment.Add(newEquipment);
        } else {
            equipment.Remove(newEquipment);
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

    void Init() {

        //add equipment
        foreach(Equipment e in baseEquipment) {
            if (e == null) { continue; }
            ToggleEquipment(true, e);
        }

        // gameObject.SetActive(player.IsLocalPlayer);


        if (!player.IsLocalPlayer)
            return;

        gameObject.transform.parent = null;
        gameObject.transform.position = Vector3.zero;

        position = player.Position;

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
    }

    public void StateToAction(StateType newState, Vector3 position) {

        //turn player to face position
        ((ControllerMUD)player.Controller).SetLookRotation(position);
        
        //do some work to find the action here
        Equipment stateEquipment = baseEquipment[(int)newState];

        if(stateEquipment != null) {
            stateEquipment.UseState(true, player.Actor);
        }

    }
    
    public static UniTask<bool> DoAction(List<TxUpdate> updates, StateType newState, Vector3 newPos) {
        return TxManager.Send<ActionFunction>(updates, ActionsMUD.ActionTx(newState, newPos));
    }
    
    public static object[] ActionTx(StateType newState, Vector3 newPos) { return new object[] { System.Convert.ToByte((int)newState), System.Convert.ToInt32(newPos.x), System.Convert.ToInt32(newPos.z)}; }

}
