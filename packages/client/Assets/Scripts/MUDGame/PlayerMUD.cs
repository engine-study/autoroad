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
    [SerializeField] protected MUDEntity entity;
    [SerializeField] protected PositionComponent positionComponent;

    public override void Init() {
        base.Init();
        
        // Debug.Log("Player Init");

        entity = GetComponentInParent<MUDEntity>();
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

        if(playerComponent.IsLocalPlayer) {
            SetLocalPlayer(this);
        }

        positionComponent = entity.GetMUDComponent<PositionComponent>();
        transform.position = positionComponent.Pos;

        baseName = MUDHelper.TruncateHash(GetComponent<PlayerComponent>().Entity.Key);

        // Debug.Log("Player Network Init");

    }

    protected override void UpdateInput() {
        base.UpdateInput();

        if (Reciever.TargetGO) {
            Actor.InputClick(0, Reciever.TargetInteract);
        }



    }



    
}
