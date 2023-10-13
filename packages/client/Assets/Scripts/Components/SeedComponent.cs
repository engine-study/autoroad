using UnityEngine;
using DefaultNamespace;
using mud;
using mud;

public class SeedComponent : ValueComponent {

    public int Seeds { get { return seeds; } }
    public static int LocalCount;
    public static System.Action OnLocalUpdate;

    public int seeds;

    protected override float SetValue(IMudTable update) {return (int)((SeedsTable)update).value;}
    protected override StatType SetStat(IMudTable update) {return StatType.Seed;}

    protected override IMudTable GetTable() {return new SeedsTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        base.UpdateComponent(update,newInfo);

        SeedsTable table = update as SeedsTable;
        seeds = (int)table.value;

        if(Entity.Key == NetworkManager.LocalAddress) {
            LocalCount = seeds;
            OnLocalUpdate?.Invoke();
        }

    }


    protected override void OnDestroy() {
        base.OnDestroy();

        LocalCount = 0;
    }
}
