using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;
using System;

public class MiliariumComponent : MUDComponent {

    PuzzleComponent puzzle;
    public GameObject puzzleActive;
    public GameObject puzzleComplete;
    protected override void OnDestroy() {
        base.OnDestroy();

    }

    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Milliarium");

        puzzle = Entity.GetMUDComponent<PuzzleComponent>();
        puzzle.OnUpdated += UpdatePuzzleState;
        UpdatePuzzleState();
    }

    protected override void InitDestroy()
    {
        base.InitDestroy();
        puzzle.OnUpdated -= UpdatePuzzleState;
    }


    protected override IMudTable GetTable() {return new MiliariumTable();}

    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        MiliariumTable table = update as MiliariumTable;

    }

    void UpdatePuzzleState() {
        if(!puzzle) return;

        puzzleActive.SetActive(!puzzle.completed);
        puzzleComplete.SetActive(puzzle.completed);
    }

}
