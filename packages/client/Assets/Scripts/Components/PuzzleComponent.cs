using UnityEngine;
using mudworld;
using mud;
using mud;
using System;

public enum PuzzleType {None, Miliarium, Bearer, Statuae, Count}
public class PuzzleComponent : MUDComponent {

    [Header("Puzzle")]
    [EnumNamedArray( typeof(PuzzleType) )]
    [SerializeField] GameObject[] stages;

    [Header("Debug")]
    public PuzzleType puzzle;
    public bool completed;

    [Header("Miliarium")]
    [SerializeField] GameObject puzzleActive;
    [SerializeField] GameObject puzzleComplete;

    [Header("Statue")]

    [Header("Debug")]
    [SerializeField] RockComponent rock;

    protected override IMudTable GetTable() {return new PuzzleTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        PuzzleTable table = update as PuzzleTable;
        puzzle = (PuzzleType)(int)table.PuzzleType;
        completed = (bool)table.Complete;

        for(int i = 0; i < stages.Length; i++) {if(stages[i] == null) continue; stages[i].SetActive(i == (int)puzzle);}

        UpdatePuzzleState();

    }

    protected override void PostInit() {
        base.PostInit();

        rock = Entity.GetMUDComponent<RockComponent>();
        transform.parent = rock.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        UpdatePuzzleState();
    
    }

    void UpdatePuzzleState() {
        puzzleActive.SetActive(!completed);
        puzzleComplete.SetActive(completed);
    }

}
