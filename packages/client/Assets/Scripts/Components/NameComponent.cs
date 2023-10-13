using UnityEngine;
using DefaultNamespace;
using mud;
using mud;
using System;

public class NameComponent : MUDComponent {

    public static Action OnLocalName;
    public string Name { get { return playerName; } }
    public static string LocalName {get{return localName;}}
    private static string localName = null;

    [Header("Name")]
    [SerializeField] private string playerName;

    protected override void OnDestroy() {
        base.OnDestroy();
        localName = null;
    }

    protected override void PostInit() {
        base.PostInit();
        Entity.SetName(playerName.Substring(0, playerName.IndexOf(" ")+1));
        // Entity.SetName(playerName + (playerName == LocalName ? " (YOU)" : ""));
    }
    
    protected override IMudTable GetTable() {return new NameTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        NameTable table = update as NameTable;
        playerName = NameUI.TableToName((int)table.first, (int)table.middle, (int)table.last);

        if(Entity.Key == NetworkManager.Instance.addressKey) {
            localName = playerName;
            OnLocalName?.Invoke();
        }
    }

}
