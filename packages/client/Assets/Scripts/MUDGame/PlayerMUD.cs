using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PlayerMUD : SPPlayer
{
    public PlayerComponent Player{get{return playerComponent;}}
    public PositionComponent Position{get{return positionComponent;}}

    [Header("MUD")]
    [SerializeField] protected PlayerComponent playerComponent;

    [Header("Debug")]
    [SerializeField] protected PositionComponent positionComponent;
    [SerializeField] protected HealthComponent healthComponent;

    public override void Init() {
        base.Init();
        
        // Debug.Log("Player Init");
        if(Player.Loaded) DoNetworkInit();
        else Player.OnLoaded += DoNetworkInit;

    }

    protected override void Destroy()
    {
        base.Destroy();
        Player.OnLoaded -= NetworkInit;
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
