using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using IWorld.ContractDefinition;

public class DebugUI : SPWindowParent
{
    public void RespawnPlayer() {
        TxManager.SendDirect<DestroyPlayerAdminFunction>();
    }

    public void GiveCoins() {
        TxManager.SendDirect<AddCoinsAdminFunction>(100);
    }

    public void GiveXP() {
        TxManager.SendDirect<AddXPAdminFunction>(new System.Numerics.BigInteger(15));
    }

    public void GiveGems() {
        TxManager.SendDirect<AddGemXPFunction>(System.Convert.ToUInt32(5));
    }

}
