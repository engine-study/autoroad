using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class BoundsComponent : MUDComponent
{

    public static bool InBounds(int x, int y) {return x >= Left && x <= Right && y <= Up && y >= Down;}
    public static int Left, Right, Up, Down;
    protected override void UpdateComponent(mud.Client.IMudTable table, UpdateInfo newInfo) {

        BoundsTable bounds = (BoundsTable)table;

        Left = (int)bounds.left;
        Right = (int)bounds.right;
        Up = (int)bounds.up;
        Down = (int)bounds.down;

    }
}
