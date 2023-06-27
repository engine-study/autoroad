using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PlayerMUD : SPPlayer
{
    public Player Player{get{return playerComponent;}}
    public Position Position{get{return positionComponent;}}

    [Header("MUD")]
    [SerializeField] protected Player playerComponent;

    [Header("Debug")]
    [SerializeField] protected MUDEntity entity;
    [SerializeField] protected Position positionComponent;

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

        positionComponent = entity.GetMUDComponent<Position>() as Position;

        base.NetworkInit();

        Debug.Log("Player Network Init");

    }

    
}
