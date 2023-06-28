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
        
        Debug.Log("Player Init");

        entity = GetComponentInParent<MUDEntity>();
        if(Player.Loaded) NetworkInit();
        else Player.OnLoaded += NetworkInit;

    }

    protected override void Destroy()
    {
        base.Destroy();
        Player.OnLoaded -= NetworkInit;
    }


    protected override void NetworkInit() {
        
        if(playerComponent.IsLocalPlayer) {
            SetLocalPlayer(this);
        }

        positionComponent = entity.GetMUDComponent<PositionComponent>() as PositionComponent;
        transform.position = positionComponent.Pos;
        base.NetworkInit();

        Debug.Log("Player Network Init");

    }

    
}
