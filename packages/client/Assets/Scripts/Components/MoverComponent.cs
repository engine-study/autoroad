using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using IWorld.ContractDefinition;
public abstract class MoverComponent : MUDComponent {

    SPAnimationMover anim;
    PositionSync sync;
    protected override void Init(MUDEntity ourEntity, TableManager ourTable) {
        base.Init(ourEntity, ourTable);
        sync = gameObject.AddComponent<PositionSync>();
        sync.rotateToFace = true;
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
