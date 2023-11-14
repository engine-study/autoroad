using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using Cysharp.Threading.Tasks;
using IWorld.ContractDefinition;
public class MoveNoBoots : Equipment
{

    public override void Engage(bool toggle, IActor newActor) {
        base.Engage(toggle, newActor);

        if(toggle) {
            // ((ControllerMUD)PlayerMUD.MUDPlayer.Controller).UpdateInput();
        }
    }

    public override bool IsInteractable() {
        return base.IsInteractable();
    }

    
    public override async UniTask<bool> SendTx() {
        

        Vector3 normalized = PlayerMUD.MUDPlayer.Position.Pos + (transform.position -  PlayerMUD.MUDPlayer.Position.Pos).normalized;
        int distance = Mathf.RoundToInt(Vector3.Distance(transform.position, PlayerMUD.MUDPlayer.Position.Pos));

        return await TxManager.SendDirect<WalkFunction>(System.Convert.ToInt32(Mathf.RoundToInt(normalized.x)), System.Convert.ToInt32(Mathf.RoundToInt(normalized.z)), System.Convert.ToInt32(distance)); 

    }

}