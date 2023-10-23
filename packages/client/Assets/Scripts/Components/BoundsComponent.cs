using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mudworld;
using mud;

public class BoundsComponent : MUDComponent
{

    public static bool OnBounds(int x, int y) {return x >= Left && x <= Right && y <= Up && y >= Down;}

    public static int Left, Right, Up, Down;
    public static BoundsComponent Instance;

    [Header("Bounds")]
    [SerializeField] Borders borders;
    [SerializeField] Vector4 borderVector;

    protected override void Awake() {
        base.Awake();
        borders.gameObject.SetActive(false);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        Instance = null;
    }

    public static void ShowBorder() {if(Instance) {Instance.borders.gameObject.SetActive(false); Instance.borders.gameObject.SetActive(true);}}

    protected override IMudTable GetTable() {return new BoundsTable();}
    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {

        Instance = this;

        BoundsTable bounds = (BoundsTable)table;

        Left = (int)bounds.Left;
        Right = (int)bounds.Right;
        Up = (int)bounds.Up;
        Down = (int)bounds.Down;

        borderVector = new Vector4(Left, Right, Up, Down);
        borders.SetBorder(borderVector);
    }
}
