using UnityEngine;
using mudworld;
using mud;
using mud;
using System;

public class MiliariumComponent : MUDComponent {

    [Header("Miliarium")]
    [SerializeField] GameObject puzzleActive;
    [SerializeField] GameObject puzzleComplete;
    [Header("Debug")]
    [SerializeField] RockComponent rock;
    [SerializeField] PuzzleComponent puzzle;


    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Milliarium");

        rock = Entity.GetMUDComponent<RockComponent>();
        transform.parent = rock.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        puzzle = Entity.GetMUDComponent<PuzzleComponent>();
        puzzle.OnUpdated += UpdatePuzzleState;

        UpdatePuzzleState();
    
    }

    // protected override void OnDestroy() {
    //     base.OnDestroy();

    // }

    protected override void InitDestroy()
    {
        base.InitDestroy();
        if(puzzle) {puzzle.OnUpdated -= UpdatePuzzleState;}
    }


    protected override MUDTable GetTable() {return new MiliariumTable();}

    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {

        MiliariumTable table = update as MiliariumTable;

    }

    void UpdatePuzzleState() {
        if(!puzzle) return;

        puzzleActive.SetActive(!puzzle.completed);
        puzzleComplete.SetActive(puzzle.completed);
    }

}
