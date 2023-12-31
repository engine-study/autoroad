using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class RoadConfigComponent : MUDComponent
{
    public static RoadConfigComponent Instance;
    public static int Width;
    public static int Left;
    public static int Right;

    [SerializeField] private int width,left,right;
    public static bool OnRoad(Vector3 pos) {return Mathf.RoundToInt(pos.x) >= Left && Mathf.RoundToInt(pos.x) <= Right;}

    protected override MUDTable GetTable() {return new RoadConfigTable();}
    protected override void UpdateComponent(mud.MUDTable table, UpdateInfo newInfo) {

        Instance = this;
        
        RoadConfigTable update = (RoadConfigTable)table;

        width = (int)update.Width;
        left = (int)update.Left;
        right = (int)update.Right;

        Width = width;
        Left = left;
        Right = right;
    }
    
    protected override void OnDestroy() {
        base.OnDestroy();
        Instance = null;

    }


}
