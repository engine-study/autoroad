using UnityEngine;
using mudworld;
using mud;
using mud;

public class SeedComponent : ValueComponent {

    public int Seeds { get { return seeds; } }
    public static int LocalCount;
    public static System.Action OnLocalUpdate;

    public int seeds;

    protected override float SetValue(MUDTable update) {return (int)((SeedsTable)update).Value;}
    protected override StatType SetStat(MUDTable update) {return StatType.Seed;}

    protected override MUDTable GetTable() {return new SeedsTable();}
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {
        base.UpdateComponent(update,newInfo);

        SeedsTable table = update as SeedsTable;
        seeds = (int)table.Value;

        if(Entity.Key == NetworkManager.LocalKey) {
            LocalCount = seeds;
            OnLocalUpdate?.Invoke();
        }

    }


    protected override void OnDestroy() {
        base.OnDestroy();

        LocalCount = 0;
    }
}
