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

    protected override IMudTable GetTable() {return new CoinageTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        CoinageTable table = update as CoinageTable;

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
