
using UnityEngine;
using mud.Client;
using NetworkManager = mud.Unity.NetworkManager;
using DefaultNamespace;
using IWorld.ContractDefinition;
using System.Numerics;
using System;
using System.Collections;

public class PlayerComponent : MUDComponent {
    public bool IsLocalPlayer { get { return isLocalPlayer; } }

    [Header("Player")]
    [SerializeField] bool spawned;
    [SerializeField] bool isLocalPlayer;
    [SerializeField] PlayerMUD playerScript;
    [SerializeField] SPInteract meleeInteract;

    [Header("Debug")]
    [SerializeField] HealthComponent health;
    [SerializeField] string attackerKey;

    public static string? LocalPlayerKey;

    public override void Init(MUDEntity ourEntity, TableManager ourTable) {
        base.Init(ourEntity, ourTable);

        isLocalPlayer = ourEntity.Key == NetworkManager.Instance.addressKey;
    }

    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {
        // throw new System.NotImplementedException();
    }

    protected override void PostInit() {
        base.PostInit();

        health = Entity.GetMUDComponent<HealthComponent>();
        meleeInteract.OnInteractToggle += Meleed;

    }

    //we got meleed
    public void Meleed() {

    }

    public void Meleed(bool toggle, IActor actor) {
        PlayerComponent otherPlayer = actor.Owner().GetComponent<PlayerComponent>();
        PlayerMUD playerScript = actor.Owner().GetComponent<PlayerMUD>();

        if(otherPlayer == null) {
            Debug.LogError("Not sure: " + actor.Owner().name, this);
        }

        string targetAddress = otherPlayer.Entity.Key;
        TxUpdate update = TxManager.MakeOptimistic(health, health.health - 1);
        TxManager.Send<MeleeFunction>(update, System.Convert.ToInt32(playerScript.Position.Pos.x), System.Convert.ToInt32(playerScript.Position.Pos.z));
        
    }

    void Die() {
        
    }
}
