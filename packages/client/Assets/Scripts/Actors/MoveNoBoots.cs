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

}