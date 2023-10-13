using UnityEngine;
using mudworld;
using mud;
using mud;

public class GemComponent : ValueComponent {
    public int Gems { get { return gems; } }
    public static int LocalGems;
    public static System.Action OnLocalUpdate;


    [Header("Position")]
    [SerializeField] private int gems = 0;


    protected override float SetValue(IMudTable update) {return (int)((GemTable)update).value;}
    protected override StatType SetStat(IMudTable update) {return StatType.Gem;}

    protected override IMudTable GetTable() {return new GemTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        base.UpdateComponent(update,newInfo);

        GemTable table = update as GemTable;

        gems = (int)table.value;

        if(Entity.Key == NetworkManager.LocalAddress) {
            LocalGems = gems;
            OnLocalUpdate?.Invoke();
        }

    }


    protected override void OnDestroy() {
        base.OnDestroy();

        LocalGems = 0;
    }
}
