using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class BoundsComponent : MUDComponent
{


    public static bool OnWorld(mud.Client.MUDEntity entity, Vector3 pos) {
        if (entity.GetMUDComponent<PlayerComponent>()) { return MapConfigComponent.OnMap(pos); }
        else return OnBounds(pos); 
    }

    public static bool OnBounds(Vector3 pos) { return OnBounds(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z)); }
    public static bool OnBounds(int x, int y) {return x >= Left && x <= Right && y <= Up && y >= 0;}
    public static int Left, Right, Up, Down;
    public static BoundsComponent Instance;

    [Header("Bounds")]
    [SerializeField] GameObject borderVisuals;
    [SerializeField] Transform front, back;
    [SerializeField] Transform left, right;
    [SerializeField] private Vector4 boundVector;

    protected override void Awake() {
        base.Awake();

        Instance = this;
        borderVisuals.SetActive(false);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        Instance = null;
    }

    public static void ShowBorder() {if(Instance) Instance.borderVisuals.SetActive(false); Instance.borderVisuals.SetActive(true);}

    protected override IMudTable GetTable() {return new BoundsTable();}
    protected override void UpdateComponent(mud.Client.IMudTable table, UpdateInfo newInfo) {

        BoundsTable bounds = (BoundsTable)table;

        Left = (int)bounds.left;
        Right = (int)bounds.right;
        Up = (int)bounds.up;
        Down = (int)bounds.down;

        boundVector = new Vector4(Left, Right, Up, Down);

        front.localPosition = Vector3.forward * (Up + .5f);

        front.localScale = Vector3.one + Vector3.right * (Right-Left);
        back.localScale = Vector3.one + Vector3.right * (Right-Left);

        left.localPosition = Vector3.right * (Left - .5f) - Vector3.forward * .5f;
        right.localPosition = Vector3.right * (Right + .5f) - Vector3.forward * .5f;

        left.localScale = Vector3.one + Vector3.forward * (Up);
        right.localScale = Vector3.one + Vector3.forward * (Up);

    }
}
