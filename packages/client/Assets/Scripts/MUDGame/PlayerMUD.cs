using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;

public class PlayerMUD : SPPlayer
{
    public static PlayerMUD MUDPlayer {get{return (PlayerMUD)LocalPlayer;}}
    public PlayerComponent Player{get{return playerComponent;}}
    public PositionComponent Position{get{return positionComponent;}}
    public PositionSync Sync{get{return positionSync;}}
    public ActionsMUD Equipment{get{return actions;}}
    public AnimationMUD AnimationMUD{get{return animMud;}}
    protected override bool Input(){ return base.Input() && GameState.GamePlaying && GameState.GameReady && !PlayerComponent.LocalPlayer.IsDead && TxManager.CanSendTx; }

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

    public override void Init() {
        base.Init();
        
        // Debug.Log("Player Init");
        if(Player.Loaded) DoNetworkInit();
        else Player.OnLoaded += DoNetworkInit;

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

        baseName = MUDHelper.TruncateHash(playerComponent.Entity.Key);

        // Debug.Log("Player Network Init");

        if(playerComponent.IsLocalPlayer) {
            SetLocalPlayer(this);
            Actor.OnActionEnd += ActionCursorUpdate;
            positionSync.OnMoveEnd += MoveEndUpdate;
        }

    }

    protected override void Destroy()
    {
        base.Destroy();

        Player.OnLoaded -= NetworkInit;
        Actor.OnActionEnd -= ActionCursorUpdate;
        positionSync.OnMoveEnd -= MoveEndUpdate;

        for(int i = 0; i < cosmetics.Length; i++) {cosmetics[i].OnUpdated -= Equip;}

    }
    
    void Equip() {SPAudioSource.Play(Root.position, sfx_equip);}

    //refresh what available actions we have
    void ActionCursorUpdate(ActionEndState endState) { CursorMUD.ForceCursorUpdate();}
    void MoveEndUpdate() { CursorMUD.ForceCursorUpdate();}

    public void UseTool() {
        Actor.InputClick(0, Reciever.TargetInteract);
    }

    protected override void UpdateInput() {
        base.UpdateInput();

        if(AnimationMUD.Action == ActionName.Dead) {
            return;
        }

        if(ToolUI.Instance.Tool) {
            Actor.InputClick(0, ToolUI.Instance.Tool.equipment);
        }

        if((Controller as ControllerMUD).MovementInput || Sync.Moving) {
            Animator.IK.SetLook(null);
        } else {
            //update rotation based on mouseInput
            // Determine the new rotation
            Vector3 mouseDir = SPInput.MousePlanePos - transform.position;
            if ( mouseDir.magnitude > .5f) { //playerScript.Actor.ActionState == ActionState.Idle && mouseDir.magnitude > .5f
                Animator.IK.SetLook(CursorMUD.LookTarget);
                animMud.Look.SetLookRotation(transform.position + mouseDir);
            } else {
                Animator.IK.SetLook(null);
            }
        }

    }
    
}
