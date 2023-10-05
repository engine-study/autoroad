using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using IWorld.ContractDefinition;
public abstract class MoverComponent : MUDComponent {

    SPAnimationMover anim;
    PositionSync sync;
    protected override void Init(SpawnInfo newSpawnInfo) {
        base.Init(newSpawnInfo);

        sync = gameObject.GetComponent<PositionSync>();
        if(sync == null) {
            sync = gameObject.AddComponent<PositionSync>();
            sync.SetSyncType(ComponentSync.ComponentSyncType.Lerp);
            sync.rotateToFace = true;
        }
        
        anim = GetComponentInChildren<SPAnimationMover>();
        // ToggleRequiredComponent(true, MUDWorld.FindPrefab<PositionComponent>());
    }
    protected override void PostInit() {
        base.PostInit();
    }

    protected override void InitDestroy() {
        base.InitDestroy();

    }

    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateInfo newInfo)
    {

    }
}
