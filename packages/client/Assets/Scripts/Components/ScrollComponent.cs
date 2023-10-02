using UnityEngine;
using DefaultNamespace;
using mud;


public class ScrollComponent : MUDComponent {

    public int Scrolls { get { return scrolls; } }
    public static int LocalScrolls;
    public static System.Action OnLocalUpdate;

    // public AudioClip sfx_gotScroll;

    public int scrolls;

    protected override IMudTable GetTable() {return new ScrollTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        ScrollTable table = update as ScrollTable;
        scrolls = (int)table.value;

        if(Entity.Key == NetworkManager.LocalKey) {
            LocalScrolls = scrolls;
            OnLocalUpdate?.Invoke();
        }

    }


    protected override void OnDestroy() {
        base.OnDestroy();

        LocalScrolls = 0;
    }
}
