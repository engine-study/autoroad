
using UnityEngine;
using mud.Client;
using NetworkManager = mud.Unity.NetworkManager;
using DefaultNamespace;
using IWorld.ContractDefinition;


public class PlayerComponent : MUDComponent {
    public bool IsLocalPlayer { get { return isLocalPlayer; } }

    [Header("Player")]
    [SerializeField] bool spawned;
    [SerializeField] bool isLocalPlayer;
    [SerializeField] PlayerMUD playerScript;
    [SerializeField] HealthComponent health;
    [SerializeField] SPInteract meleeInteract;

    public static string? LocalPlayerKey;

    public override void Init(MUDEntity ourEntity, TableManager ourTable) {
        base.Init(ourEntity, ourTable);

        playerScript = GetComponent<PlayerMUD>();
        isLocalPlayer = ourEntity.Key == NetworkManager.Instance.addressKey;
        meleeInteract.OnInteractToggle += Meleed;
    }

    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {
        // throw new System.NotImplementedException();
    }

    protected override void PostInit() {
        base.PostInit();

        health = Entity.GetMUDComponent<HealthComponent>();

    }

    //we got meleed
    public void Meleed() {

    }

    public void Meleed(bool toggle, IActor actor) {
        PlayerComponent otherPlayer = actor.Owner().GetComponent<PlayerComponent>();

        if(otherPlayer == null) {
            Debug.LogError("Not sure: " + actor.Owner().name, this);
        }

        string targetAddress = otherPlayer.Entity.Key;
        TxUpdate update = TxManager.MakeOptimistic(health, health.health - 1);
        TxManager.Send<MeleeFunction>(update, targetAddress);
    }

    void Die() {
        
    }
}
