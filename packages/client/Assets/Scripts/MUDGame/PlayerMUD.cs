using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PlayerMUD : SPPlayer
{
    public Player Player{get{return playerComponent;}}
    public Position Position{get{return positionComponent;}}

    [Header("MUD")]
    [SerializeField] protected MUDEntity entity;
    [SerializeField] protected Player playerComponent;
    [SerializeField] protected Position positionComponent;

    public override void Init() {
        base.Init();
        
        entity = GetComponentInParent<MUDEntity>();
        Player.OnLoaded += NetworkInit;
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

    }

    
}
