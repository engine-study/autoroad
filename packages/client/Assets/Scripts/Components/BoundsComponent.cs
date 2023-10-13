using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mudworld;
using mud;

public class BoundsComponent : MUDComponent
{


    public static bool OnWorld(mud.MUDEntity entity, Vector3 pos) {
        if (entity.GetMUDComponent<PlayerComponent>()) { return MapConfigComponent.OnMap(pos); }
        else return OnBounds(pos); 
    }

    public static bool OnBounds(Vector3 pos) { return OnBounds(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z)); }
    public static bool OnBounds(int x, int y) {return x >= Left && x <= Right && y <= Up && y >= 0;}
    public static int Left, Right, Up, Down;
    public static BoundsComponent Instance;

    [Header("Bounds")]
    [SerializeField] Borders borders;
    [SerializeField] Vector4 borderVector;

    protected override void Awake() {
        base.Awake();

        Instance = this;
        borders.gameObject.SetActive(false);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        Instance = null;
    }

    public static void ShowBorder() {if(Instance) {Instance.borders.gameObject.SetActive(false); Instance.borders.gameObject.SetActive(true);}}

    protected override IMudTable GetTable() {return new BoundsTable();}
    protected override void UpdateComponent(mud.IMudTable table, UpdateInfo newInfo) {

        BoundsTable bounds = (BoundsTable)table;

        Left = (int)bounds.left;
        Right = (int)bounds.right;
        Up = (int)bounds.up;
        Down = (int)bounds.down;

        borderVector = new Vector4(Left, Right, Up, Down);
        borders.SetBorder(borderVector);
    }
}
