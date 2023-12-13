using mud;
using mudworld;

public abstract class MoverComponent : MUDComponent {

    public SPAnimator Animator {get{return animator;}}

    protected SPAnimationMover anim;
    protected SPAnimator animator;
    protected PositionSync sync;
    protected override void Init(SpawnInfo newSpawnInfo) {
        base.Init(newSpawnInfo);

        sync = gameObject.GetComponent<PositionSync>();
        if(sync == null) {
            sync = gameObject.AddComponent<PositionSync>();
            sync.SetSyncType(ComponentSync.ComponentSyncType.Lerp);
            sync.rotateToFace = true;
        }
        
        anim = GetComponentInChildren<SPAnimationMover>(true);
        animator = GetComponentInChildren<SPAnimator>(true);

    }
    
    // protected override void PostInit() {
    //     base.PostInit();
    // }

    // protected override void InitDestroy() {
    //     base.InitDestroy();

    // }

    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {}
    
}
