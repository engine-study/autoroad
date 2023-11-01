using mud;
using mudworld;

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

    }
    
    // protected override void PostInit() {
    //     base.PostInit();
    // }

    // protected override void InitDestroy() {
    //     base.InitDestroy();

    // }

    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {}
    
}
