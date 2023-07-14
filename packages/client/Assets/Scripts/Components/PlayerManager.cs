using DefaultNamespace;
using IWorld.ContractDefinition;
using mud.Unity;
using UniRx;
using UnityEngine;
using System.Collections;
using ObservableExtensions = UniRx.ObservableExtensions;
using System.Threading.Tasks;
using mud.Client;

public class PlayerManager : MUDTableManager {

    public override System.Type TableType() { return typeof(PlayerTable); }
    protected override void Subscribe(NetworkManager nm) {
        var SpawnSubscription = PlayerTable.OnRecordInsert().ObserveOnMainThread().Subscribe(OnInsertRecord);
        _disposers.Add(SpawnSubscription);

        var UpdateSubscription = ObservableExtensions.Subscribe(PlayerTable.OnRecordUpdate().ObserveOnMainThread(),
                OnUpdateRecord);
        _disposers.Add(UpdateSubscription);
    }

    protected override async void InitTable(NetworkManager nm) {

        var addressKey = net.addressKey;
        var currentPlayer = IMudTable.GetTable<PlayerTable>(addressKey);

        if (currentPlayer == null) {
            // spawn the player
            Debug.Log("Spawning TX...");
            await TxManager.Send<SpawnFunction>();

        }

        base.InitTable(nm);

    }

    protected override mud.Client.IMudTable RecordUpdateToTable(mud.Client.RecordUpdate tableUpdate) {
        return (tableUpdate as PlayerTableUpdate).TypedValue.Item1;
    }

}
