
using UnityEngine;
using mud.Client;
using NetworkManager = mud.Unity.NetworkManager;

public class PlayerComponent : MUDComponent {
    public bool IsLocalPlayer { get { return isLocalPlayer; } }

    [Header("Player")]
    [SerializeField] bool spawned;
    [SerializeField] bool isLocalPlayer;

    public static string? LocalPlayerKey;

    public override void Init(MUDEntity ourEntity, TableManager ourTable) {
        base.Init(ourEntity, ourTable);

        isLocalPlayer = ourEntity.Key == NetworkManager.Instance.addressKey;
    }

    protected override void UpdateComponent(IMudTable table, UpdateEvent eventType) {
        // throw new System.NotImplementedException();
    }
}
