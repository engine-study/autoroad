using UnityEngine;
using mudworld;
using mud;
using mud;
using System;

public class PuzzleComponent : MUDComponent {

    [Header("Name")]
    public bool completed;


    protected override IMudTable GetTable() {return new PuzzleTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        PuzzleTable table = update as PuzzleTable;
        
        completed = (bool)table.complete;
    }

}
