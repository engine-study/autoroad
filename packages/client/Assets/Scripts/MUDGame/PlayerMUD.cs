using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PlayerMUD : SPPlayer
{
    public PlayerComponent Player{get{return playerComponent;}}
    public PositionComponent Position{get{return positionComponent;}}
    public PositionSync Sync{get{return positionSync;}}
    public ActionsMUD Actions{get{return actions;}}
    public AnimationMUD AnimationMUD{get{return animMud;}}
    public static bool CanInput {get{return LocalPlayer && LocalPlayer.HasInit && !PlayerComponent.LocalPlayer.IsDead;}}

    [Header("MUD")]
    [SerializeField] private PlayerComponent playerComponent;
    [SerializeField] private PositionSync positionSync;
    [SerializeField] private ActionsMUD actions;
    [SerializeField] private AnimationMUD animMud;
    [SerializeField] public Cosmetic bodyCosmetic;
    [SerializeField] public Cosmetic headCosmetic;
    [SerializeField] public Cosmetic capeCosmetic;
    [SerializeField] public Cosmetic backpackCosmetic;
    Cosmetic [] cosmetics;
    [SerializeField] AudioClip [] sfx_equip;

    [Header("Debug")]
    [SerializeField] private PositionComponent positionComponent;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private GemComponent gemComponent;
    [SerializeField] private ActionComponent action;

    public override void Init() {
        base.Init();
        
        // Debug.Log("Player Init");
        if(Player.Loaded) DoNetworkInit();
        else Player.OnComponentsLoaded += DoNetworkInit;

        cosmetics = new Cosmetic[] {bodyCosmetic, headCosmetic, capeCosmetic, backpackCosmetic};
        for(int i = 0; i < cosmetics.Length; i++) {cosmetics[i].OnUpdated += Equip;}

    }

    protected override void NetworkInit() {
        
        base.NetworkInit();

        // for(int i = 0; i < playerComponent.Entity.Components.Count; i++) {
        //     Debug.Log("Player: " + playerComponent.Entity.Components[i].GetType().ToString());
        // }

        positionComponent = playerComponent.Entity.GetMUDComponent<PositionComponent>();
        transform.position = positionComponent.Pos;

        action = MUDWorld.FindOrMakeComponent<ActionComponent>(playerComponent.Entity);
        healthComponent = playerComponent.Entity.GetMUDComponent<HealthComponent>();
        gemComponent = playerComponent.Entity.GetMUDComponent<GemComponent>();

        baseName = MUDHelper.TruncateHash(playerComponent.Entity.Key);

        // Debug.Log("Player Network Init");

        if(healthComponent.Health < 0) {
            Root.gameObject.SetActive(false);
        }

        if(playerComponent.IsLocalPlayer) {
            SetLocalPlayer(this);
        }
        
        if(IsLocalPlayer) {
            Actor.OnActionEnd += ActionCursorUpdate;
        }

    }

    protected override void Destroy()
    {
        base.Destroy();
        Player.OnComponentsLoaded -= NetworkInit;
        Actor.OnActionEnd -= ActionCursorUpdate;
        for(int i = 0; i < cosmetics.Length; i++) {cosmetics[i].OnUpdated -= Equip;}

    }
    
    void Equip() {SPAudioSource.Play(Root.position, sfx_equip);}

    //refresh what available actions we have
    void ActionCursorUpdate(ActionEndState endState) {
        CursorMUD.ForceCursorUpdate();
    }


    protected override void UpdateInput() {
        base.UpdateInput();

        if (Reciever.TargetGO) {
            Actor.InputClick(0, Reciever.TargetInteract);
        }

        if(Sync.Moving == false) {
            //update rotation based on mouseInput
            // Determine the new rotation
            Vector3 mouseDir = SPInput.MousePlanePos - transform.position;
            if ( mouseDir.magnitude > .5f) { //playerScript.Actor.ActionState == ActionState.Idle && mouseDir.magnitude > .5f
                Animator.IK.SetLook(CursorMUD.LookTarget);
                animMud.Look.SetLookRotation(transform.position + mouseDir);
            } 
        } else {
            Animator.IK.SetLook(null);
        }

    }
    
}
