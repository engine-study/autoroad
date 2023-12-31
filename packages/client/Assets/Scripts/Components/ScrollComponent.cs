using UnityEngine;
using mudworld;
using mud;

public class ScrollComponent : ValueComponent {

    public int Scrolls { get { return scrolls; } }
    public static int LocalScrolls;
    public static System.Action OnLocalUpdate;

    // public AudioClip sfx_gotScroll;

    public int scrolls;

    protected override float SetValue(MUDTable update) {return (int)((ScrollTable)update).Value;}
    protected override StatType SetStat(MUDTable update) {return StatType.Scroll;}
    protected override MUDTable GetTable() {return new ScrollTable();}

    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {
        base.UpdateComponent(update,newInfo);

        ScrollTable table = update as ScrollTable;
        scrolls = (int)table.Value;

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
