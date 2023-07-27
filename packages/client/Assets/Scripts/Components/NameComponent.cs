using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class NameComponent : MUDComponent {
    public string Name { get { return playerName; } }

    [Header("Name")]
    [SerializeField] private string playerName;

    protected override void PostInit() {
        base.PostInit();
        Entity.SetName(playerName);
    }
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        NameTable table = update as NameTable;
        playerName = NameUI.TableToName((int)table.first, (int)table.middle, (int)table.last);

    }

}
