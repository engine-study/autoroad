using UnityEngine;
using mudworld;
using mud;

public class PuzzleComponent : MUDComponent {

    [Header("Puzzle")]
    [SerializeField] SPEnableDisable completeFX;

    [EnumNamedArray( typeof(PuzzleType) )]
    [SerializeField] GameObject[] stages;
    [EnumNamedArray( typeof(PuzzleType) )]
    [SerializeField] GameObject[] stagesActive;
    [EnumNamedArray( typeof(PuzzleType) )]
    [SerializeField] GameObject[] stagesComplete;

    [Header("Debug")]
    public PuzzleType puzzle;
    public string solvedBy;
    public bool completed;
    bool wasCompleted = false; 
    [Header("Debug")]
    [SerializeField] PositionSync sync;

    protected override void PostInit() {
        base.PostInit();

        sync = Entity.GetRootComponent<PositionSync>();
        sync.OnMoveEnd += CompletedWhenMoved;

        transform.parent = sync.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if(puzzle == PuzzleType.Proctor) {

        } else {

        }
    
        InstantUpdate();
    }

    protected override MUDTable GetTable() {return new PuzzleTable();}
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {

        PuzzleTable table = update as PuzzleTable;
        puzzle = (PuzzleType)(int)table.PuzzleType;
        completed = (bool)table.Complete;
        solvedBy = (string)table.Solver;

        for(int i = 0; i < stages.Length; i++) {if(stages[i] == null) {continue;} stages[i].SetActive(i == (int)puzzle);}

        if(Loaded) {

        } else {
            InstantUpdate();
        }

        wasCompleted = completed;

    }

    void InstantUpdate() {
        for(int i = 0; i < stagesActive.Length; i++) {if(stagesActive[i] == null) {continue;} stagesActive[i].SetActive(!completed);}
        for(int i = 0; i < stagesComplete.Length; i++) {if(stagesComplete[i] == null) {continue;} stagesComplete[i].SetActive(completed);}
    }

    void CompletedWhenMoved() {

        if(Loaded && completed) {
                        
            PlayerComponent filledBy = MUDWorld.FindComponent<PlayerTable, PlayerComponent>(solvedBy);
            if(filledBy) {
                NotificationUI.AddNotification($"Puzzle solved by {filledBy.Entity.Name}");
            }
            
            completeFX.Spawn(true);
            InstantUpdate();

            sync.OnMoveEnd -= CompletedWhenMoved;

        }


    }

}
