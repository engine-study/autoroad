using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class MapConfigComponent : MUDComponent
{

    public static MapConfigComponent Instance;

    
    public static bool OnWorldOrMap(MUDEntity entity, Vector3 pos) {
        if (entity?.GetMUDComponent<PlayerComponent>()) { return OnWorld(pos); }
        else return OnMap(pos); 
    }

    //MAP
    public static bool OnMap(Vector3 pos) { return OnMap(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z)); }
    public static bool OnMap(int x, int y) {return x >= BoundsComponent.Left && x <= BoundsComponent.Right && y <= BoundsComponent.Up && y >= 0;}

    //WORLD
    public static bool OnWorld(Vector3 newPos) { return OnWorld(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.z)); }
    public static bool OnWorld(int x, int y) {return x >= -SpawnWidth && x <= SpawnWidth && y <= BoundsComponent.Up && y >= 0;}

    public static int Width, Height, SpawnWidth;
    [SerializeField] private int width, height, playSpawnWidth;
    protected override IMudTable GetTable() {return new MapConfigTable();}
    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {

        Instance = this;

        MapConfigTable update = table as MapConfigTable;

        width = (int)update.PlayWidth;
        height = (int)update.PlayHeight;
        playSpawnWidth = (int)update.PlaySpawnWidth;

        Width = width;
        Height = height;
        SpawnWidth = playSpawnWidth;

    }

    protected override void OnDestroy() {
        base.OnDestroy();
        Instance = null;
    }

}
