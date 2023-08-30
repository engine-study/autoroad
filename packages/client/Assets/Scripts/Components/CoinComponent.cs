using System.Collections;
using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;

public class CoinComponent : MUDComponent {
    public int Coins { get { return coins; } }
    public static int LocalCoins;
    public static System.Action OnLocalUpdate;
    public PositionComponent position;

    [Header("Position")]
    [SerializeField] private int coins = 0;
    [SerializeField] private int lastCoins = 0;

    protected override IMudTable GetTable() {return new CoinageTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        CoinageTable table = update as CoinageTable;
        coins = (int)table.value;

        if(Entity.Key == NetworkManager.LocalAddress) {
            LocalCoins = coins;
            OnLocalUpdate?.Invoke();
        }

        if(Loaded) {
            int coinDiff = coins - lastCoins;
            Spawn(coinDiff);
        }

        lastCoins = coins;

    }

    protected override void PostInit() {
        base.PostInit();
    }

    
    Coroutine spawnCoroutine = null;
    void Spawn(int amount) {

        if (position == null) position = Entity.GetMUDComponent<PositionComponent>();
        if (position == null) { Debug.LogError("No position component", this); return; }
        if (position.Target == null) { Debug.LogError("No position target", this); return; }
        spawnCoroutine = StartCoroutine(SpawnCoins(amount));
    }

    IEnumerator SpawnCoins(int amount) {

        for (int i = 0; i < amount; i++) {
            SPResourceJuicy coin = SPResourceJuicy.SpawnResource("Prefabs/Coin", position.Target, position.Target.position + Vector3.up, Random.rotation);
            coin.SendResource();
            yield return new WaitForSeconds(.1f);
        }

    }



    protected override void OnDestroy() {
        base.OnDestroy();

        LocalCoins = 0;
    }
}
