
using UnityEngine;
using mud;
using mudworld;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerComponent : MUDComponent {

    public static int PlayerCount;
    public static PlayerComponent LocalPlayer;
    public static Action OnPlayerSpawn;


    public bool IsLocalPlayer { get { return isLocalPlayer; } }
    public bool IsDead {get{return health.IsDead;}}
    public string Address {get{return address;}}
    public PlayerMUD PlayerScript {get{return playerScript;}}

    [Header("Player")]
    [SerializeField] bool isLocalPlayer;
    [SerializeField] PlayerMUD playerScript;
    [SerializeField] AudioClip [] sfx_hitSound;

    [Header("Debug")]
    [SerializeField] string address;
    [SerializeField] HealthComponent health;
    [SerializeField] PositionComponent position;

    int lastHealth;
    bool wasAlive = false;

    protected override void Init(SpawnInfo newSpawnInfo) {
        base.Init(newSpawnInfo);

        PlayerCount++;

        isLocalPlayer = Entity.Key == NetworkManager.LocalKey;
        if (IsLocalPlayer) {
            LocalPlayer = this;
            OnPlayerSpawn?.Invoke();
        }
    }

    
    protected override void InitDestroy() {
        base.InitDestroy();

        PlayerCount--;

        if(health) { health.OnUpdated -= CheckHealth;}
        if(position) { position.OnUpdated -= CheckPosition;}

    }


    protected override MUDTable GetTable() {return new PlayerTable();}
    protected override void UpdateComponent(MUDTable table, UpdateInfo newInfo) {

        address = "0x" + Entity.Key.Substring(Entity.Key.Length - 20);

    }

    protected override void PostInit() {
        base.PostInit();

        health = Entity.GetMUDComponent<HealthComponent>();
        health.OnUpdated += CheckHealth;

        position = Entity.GetMUDComponent<PositionComponent>();
        position.OnUpdated += CheckPosition;

    }

    
    protected override void OnDestroy() {
        base.OnDestroy();

        if(isLocalPlayer) {
            LocalPlayer = null;
        }
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
            MotherUI.ToggleRespawn(health.IsDead);
        }

        lastHealth = health.Health;
    }

    void CheckPosition() {

        DeathCheck();

    }

    void DeathCheck() {

        if(!HasInit) {
            return;
        }

        bool isAlive = position.UpdateInfo.UpdateType != UpdateType.DeleteRecord;

        if(isAlive) {

            if(killCoroutine != null) {StopCoroutine(killCoroutine);}
            playerScript.Root.gameObject.SetActive(isAlive);

            //we are alive again after being dead
            if(HasInit) {
                playerScript.gameObject.SetActive(isAlive);

                // playerScript.Respawn(transform.position);
                // playerScript.Root.gameObject.SetActive(isAlive);
                // playerScript.Animator.PlayClip("Idle");
            }
            
        } else {
            //we are dead
            //no, let death action state do this
            // playerScript.Kill();
            // killCoroutine = StartCoroutine(KillCoroutine());
        }
    }

    Coroutine killCoroutine;
    IEnumerator KillCoroutine() {
        yield return new WaitForSeconds(2f);
        playerScript.Root.gameObject.SetActive(false);

    }

    void Die() {
        
    }
}
