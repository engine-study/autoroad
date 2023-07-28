
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
    [SerializeField] AudioClip [] sfx_hitSound;
    [SerializeField] AudioClip [] sfx_deathSound;

    [Header("Debug")]
    [SerializeField] HealthComponent health;
    int lastHealth;

    public static string? LocalPlayerKey;

    protected override void Init(MUDEntity ourEntity, TableManager ourTable) {
        base.Init(ourEntity, ourTable);

        isLocalPlayer = ourEntity.Key == NetworkManager.LocalAddress;
    }

    protected override IMudTable GetTable() {return new PlayerTable();}
    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {
        // throw new System.NotImplementedException();
    }

    protected override void PostInit() {
        base.PostInit();

        health = Entity.GetMUDComponent<HealthComponent>();
        health.OnUpdated += CheckHealth;
        meleeInteract.OnInteractToggle += Meleed;

    }

    
    protected override void OnDestroy() {
        base.OnDestroy();
        LocalPlayerKey = null;
        isLocalPlayer = false;
    }


    //we got meleed
    public void Meleed() {

    }

    Coroutine dieCoroutine;

    void CheckHealth() {

        if(!Loaded) {
            return;
        }

        if(health.health != lastHealth) {
            if(health.health < 0) {
                SPAudioSource.Play(transform.position, sfx_deathSound);
                playerScript.Animator.PlayClip("Die");
            } else {
                SPAudioSource.Play(transform.position, sfx_hitSound);
                playerScript.Animator.PlayClip("Hit");
            }
        }

        if(lastHealth < 1 && health.health > 0) {
            gameObject.SetActive(health.health > 0);
        }

        if(IsLocalPlayer) {
            MotherUI.TogglePlayerSpawning(health.health <= 0);
        }

        lastHealth = health.health;
    }

    public void Meleed(bool toggle, IActor actor) {
        PlayerComponent otherPlayer = actor.Owner().GetComponent<PlayerComponent>();
        PlayerMUD otherScript = actor.Owner().GetComponent<PlayerMUD>();

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
