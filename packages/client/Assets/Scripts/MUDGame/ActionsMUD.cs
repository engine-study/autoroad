using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;
using System;

public enum ActionName {None, Idle, Dead, Mining, Shoveling, Stick, Fishing, Walking, Buy, Plant, Push, Chop, Teleport, Melee, Hop, Spawn, Bow, Swap}
public class ActionsMUD : MonoBehaviour
{
    public static ActionsMUD LocalActions;
    public Action OnInit;
    public Dictionary<string, Equipment> Equipment {get{return equipmentDict;}}
    public List<Equipment> EquipmentList {get{return equipment;}}

    [Header("Equipment")]
    [EnumNamedArray( typeof(ActionName) )]
    [SerializeField] List<Equipment> baseEquipment;
    [SerializeField] List<Equipment> organizedEquipment;
    [SerializeField] List<Equipment> local;

    [Header("Debug")]
    public bool hasInit;
    [SerializeField] PlayerMUD player;
    [SerializeField] PositionComponent position;
    [SerializeField] List<Equipment> equipment;
    [SerializeField] Dictionary<string, Equipment> equipmentDict;


    void Awake() {

        equipment = new List<Equipment>();
        equipmentDict = new Dictionary<string, Equipment>();

        player = GetComponentInParent<PlayerMUD>();

        if(player.HasLoaded) Init();
        else player.OnPostInit += Init;
        
    }

    void OnDestroy() {

        if(player) {
            player.OnPostInit -= Init;
            (player.Controller as ControllerMUD).OnFinishedMove -= UpdateActions;
        }
        CursorMUD.OnGridPosition -= GiveActionsAt;
        if(position) { position.OnUpdated -= UpdateActions;}
    }

    void Init() {

        hasInit = true;

        //exit now if we aren't a localplayer, disable our actions
        if (!player.IsLocalPlayer) {gameObject.SetActive(false); return;}

        //add equipment
        foreach(Equipment e in organizedEquipment) {
            if (e == null) { continue; }
            ToggleEquipment(true, e);
        }

        LocalActions = this;

        gameObject.transform.parent = null;
        gameObject.transform.position = Vector3.zero;

        position = player.Position;

        position.OnUpdated += UpdateActions;
        (player.Controller as ControllerMUD).OnFinishedMove += UpdateActions;
        CursorMUD.OnGridPosition += GiveActionsAt;

        OnInit?.Invoke();

    }

    public void ToggleEquipment(bool toggle, Equipment newEquipment) {

        if(newEquipment == null) {Debug.LogError("null", this); return;}

        if(toggle && !equipment.Contains(newEquipment)) {
            equipment.Add(newEquipment);
            equipmentDict.Add(newEquipment.name, newEquipment);
        } else {
            equipment.Remove(newEquipment);
            equipmentDict.Remove(newEquipment.name);
        }

    }


    //everytime player updates position we should recalculate the actions
    void UpdateActions() {
        GiveActionsAt(CursorMUD.GridPos);
    }

    //add actions base on what we encounter on the grid
    void GiveActionsAt(Vector3 newPos) {
        for(int i = 0; i < equipment.Count; i++) {
            equipment[i].transform.position = newPos;
            equipment[i].ToggleActor(true, player.Actor);
            player.Reciever.ToggleInteractableManual(equipment[i].IsInteractable(), equipment[i]);
        }

        ToolUI.Instance?.UpdateAllActions();
    }


    public void ActionToActionProp(ActionName newState, Vector3 position) {

        //turn player to face position
        player.AnimationMUD.Look.SetLookRotation(position);

        if ((int)newState >= baseEquipment.Count) { Debug.LogError(newState.ToString(), this); return; }

        //do some work to find the action here
        Equipment e = baseEquipment[(int)newState];

        if (e == null) { return;}

        e.UseState(true);

    }

    public static TxUpdate PositionOptimistic(MUDEntity entity, Vector3 atPos) {
        PositionComponent posComponent = MUDWorld.FindOrMakeComponent<PositionComponent>(entity.Key);
        return TxManager.MakeOptimistic(posComponent, PositionComponent.PositionToOptimistic(atPos));
    }

    public static TxUpdate ActionOptimistic(MUDEntity entity, ActionName action, Vector3 atPos) {
        ActionComponent actionComponent = MUDWorld.FindOrMakeComponent<ActionComponent>(entity.Key);
        return TxManager.MakeOptimistic(actionComponent, action, (int)atPos.x, (int)atPos.z, "0"); 
    }

    
    public static UniTask<bool> ActionTx(MUDEntity entity, ActionName action, Vector3 atPos, List<TxUpdate> updates = null) {

        //add the senders optimistic update
        if (updates == null) { updates = new List<TxUpdate>(); }
        updates.Add(ActionOptimistic(entity, action, atPos));

        return TxManager.Send<ActionFunction>(updates, ActionsMUD.ActionParameters(action, atPos)); 

    }

    public static object[] ActionParameters(ActionName newState, Vector3 newPos) { return new object[] { System.Convert.ToByte((int)newState), System.Convert.ToInt32(newPos.x), System.Convert.ToInt32(newPos.z)}; }

}
