using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using IWorld.ContractDefinition;

public class DebugUI : SPWindowParent
{
    public void RespawnPlayer() {
        TxManager.Send<DestroyPlayerAdminFunction>();
    }

    public void GiveCoins() {
        TxManager.Send<AddCoinsAdminFunction>(25);
    }

}
