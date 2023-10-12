using System.Collections;
using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;

public class CoinComponent : ValueComponent {
    public int Coins { get { return coins; } }
    public static int LocalCoins;
    public static System.Action OnLocalUpdate;
    public PositionComponent position;

    [Header("Position")]
    [SerializeField] private int coins = 0;
    [SerializeField] private int lastCoins = 0;

    protected override IMudTable GetTable() {return new CoinageTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        base.UpdateComponent(update, newInfo);

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

    protected override float SetValue(IMudTable mudTable) {return (int)((CoinageTable)mudTable).value;}
    protected override string SetString(IMudTable mudTable) {return Value.ToString("000");}
    protected override StatType SetStat(IMudTable mudTable) {return StatType.RoadCoin;}

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

        int remainder = amount;

        int quarters = 0;
        int dimes = 0;
        int nickels = 0;

        if(remainder > 50) {
            quarters = amount / 25;
            remainder %= 25;
        }

        if(remainder > 20) {
            dimes = remainder / 10;
            remainder %= 10;
        }

        if(remainder > 10) {
            nickels = remainder / 5;
            remainder %= 5;
        }

        int pennies = remainder;

        for (int i = 0; i < quarters+dimes+nickels+pennies; i++) {

            string prefab = "";

            if(i < quarters) prefab = "Prefabs/CoinQuarter";
            else if(i < quarters+dimes) prefab = "Prefabs/CoinDime";
            else if(i < quarters+dimes+nickels) prefab = "Prefabs/CoinNickel";
            else prefab = "Prefabs/CoinPenny";

            SPResourceJuicy coin = SPResourceJuicy.SpawnResource(prefab, position.Target, position.Target.position + Vector3.up, Random.rotation);
            coin.SendResource();
            yield return new WaitForSeconds(.1f);
        }

    }



    protected override void OnDestroy() {
        base.OnDestroy();

        LocalCoins = 0;
    }
}
