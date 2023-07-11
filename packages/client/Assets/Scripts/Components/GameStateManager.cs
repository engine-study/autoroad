using UnityEngine;

using mud.Client;
using UniRx;
using ObservableExtensions = UniRx.ObservableExtensions;

using DefaultNamespace;

public class GameStateManager : MUDTableManager {


    protected override void Subscribe(mud.Unity.NetworkManager nm)
    {

        var InsertSub = ObservableExtensions.Subscribe(GameStateTable.OnRecordInsert().ObserveOnMainThread(),
                OnInsertRecord);
        _disposers.Add(InsertSub);

        var UpdateSub = ObservableExtensions.Subscribe(GameStateTable.OnRecordUpdate().ObserveOnMainThread(),
                OnUpdateRecord);
        _disposers.Add(UpdateSub);

        //var MergedUpdate = ObservableExtensions.Subscribe(PositionTable.OnRecordInsert().Merge(PositionTable.OnRecordUpdate()).ObserveOnMainThread(),OnChainPositionUpdate);
    }

    protected override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
    {
        GameStateTableUpdate update = tableUpdate as GameStateTableUpdate;

        var currentValue = update.TypedValue.Item1;
        if (currentValue == null)
        {
            Debug.LogError("No currentValue");
            return null;
        }

        return currentValue;
    }


}