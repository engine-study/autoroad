using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using mud.Unity;
using IWorld.ContractDefinition;
using Nethereum.Contracts;
using Cysharp.Threading.Tasks;

public enum ActionName {Idle, Dead, Mining, Shoveling, Stick, Fishing, Walking, Buy, Plant, Push, Chop, Teleport, Melee, Hop}
public class ActionsMUD : MonoBehaviour
{
    [EnumNamedArray( typeof(ActionName) )]
    [SerializeField] List<Equipment> baseEquipment;

    [Header("Equipment")]
    [SerializeField] List<Equipment> equipment;

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
        if(!player.IsLocalPlayer) {
            foreach(Equipment e in baseEquipment) {
                if (e == null) { continue; }
                e.gameObject.SetActive(false);
            }
        }

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

    
    public void ActionToActionProp(ActionName newState, Vector3 position) {

        //turn player to face position
        ((ControllerMUD)player.Controller).SetLookRotation(position);

        if (baseEquipment.Count - 1 < (int)newState) { Debug.LogError(newState.ToString(), this); }

        //do some work to find the action here
        Equipment e = baseEquipment[(int)newState];

        if (e == null){
            return;
        }

        e.UseState(true);
    
        if (action != null) { StopCoroutine(action); }
        action = StartCoroutine(ActionCoroutine(e));

    }

    Coroutine action;
    IEnumerator ActionCoroutine(Equipment equipment) {
        yield return new WaitForSeconds(2f);
        equipment.UseState(false);

    }

    
    public static UniTask<bool> ActionTx(Vector3 newPos, MUDEntity entity, ActionName targetAction, ActionName txAction, bool sendTX = true) {

        // AnimationComponent anim = MUDWorld.FindOrMakeComponent<AnimationComponent>(entity.Key);
        ActionComponent actionComponent = MUDWorld.FindOrMakeComponent<ActionComponent>(entity.Key);
        PositionComponent posComponent = MUDWorld.FindOrMakeComponent<PositionComponent>(entity.Key);

        List<TxUpdate> updates = new List<TxUpdate>();
        updates.Add(TxManager.MakeOptimistic(actionComponent, targetAction, (int)newPos.x, (int)newPos.z)); 
        updates.Add(TxManager.MakeOptimistic(posComponent, PositionComponent.PositionToOptimistic(newPos)));

        if (sendTX) { 
            //set our local state to the Tx
            updates.Add(TxManager.MakeOptimistic(ActionComponent.LocalState, txAction, (int)newPos.x, (int)newPos.z)); 

            //send Tx
            return ActionsMUD.DoAction(updates, txAction, newPos); 
        } else {
            return new UniTask<bool>();
        }
    }
    
    public static UniTask<bool> DoAction(List<TxUpdate> updates, ActionName newState, Vector3 newPos) {
        return TxManager.Send<ActionFunction>(updates, ActionsMUD.ActionTx(newState, newPos));
    }

    public static object[] ActionTx(ActionName newState, Vector3 newPos) { return new object[] { System.Convert.ToByte((int)newState), System.Convert.ToInt32(newPos.x), System.Convert.ToInt32(newPos.z)}; }

}
