using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mud;
using Cysharp.Threading.Tasks;
using IWorld.ContractDefinition;
public class MoveNoBoots : Equipment
{
    
    public override bool IsInteractable() {
        return base.IsInteractable();
    }

    
    public override async UniTask<bool> SendTx() {
        
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;
        int distance = Mathf.RoundToInt(Vector3.Distance(transform.position, PlayerMUD.MUDPlayer.Position.Pos));

        return await TxManager.SendDirect<WalkFunction>(System.Convert.ToInt32(x), System.Convert.ToInt32(y), System.Convert.ToInt32(distance)); 

    }

}