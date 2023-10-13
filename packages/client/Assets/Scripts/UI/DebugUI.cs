using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;

public class DebugUI : SPWindowParent
{
    public void RespawnPlayer() {
        TxManager.SendDirect<KillPlayerAdminFunction>(PositionComponent.PositionToTransaction(CursorMUD.GridPos));
    }

    public void MegaSummonMile() {
        TxManager.SendDirect<MegaSummonFunction>();
    }

    public void HelpSummonMile() {
        TxManager.SendDirect<HelpSummonFunction>();
    }

    public void GiveCoins() {
        TxManager.SendDirect<AddCoinsAdminFunction>(100);
    }

    public void GiveXP() {
        TxManager.SendDirect<AddXPAdminFunction>(new System.Numerics.BigInteger(15));
    }

    public void GiveGems() {
        TxManager.SendDirect<AddGemXPFunction>(System.Convert.ToInt32(5));
    }

    public void SpawnPuzzle() {
        TxManager.SendDirect<SpawnPuzzleAdminFunction>();
    }
    
    public void SpawnMile() {
        TxManager.SendDirect<SpawnMileAdminFunction>();
    }

    public void FinishMile() {
        TxManager.SendDirect<FinishMileAdminFunction>();
    }
}
