using UnityEngine;

using mud.Client;
using UniRx;
using ObservableExtensions = UniRx.ObservableExtensions;

using DefaultNamespace;

public class PositionManager : MUDTableManager {
    public override System.Type TableType() { return typeof(PositionTable); }
    protected override void Subscribe(mud.Unity.NetworkManager nm) {

        var InsertSub = ObservableExtensions.Subscribe(PositionTable.OnRecordInsert().ObserveOnMainThread(),
                OnInsertRecord);
        _disposers.Add(InsertSub);

        var UpdateSub = ObservableExtensions.Subscribe(PositionTable.OnRecordUpdate().ObserveOnMainThread(),
                OnUpdateRecord);
        _disposers.Add(UpdateSub);

        //var MergedUpdate = ObservableExtensions.Subscribe(PositionTable.OnRecordInsert().Merge(PositionTable.OnRecordUpdate()).ObserveOnMainThread(),OnChainPositionUpdate);
    }

    protected override IMudTable RecordUpdateToTable(RecordUpdate tableUpdate) {
        PositionTableUpdate update = tableUpdate as PositionTableUpdate;

        var currentValue = update.TypedValue.Item1;
        if (currentValue == null) {
            Debug.LogError("No currentValue");
            return null;
        }

        return currentValue;
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

}
