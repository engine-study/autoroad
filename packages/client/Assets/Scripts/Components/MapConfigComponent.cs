using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class MapConfigComponent : MUDComponent
{

    public static MapConfigComponent Instance;
    public static bool OnMap(Vector3 newPos) { return OnMap(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.z)); }
    public static bool OnMap(int x, int y) {return x >= -SpawnWidth && x <= SpawnWidth && y <= BoundsComponent.Up && y >= 0;}

    public static int Width, Height, SpawnWidth;
    [SerializeField] private int playWidth, playHeight, playSpawnWidth;
    protected override IMudTable GetTable() {return new MapConfigTable();}
    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {

        Instance = this;

        MapConfigTable update = table as MapConfigTable;

        playWidth = (int)update.PlayWidth;
        playHeight = (int)update.PlayHeight;
        playSpawnWidth = (int)update.PlaySpawnWidth;

        Width = playWidth;
        Height = playHeight;
        SpawnWidth = playSpawnWidth;

    }
}
