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

    [SerializeField] private int width,height,left,right;
    public static bool OnRoad(int x, int y) {return x >= Left && x <= Right;}

    protected override IMudTable GetTable() {return new RoadConfigTable();}
    protected override void UpdateComponent(mud.IMudTable table, UpdateInfo newInfo) {

        Instance = this;
        
        RoadConfigTable update = (RoadConfigTable)table;

        width = (int)update.width;
        left = (int)update.left;
        right = (int)update.right;

        Width = width;
        Left = left;
        Right = right;
    }


}
