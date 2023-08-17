using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;

public class GemComponent : MUDComponent {
    public int Gems { get { return gems; } }
    public static int LocalGems;
    public static System.Action OnLocalUpdate;


    [Header("Position")]
    [SerializeField] private int gems = 0;

    protected override IMudTable GetTable() {return new GemTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        GemTable table = update as GemTable;

        gems = (int)table.value;

        if(Entity.Key == NetworkManager.LocalAddress) {
            LocalGems = gems;
            OnLocalUpdate?.Invoke();
        }

        if(Loaded) {
            MileComplete.AddGem(this);
        }
    }


    protected override void OnDestroy() {
        base.OnDestroy();

        LocalGems = 0;
    }
}
