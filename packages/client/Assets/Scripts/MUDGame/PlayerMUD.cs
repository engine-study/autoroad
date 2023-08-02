using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PlayerMUD : SPPlayer
{
    public PlayerComponent Player{get{return playerComponent;}}
    public PositionComponent Position{get{return positionComponent;}}

    [Header("MUD")]
    [SerializeField] private PlayerComponent playerComponent;
    [SerializeField] public Cosmetic bodyCosmetic;
    [SerializeField] public Cosmetic headCosmetic;
    [SerializeField] public Cosmetic capeCosmetic;
    [SerializeField] public Cosmetic backpackCosmetic;
    Cosmetic [] cosmetics;
    [SerializeField] AudioClip [] sfx_equip;

    [Header("Debug")]
    [SerializeField] private PositionComponent positionComponent;
    [SerializeField] private HealthComponent healthComponent;

    public override void Init() {
        base.Init();
        
        // Debug.Log("Player Init");
        if(Player.Loaded) DoNetworkInit();
        else Player.OnLoaded += DoNetworkInit;

        cosmetics = new Cosmetic[] {bodyCosmetic, headCosmetic, capeCosmetic, backpackCosmetic};
        for(int i = 0; i < cosmetics.Length; i++) {cosmetics[i].OnUpdated += Equip;}
    }
    void Equip() {SPAudioSource.Play(Root.position, sfx_equip);}

    protected override void Destroy()
    {
        base.Destroy();
        Player.OnLoaded -= NetworkInit;
        for(int i = 0; i < cosmetics.Length; i++) {cosmetics[i].OnUpdated -= Equip;}

    }

    protected override void NetworkInit() {
        
        base.NetworkInit();

        // for(int i = 0; i < playerComponent.Entity.Components.Count; i++) {
        //     Debug.Log("Player: " + playerComponent.Entity.Components[i].GetType().ToString());
        // }

        positionComponent = playerComponent.Entity.GetMUDComponent<PositionComponent>();
        transform.position = positionComponent.Pos;

        healthComponent = playerComponent.Entity.GetMUDComponent<HealthComponent>();

        baseName = MUDHelper.TruncateHash(playerComponent.Entity.Key);

        // Debug.Log("Player Network Init");

        if(playerComponent.IsLocalPlayer) {
            SetLocalPlayer(this);
        }

        if(healthComponent.Health < 0) {
            Root.gameObject.SetActive(false);
        }

    }



    protected override void UpdateInput() {
        base.UpdateInput();

        if (Reciever.TargetGO) {
            Actor.InputClick(0, Reciever.TargetInteract);
        }

    }
    
    public override void Kill() {
        base.Kill();

        Animator.PlayClip("Die");

    }



    
}
