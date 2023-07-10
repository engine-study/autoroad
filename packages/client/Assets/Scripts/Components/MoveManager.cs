using UnityEngine;
using mud.Client;
using UniRx;
using ObservableExtensions = UniRx.ObservableExtensions;

using DefaultNamespace;

public class MoveManager : MUDTableManager
{
   
    protected override void Subscribe(mud.Unity.NetworkManager nm)
    {

        var InsertSub = ObservableExtensions.Subscribe(MoveTable.OnRecordInsert().ObserveOnMainThread(),
                OnInsertRecord);
        _disposers.Add(InsertSub);

        var UpdateSub = ObservableExtensions.Subscribe(MoveTable.OnRecordUpdate().ObserveOnMainThread(),
                OnUpdateRecord);
        _disposers.Add(UpdateSub);

    }

    protected override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
    {
        MoveTableUpdate update = tableUpdate as MoveTableUpdate;

        var currentValue = update.TypedValue.Item1;
        if (currentValue == null)
        {
            Debug.LogError("No currentValue");
            return null;
        }

        return currentValue;
    }
}
