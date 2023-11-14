using System.Collections;
using System.Collections.Generic;
using mud;
using mudworld;
using UnityEngine;

public class LeaderboardUI : SPWindowParent
{
    [SerializeField] List<SPButton> names;
    [SerializeField] List<SPButton> levels;
    [SerializeField] List<SPButton> xps;
    [SerializeField] List<LeaderboardSlot> leaders;
    [SerializeField] Dictionary<string, LeaderboardSlot> leaderDict;

    
    public class LeaderboardSlot {
        public string Key;
        public string Name;
        public int xp;
        public NPCTable npc;
    }

    public override void Init() {
        if(HasInit) {return;}
        base.Init();

        leaders = new List<LeaderboardSlot>();
        leaderDict = new Dictionary<string, LeaderboardSlot>();

        // NetworkManager.OnInitialized += LoadXP;
        TableManager.OnTableRegistered += LoadXPTable;
    }

    public override void ToggleWindow(bool toggle) {
        base.ToggleWindow(toggle);

        if(toggle) {
            UpdateList();
        }

    }

    public void HoverLeader(int number) {
        PositionSync pos = MUDWorld.FindEntity(leaders[number].Key)?.GetRootComponent<PositionSync>();
        if(pos) {
            WorldScroll.Instance.SetToObject(pos.Target, true);
        }
        
    }

    void LoadXPTable(TableManager newTable) {
        if(newTable.Prefab.GetType() == typeof(XPComponent)) {
            newTable.OnComponentSpawned += LoadXPComponent;
            newTable.OnComponentUpdated += UpdateXP;
            TableManager.OnTableRegistered -= LoadXPTable;
        }
    }

    void LoadXPComponent(MUDComponent c) {

        XPComponent xp = c as XPComponent;
        LeaderboardSlot ls = new LeaderboardSlot();

        ls.Key = xp.Entity.Key;
        ls.xp = (int)xp.Value;
        ls.npc = MUDTable.GetTable<NPCTable>(ls.Key);

        if(ls.npc == null) {Debug.LogError("Can't find NPC", this); return;}

        NPCType npc = (NPCType)ls.npc.Value;
        
        if(npc == NPCType.Player) {

            NameTable name = MUDTable.GetTable<NameTable>(ls.Key);
            if(name == null) {
                return;
            }

            string nameString = NameUI.TableToName((int)name.First,(int)name.Middle,(int)name.Last);
            ls.Name = nameString;
        } else if(npc != NPCType.None) {
            ls.Name = npc.ToString();
        } else {
            Debug.LogError("Bad", this);
            return;
        }

        leaders.Add(ls);
        leaderDict.Add(ls.Key, ls);
    }

    void UpdateXP(MUDComponent c) {
        XPComponent xp = c as XPComponent;
        leaderDict.TryGetValue(c.Entity.Key, out LeaderboardSlot ls);
        if(ls == null) return;
        
        ls.xp = (int)xp.Value;

        if(!gameObject.activeInHierarchy) return;
        
        UpdateList();
    }

    void UpdateList() {
        
        //sort
        leaders.Sort((a, b) => b.xp.CompareTo(a.xp));

        //show text
        for(int i = 0; i < names.Count; i++) {
            if(i < leaders.Count) {
                names[i].ToggleWindowOpen();
                int index = i+1;
                names[i].UpdateField(leaders[i].Name);
                // xps[i].UpdateField(FormatNumber(leaders[i].xp));
                levels[i].UpdateField(XPComponent.XPToLevel(leaders[i].xp).ToString());
            } else {
                names[i].ToggleWindowClose();
            }
        }
        
    }

    void LoadXP() {

        // var allXP = new Query().In(XPTable.Table).In(XPTable.Table);
        // var xpRecords = NetworkManager.Datastore.RunQuery(allXP);

        // foreach(RxRecord r in xpRecords) {

        // }

    }

    void UpdateXP() {

    }

}
