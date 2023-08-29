
using UnityEngine;
using mud.Client;
using NetworkManager = mud.Unity.NetworkManager;
using DefaultNamespace;
using IWorld.ContractDefinition;
using System.Numerics;
using System;
using System.Collections;

public class PlayerComponent : MUDComponent {

    public static PlayerComponent LocalPlayer;
    public static System.Action OnPlayerSpawn;
    public bool IsLocalPlayer { get { return isLocalPlayer; } }
    public PlayerMUD PlayerScript {get{return playerScript;}}

    [Header("Player")]
    [SerializeField] bool spawned;
    [SerializeField] bool isLocalPlayer;
    [SerializeField] PlayerMUD playerScript;
    [SerializeField] SPInteract meleeInteract;
    [SerializeField] ParticleSystem fx_death;    
    [SerializeField] AudioClip [] sfx_hitSound;
    [SerializeField] AudioClip [] sfx_deathSound;

    [Header("Debug")]
    [SerializeField] HealthComponent health;
    [SerializeField] PositionComponent position;
    [SerializeField] GameEventComponent gameEvent;
    int lastHealth;

    protected override void Init(SpawnInfo newSpawnInfo) {
        base.Init(newSpawnInfo);

        isLocalPlayer = Entity.Key == NetworkManager.LocalAddress;
        if (IsLocalPlayer) {
            LocalPlayer = this;
            OnPlayerSpawn?.Invoke();
        }

    }

    protected override IMudTable GetTable() {return new PlayerTable();}
    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {

    }

    protected override void PostInit() {
        base.PostInit();

        health = Entity.GetMUDComponent<HealthComponent>();
        health.OnUpdated += CheckHealth;

        position = Entity.GetMUDComponent<PositionComponent>();
        position.OnUpdated += CheckPosition;

        meleeInteract.OnInteractToggle += Meleed;

    }

    void AddGameEvents() {
        gameEvent = Entity.GetMUDComponent<GameEventComponent>();
        gameEvent.OnUpdated += PlayerEvent;

    }

    protected override void InitDestroy() {
        base.InitDestroy();

        if(health) {
            health.OnUpdated -= CheckHealth;
        }

        if(position) {
            position.OnUpdated -= CheckPosition;
        }

        if(gameEvent)
            gameEvent.OnUpdated -= PlayerEvent;

    }

    
    protected override void OnDestroy() {
        base.OnDestroy();
        isLocalPlayer = false;
        LocalPlayer = null;
    }


    //we got meleed
    public void Meleed() {

    }


    void PlayerEvent() {
        string eventName = gameEvent.GameEvent;

        playerScript.Resources.fx_spawn.Play();
    }

    void CheckHealth() {

        if(!Loaded) {
            return;
        }

        if(health.Health != lastHealth) {
                    
            if(health.Health < 1) {

            } else if(lastHealth < 1) {

            } else {
                //normal hit
                playerScript.Animator.PlayClip("Hit");
                SPAudioSource.Play(transform.position, sfx_hitSound);

            }
        }

        if(IsLocalPlayer) {
            Debug.Log("Died, showing Spawn UI", this);
            MotherUI.TogglePlayerSpawning(health.Health <= 0 || health.UpdateInfo.UpdateType == UpdateType.DeleteRecord);
        }

        lastHealth = health.Health;
    }

    void CheckPosition() {

        bool isAlive = position.UpdateInfo.UpdateType != UpdateType.DeleteRecord;

        //we are dead
        if(isAlive) {
            if(killCoroutine != null) {StopCoroutine(killCoroutine);}
            //we are alive again after being dead
            playerScript.Respawn(transform.position);
            playerScript.Root.gameObject.SetActive(isAlive);
            playerScript.Animator.PlayClip("Idle");
        } else {
            playerScript.Kill();
            fx_death.Play();
            killCoroutine = StartCoroutine(KillCoroutine());
        }


    }

    Coroutine killCoroutine;
    IEnumerator KillCoroutine() {
        yield return new WaitForSeconds(.5f);
        playerScript.Root.gameObject.SetActive(false);

    }

    public void Meleed(bool toggle, IActor actor) {

        if(!toggle) {
            return;
        }

        if(health.Health < 1) {
            //already dead
            return;
        }

        PlayerComponent otherPlayer = actor.Owner().GetComponent<PlayerComponent>();
        PlayerMUD otherScript = actor.Owner().GetComponent<PlayerMUD>();

        if(otherPlayer == null) {
            Debug.LogError("Not sure: " + actor.Owner().name, this);
        }

        Debug.Log("Meleed", this);

        string targetAddress = otherPlayer.Entity.Key;
        TxUpdate update = TxManager.MakeOptimistic(health, health.Health == 1 ? -1 : health.Health - 1);
        TxManager.Send<MeleeFunction>(update, System.Convert.ToInt32(playerScript.Position.Pos.x), System.Convert.ToInt32(playerScript.Position.Pos.z));

    }

    void Die() {
        
    }
}
