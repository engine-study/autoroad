using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using DefaultNamespace;

public enum MoveType { None, Obstruction, Hole, Carry, Push, Trap } 
public class MoveComponent : MUDComponent {

    public bool HasBeenSunk {get { return pos.PosLayer.y < 0; } }
    public PositionComponent Pos {get { return pos; } }
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

            if (newInfo.Source != UpdateSource.Revert && lastPos != pos.Pos) {
                OnMove?.Invoke();
            }

            if (HasBeenSunk && pos.UpdateInfo.Source == UpdateSource.Onchain) {
                OnHole?.Invoke();
            }

            OnPositionUpdate?.Invoke(c,newInfo);
        }

        lastPos = pos.Pos;
    }





}
