using UnityEngine;
using mudworld;
using mud;
using mud;
using System;

public class NameComponent : MUDComponent {

    public static Action OnLocalName;
    public string Name { get { return playerName; } }
    public static string LocalName;

    [Header("Name")]
    [SerializeField] private string playerName;
    [SerializeField] private string localNameDebug;

    protected override void OnDestroy() {
        base.OnDestroy();
        LocalName = null;
    }

    protected override void PostInit() {
        base.PostInit();
        // Entity.SetName(playerName.Substring(0, playerName.IndexOf(" ")+1));
        Entity.SetName(playerName);
        // Entity.SetName(playerName + (playerName == LocalName ? " (YOU)" : ""));
    }
    
    protected override MUDTable GetTable() {return new NameTable();}
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {

        NameTable table = update as NameTable;
        playerName = NameUI.TableToName((int)table.First, (int)table.Middle, (int)table.Last);

        if(Entity.Key == NetworkManager.LocalKey) {
            LocalName = playerName;
            localNameDebug = playerName;
            OnLocalName?.Invoke();
        }
    }

}
