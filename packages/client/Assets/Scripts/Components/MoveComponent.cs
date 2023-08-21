using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public enum MoveType { None, Obstruction, Hole, Carry, Push } 
public class MoveComponent : MUDComponent {

    public bool HasBeenSunk {get { return pos.PosLayer.y < 0; } }
    public System.Action OnMove, OnPush, OnHole;
    public System.Action<MUDComponent, UpdateInfo> OnPositionUpdate;
    
    [Header("Move")]
    public MoveType MoveType;
    [SerializeField] PositionComponent pos;
    [SerializeField] SPFlashShake flash;


    protected override IMudTable GetTable() { return new MoveTable(); }
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        MoveTable table = (MoveTable)update;
        MoveType = (MoveType)table.value;

    }

    protected override void PostInit() {
        base.PostInit();

        pos = Entity.GetMUDComponent<PositionComponent>();

        if(pos == null) {
            return;
        }

        pos.OnUpdatedInfo += UpdatePositionCheck;

        if (HasBeenSunk) {
            gameObject.SetActive(false);
        }
    }

    protected override void InitDestroy() {
        base.InitDestroy();

        if(pos) {
            pos.OnUpdatedInfo -= UpdatePositionCheck;
        }
    }


    Vector3 lastPos;
    void UpdatePositionCheck(MUDComponent c, UpdateInfo newInfo) {

        PositionComponent pos = c as PositionComponent;

        if (Loaded) {

            if (newInfo.UpdateType == UpdateType.DeleteRecord){
                gameObject.SetActive(false);
            }

            if (newInfo.UpdateSource != UpdateSource.Revert && lastPos != pos.Pos) {
                OnMove?.Invoke();
            }

            if (HasBeenSunk && pos.UpdateSource == UpdateSource.Onchain) {
                OnHole?.Invoke();
            }

            OnPositionUpdate?.Invoke(c,newInfo);
        }

        lastPos = pos.Pos;
    }





}
