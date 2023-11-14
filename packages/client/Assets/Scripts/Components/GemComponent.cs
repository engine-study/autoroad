using UnityEngine;
using mudworld;
using mud;

public class GemComponent : ValueComponent {
    public int Gems { get { return gems; } }
    public static int LocalGems;
    public static System.Action OnLocalUpdate;


    [Header("Position")]
    [SerializeField] private int gems = 0;


    protected override float SetValue(MUDTable update) {return (int)((GemTable)update).Value;}
    protected override StatType SetStat(MUDTable update) {return StatType.Gem;}

    protected override MUDTable GetTable() {return new GemTable();}
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {
        base.UpdateComponent(update,newInfo);

        GemTable table = update as GemTable;

        gems = (int)table.Value;

        if(Entity.Key == NetworkManager.LocalKey) {
            LocalGems = gems;
            OnLocalUpdate?.Invoke();
        }

    }


    protected override void OnDestroy() {
        base.OnDestroy();

        LocalGems = 0;
    }
}
