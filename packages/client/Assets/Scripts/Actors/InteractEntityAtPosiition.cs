using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using System;

public class InteractEntityAtPosition : SPInteract
{
    [Header("Transaction")]
    public string functionName;

    public override void Interact(bool toggle, IActor newActor) {
        base.Interact(toggle, newActor);

        if(toggle) {
            // TxManager.MakeOptimistic
            // TxManager.Send(Type.GetType(functionName), 
        }
    }
}
