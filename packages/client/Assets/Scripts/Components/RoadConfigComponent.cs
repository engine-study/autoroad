using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class RoadConfigComponent : MUDComponent
{
    public static int Width;
    public static int Height;
    public static int Left;
    public static int Right;

    [SerializeField] private int width,height,left,right;

    protected override IMudTable GetTable() {return new RoadConfigTable();}
    protected override void UpdateComponent(mud.Client.IMudTable table, UpdateInfo newInfo) {
        RoadConfigTable update = (RoadConfigTable)table;

        width = (int)update.width;
        height = (int)update.height;        
        left = (int)update.left;
        right = (int)update.right;

        Width = width;
        Height = height;        
        Left = left;
        Right = right;
    }


}
