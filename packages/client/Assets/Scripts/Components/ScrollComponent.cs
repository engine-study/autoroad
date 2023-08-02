using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;

public class ScrollComponent : MUDComponent {

    public int Scrolls { get { return scrolls; } }
    public static int LocalScrolls;
    public static System.Action OnLocalUpdate;

    public int scrolls;

    protected override IMudTable GetTable() {return new ScrollTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        ScrollTable table = update as ScrollTable;
        scrolls = (int)table.value;

        if(Entity.Key == NetworkManager.LocalAddress) {
            LocalScrolls = scrolls;
            OnLocalUpdate?.Invoke();
        }

    }


    protected override void OnDestroy() {
        base.OnDestroy();

        LocalScrolls = 0;
    }
}
