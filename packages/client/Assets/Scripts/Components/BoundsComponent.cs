using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class BoundsComponent : MUDComponent
{

    public static bool InBounds(int x, int y) {return x >= left && x <= right && y <= up && y >= down;}
    public static int left, right, up, down;
    protected override void UpdateComponent(mud.Client.IMudTable table, UpdateEvent eventType) {

        BoundsTable bounds = (BoundsTable)table;

        left = (int)bounds.left;
        right = (int)bounds.right;
        up = (int)bounds.up;
        down = (int)bounds.down;

    }
}
