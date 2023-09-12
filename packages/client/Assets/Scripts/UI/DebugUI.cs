using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using IWorld.ContractDefinition;

public class DebugUI : SPWindowParent
{
    public void RespawnPlayer() {
        TxManager.SendSafe<DestroyPlayerAdminFunction>();
    }

    public void GiveCoins() {
        TxManager.SendSafe<AddCoinsAdminFunction>(100);
    }

    public void GiveXP() {
        TxManager.SendSafe<AddXPAdminFunction>(new System.Numerics.BigInteger(15));
    }

    public void GiveGems() {
        TxManager.SendSafe<AddGemXPFunction>(System.Convert.ToUInt32(5));
    }

}
