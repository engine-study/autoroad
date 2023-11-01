using UnityEngine;
using mudworld;
using mud;
using mud;
using System;

public enum PuzzleType {None, Miliarium, Bearer, Statuae, Count}
public class PuzzleComponent : MUDComponent {

    [Header("Puzzle")]
    [SerializeField] SPEnableDisable completeFX;

    [EnumNamedArray( typeof(PuzzleType) )]
    [SerializeField] GameObject[] stages;
    [SerializeField] GameObject[] stagesComplete;

    [Header("Debug")]
    public PuzzleType puzzle;
    public bool completed;

    [Header("Miliarium")]

    [Header("Statue")]

    [Header("Debug")]
    [SerializeField] PositionSync sync;
    [SerializeField] RockComponent rock;


    protected override IMudTable GetTable() {return new PuzzleTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        PuzzleTable table = update as PuzzleTable;
        puzzle = (PuzzleType)(int)table.PuzzleType;
        completed = (bool)table.Complete;

        if(completed && Loaded) {

            PlayerComponent filledBy = MUDWorld.FindComponent<PlayerComponent>((string)table.Solver);
            if(filledBy) {
                NotificationUI.AddNotification($"Puzzle solved by {filledBy.Entity.Name}");
            }
        }

        for(int i = 0; i < stages.Length; i++) {if(stages[i] == null) continue; stages[i].SetActive(i == (int)puzzle);}
        for(int i = 0; i < stagesComplete.Length; i++) {if(stagesComplete[i] == null) continue; stagesComplete[i].SetActive(false);}

    }

    protected override void PostInit() {
        base.PostInit();

        rock = Entity.GetMUDComponent<RockComponent>();
        sync = rock.GetComponent<PositionSync>();

        sync.OnMoveEnd += CompletedWhenMoved;

        transform.parent = rock.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    
    }

    void CompletedWhenMoved() {
        if(completed) {
            completeFX.Spawn(true);
            for(int i = 0; i < stagesComplete.Length; i++) {if(stagesComplete[i] == null) continue; stagesComplete[i].SetActive(true);}
        }


    }

}
