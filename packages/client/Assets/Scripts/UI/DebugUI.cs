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
        TxManager.Send<AddCoinsAdminFunction>(100);
    }

    public void GiveXP() {
        TxManager.Send<AddXPAdminFunction>(new System.Numerics.BigInteger(15));
    }

    public void GiveGems() {
        TxManager.Send<AddGemXPFunction>(System.Convert.ToUInt32(5));
    }

}
