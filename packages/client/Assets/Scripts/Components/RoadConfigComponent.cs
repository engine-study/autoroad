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
    public int width,height,left,right;

    protected override void UpdateComponent(mud.Client.IMudTable table, UpdateInfo newInfo) {
        RoadConfigTable update = (RoadConfigTable)table;
        width = (int)update.width;
        height = (int)update.height;        
        left = (int)update.left;
        right = (int)update.right;
        Width = (int)update.width;
        Height = (int)update.height;        
        Left = (int)update.left;
        Right = (int)update.right;
    }


}
