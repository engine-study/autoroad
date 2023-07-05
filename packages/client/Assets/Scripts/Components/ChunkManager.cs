using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;


using UniRx;
using ObservableExtensions = UniRx.ObservableExtensions;

public class ChunkManager : MUDTableManager
{
    protected override void Subscribe(mud.Unity.NetworkManager nm)
    {

        var InsertSub = ObservableExtensions.Subscribe(ChunkTable.OnRecordInsert().ObserveOnMainThread(),
                OnInsertRecord);
        _disposers.Add(InsertSub);

        var UpdateSub = ObservableExtensions.Subscribe(ChunkTable.OnRecordUpdate().ObserveOnMainThread(),
                OnUpdateRecord);
        _disposers.Add(UpdateSub);

        //var MergedUpdate = ObservableExtensions.Subscribe(PositionTable.OnRecordInsert().Merge(PositionTable.OnRecordUpdate()).ObserveOnMainThread(),OnChainPositionUpdate);
    }

    // public static void GetPosition(Position component)
    // {

    //     Position positionComp = component as Position;

    //     var table = PositionTable.GetTableValue(component.Entity.Key);

    //     if (table == null)
    //     {
    //         Debug.LogError("No position on " + component.Entity.gameObject, component.Entity.gameObject);
    //         return;
    //     }

    //     positionComp.UpdateComponent(table, TableEvent.Manual);
    // }


    protected override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate)
    {
        ChunkTableUpdate update = tableUpdate as ChunkTableUpdate;

        var currentValue = update.TypedValue.Item1;
        if (currentValue == null)
        {
            Debug.LogError("No currentValue");
            return null;
        }

        return currentValue;
    }
}
