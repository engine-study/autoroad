using UnityEngine;
using DefaultNamespace;
using mud;


public class SeedsComponent : MUDComponent {

    public int Count { get { return count; } }
    public static int LocalCount;
    public static System.Action OnLocalUpdate;

    public int count;

    protected override IMudTable GetTable() {return new SeedsTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        SeedsTable table = update as SeedsTable;
        count = (int)table.value;

        if(Entity.Key == NetworkManager.LocalKey) {
            LocalCount = count;
            OnLocalUpdate?.Invoke();
        }

    }


    protected override void OnDestroy() {
        base.OnDestroy();

        LocalCount = 0;
    }
}
