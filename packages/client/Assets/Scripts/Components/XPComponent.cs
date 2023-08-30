using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;

public class XPComponent : MUDComponent {

    public int Count { get { return xp; } }
    public static int LocalXP;
    public static int LocalLevel;
    public static System.Action OnXPUpdate;
    public static System.Action OnLevelUp;

    [SerializeField] int xp;
    [SerializeField] int level = -1;
    [SerializeField] int lastLevel = -1;

    protected override void Awake() {
        base.Awake();

        xp = -1;
        level = -1;
    }

    protected override IMudTable GetTable() {return new XPTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        XPTable table = update as XPTable;

        xp = (int)table.value;
        level = XPToLevel(xp);

        if(lastLevel != -1 && level != lastLevel) {
            OnLevelUp?.Invoke();
        }

        if(Entity.Key == NetworkManager.LocalAddress) {
            LocalXP = xp;
            LocalLevel = level;
            OnXPUpdate?.Invoke();
        }

        lastLevel = level;

    }

    public static int XPToLevel(int xp) {
        return Mathf.FloorToInt(60f * Mathf.Log((float)xp + 565f, 10f) - 164f);
    }

    public static int LevelToXP(int level) {
        return Mathf.RoundToInt((Mathf.Pow(10, (level+164) / 60f) - 565f));
        //Math.Pow(10, (x + 164) / 60) - 565
        
    }

    public static float XPToLevelProgress(int xp) {
        float currentLevel = XPToLevel(xp);
        float currentLevelXP = LevelToXP((int)currentLevel);
        float nextLevelXP = LevelToXP((int)currentLevel + 1);
        return (xp-currentLevelXP) / (nextLevelXP-currentLevelXP);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        LocalXP = 0;
    }


    //⁅60log(𝑥+540)−164⁆
    // If you want a logarithmic levelling formula that would simply be:

    // Level = Math.max( Math.floor( constA * Math.log( XP + constC ) + constB ), 1 )
    // For ~10 million XP at level 100 you should choose something like constA = 8.7, constB = -40 and constC = 111.

    // If the level gap rises too fast for your taste increase constA, decrease constB if you want the inital level gap to be higher, and finally set constC ~= exp((1-constB)/constA), in order to properly start at level 1.

    // Note that the appropriateness of any levelling formula depend completely on how fast players can gain XP at any given level.

    // See also: Algorithm for dynamically calculating a level based on experience points?
}
