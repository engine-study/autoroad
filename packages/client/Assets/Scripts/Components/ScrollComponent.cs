using UnityEngine;
using DefaultNamespace;
using mud;
using mud;

public class ScrollComponent : ValueComponent {

    public int Scrolls { get { return scrolls; } }
    public static int LocalScrolls;
    public static System.Action OnLocalUpdate;

    // public AudioClip sfx_gotScroll;

    public int scrolls;

    protected override float SetValue(IMudTable update) {return (int)((ScrollTable)update).value;}
    protected override StatType SetStat(IMudTable update) {return StatType.Scroll;}

    protected override IMudTable GetTable() {return new ScrollTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        base.UpdateComponent(update,newInfo);

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
