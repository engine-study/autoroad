using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mud;
using IWorld.ContractDefinition;
using Nethereum.Contracts;
using Cysharp.Threading.Tasks;

public enum ActionName {None, Idle, Dead, Mining, Shoveling, Stick, Fishing, Walking, Buy, Plant, Push, Chop, Teleport, Melee, Hop, Spawn, Bow}
public class ActionsMUD : MonoBehaviour
{
    public static ActionsMUD LocalActions;

    [EnumNamedArray( typeof(ActionName) )]

    [Header("Equipment")]
    [SerializeField] List<Equipment> baseEquipment;
    [SerializeField] List<Equipment> local;

    [Header("Debug")]
    [SerializeField] bool hasInit;
    [SerializeField] PlayerMUD player;
    [SerializeField] PositionComponent position;
    [SerializeField] List<Equipment> equipment;



    void Awake() {

        player = GetComponentInParent<PlayerMUD>();

        if(player.HasLoaded) Init();
        else player.OnPostInit += Init;
        
    }

    public void ToggleEquipment(bool toggle, Equipment newEquipment) {

        if(toggle && !equipment.Contains(newEquipment)) {
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

        hasInit = true;

        //exit now if we aren't a localplayer, disable our actions
        if (!player.IsLocalPlayer) {gameObject.SetActive(false); return;}

        //add equipment
        foreach(Equipment e in baseEquipment) {
            if (e == null) { continue; }
            ToggleEquipment(true, e);
        }

        LocalActions = this;

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
            equipment[i].ToggleActor(true, player.Actor);
            player.Reciever.ToggleInteractableManual(equipment[i].IsInteractable(), equipment[i]);
        }
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
