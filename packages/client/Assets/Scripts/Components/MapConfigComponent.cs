using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class MapConfigComponent : MUDComponent
{

    public static MapConfigComponent Instance;
    public static bool OnMap(int x, int y) {return x >= -SpawnWidth && x <= SpawnWidth && y <= BoundsComponent.Up && y >= 0;}

    public static int PlayWidth, SpawnWidth;
    [SerializeField] private int playArea;
    [SerializeField] private int spawnArea;
    protected override IMudTable GetTable() {return new MapConfigTable();}
    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {

        Instance = this;

        MapConfigTable update = table as MapConfigTable;

        playArea = (int)update.playArea;
        spawnArea = (int)update.spawnArea;

        PlayWidth = playArea;
        SpawnWidth = spawnArea;

    }
}
