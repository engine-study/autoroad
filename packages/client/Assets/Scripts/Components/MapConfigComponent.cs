using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class MapConfigComponent : MUDComponent
{

    public static int PlayWidth, SpawnWidth;
    [SerializeField] private int playArea;
    [SerializeField] private int spawnArea;
    protected override IMudTable GetTable() {return new MapConfigTable();}
    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {
        MapConfigTable update = table as MapConfigTable;

        playArea = (int)update.playArea;
        spawnArea = (int)update.spawnArea;

        PlayWidth = playArea;
        SpawnWidth = spawnArea;

    }
}
