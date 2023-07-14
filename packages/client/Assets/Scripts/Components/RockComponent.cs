using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using mud.Unity;
using IWorld.ContractDefinition;

public enum RockType { None, Stone, Statumen, Pavimentum, Rudus, Nucleus, _Count }
public class RockComponent : MUDComponent {

    [Header("Rock")]
    [SerializeField] protected RockType rockType;
    [SerializeField] ParticleSystem fx_break, fx_drag;
    [SerializeField] SPAudioSource source;
    SPBase rockBase;

    [SerializeField] GameObject[] stages;
    public AudioClip[] sfx_drag, sfx_dragBase, sfx_pickHit, sfx_whoosh, sfx_smallBreaks, sfx_bigBreaks;
    RockType lastStage = RockType._Count;

    protected override void Awake() {
        base.Awake();
        rockType = RockType._Count;
        rockBase = GetComponent<SPBase>();
    }

    public override void Init(MUDEntity ourEntity, MUDTableManager ourTable) {
        base.Init(ourEntity, ourTable);

        ourEntity.OnComponentUpdated += UpdatePositionCheck;
    }

    protected override void InitDestroy() {
        base.InitDestroy();
        if(Entity) {
            Entity.OnComponentUpdated -= UpdatePositionCheck;
        }

    }

    Vector3 lastPos;
    void UpdatePositionCheck(MUDComponent c, UpdateEvent updateType) {

        PositionComponent pos = c as PositionComponent;
        if(pos) {
            if(updateType != UpdateEvent.Revert && lastPos != pos.Pos) {
                fx_drag.Play();
                source.PlaySound(sfx_drag);
                source.PlaySound(sfx_dragBase);
            }
            
            lastPos = pos.Pos;

        }


    }

    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateEvent eventType) {

        base.UpdateComponent(update, eventType);

        RockTable rockUpdate = (RockTable)update;

        if (rockUpdate == null) {
            Debug.LogError("No rockUpdate", this);
        } else {

            // stage = rockUpdate.rockType != null ? (int)rockUpdate.rockType : stage;
            // rockType = rockUpdate.rockType != null ? (RockType)rockUpdate.rockType : rockType;
            // Debug.Log(rockUpdate.value.ToString());

            rockType = rockUpdate.value != null ? (RockType)rockUpdate.value : RockType._Count;

        }

        rockBase.baseName = rockType.ToString();
        for (int i = 0; i < stages.Length; i++) {
            stages[i].SetActive(i == (int)rockType);
        }

        if (Loaded && lastStage != rockType) {

            if (eventType == UpdateEvent.Update || eventType == UpdateEvent.Optimistic) {
                source.PlaySound(sfx_whoosh);
                if(lastStage < RockType.Rudus) {
                    source.PlaySound(sfx_pickHit);
                    source.PlaySound(sfx_bigBreaks);
                } else {
                    source.PlaySound(sfx_smallBreaks);
                }
              
                fx_break.Play();
            }
        }

        lastStage = rockType;

    }

    public void Mine() {
        MineRock((int)transform.position.x, (int)transform.position.z);
    }

    public async void MineRock(int x, int y) {
        List<TxUpdate> updates = new List<TxUpdate>();
        updates.Add(TxManager.MakeOptimistic(this, (int)(rockType + 1)));
        await TxManager.Send<MineFunction>(updates, x, y);
    }
    // public async void MineRock(int x, int y)
    // {
    //     try
    //     {
    //         RockTable fakeTable = new RockTable();
    //         fakeTable.value = (ulong)(rockType+1);
    //         // function moveFrom(int32 startX, int32 startY, int32 x, int32 y) public {
    //         UpdateComponent(fakeTable, UpdateEvent.Optimistic);
    //         await NetworkManager.Instance.worldSend.TxExecute<MineFunction>(x, y);
    //     }
    //     catch (System.Exception ex)
    //     {
    //         //if our transaction fails, force the player back to their position on the table
    //         Debug.LogException(ex);
    //         RockTable fakeTable = new RockTable();
    //         // fakeTable.value = RockTable.GetRockTableValue(Entity.Key).value;
    //         UpdateComponent(fakeTable, UpdateEvent.Revert);

    //     }
    // }
}
