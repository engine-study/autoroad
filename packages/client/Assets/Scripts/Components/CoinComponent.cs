using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;

public class CoinComponent : MUDComponent {
    public int Coins { get { return coins; } }
    public static int LocalCoins;
    public static System.Action OnLocalUpdate;


    [Header("Position")]
    [SerializeField] private int coins = 0;

    protected override IMudTable GetTable() {return new CoinageTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        CoinageTable table = update as CoinageTable;

        coins = (int)table.value;

        if(Entity.Key == NetworkManager.LocalAddress) {
            LocalCoins = coins;
            OnLocalUpdate?.Invoke();
        }

    }


    protected override void OnDestroy() {
        base.OnDestroy();

        LocalCoins = 0;
    }
}
