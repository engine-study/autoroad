using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class PositionComponent : MUDComponent {

    public Vector3Int PosInt { get { return posInt; } }
    public Vector3 Pos { get { return position3D; } }
    public Vector3 PosLayer { get { return position3DLayer; } }

    public static object[] PositionToOptimistic(Vector3 newPos) { return new object[] { System.Convert.ToInt32(newPos.x), System.Convert.ToInt32(newPos.z), System.Convert.ToInt32(newPos.y) }; }
    public static object[] PositionToTransaction(Vector3 newPos) { return new object[] { System.Convert.ToInt32(newPos.x), System.Convert.ToInt32(newPos.z)}; }

    [Header("Position")]
    [SerializeField] private int layer = 0;
    [SerializeField] private Vector2 position2D;
    [SerializeField] private Vector3 position3D;
    [SerializeField] private Vector3Int posInt;
    [SerializeField] private Vector3 position3DLayer;

    protected override IMudTable GetTable() {return new PositionTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        PositionTable table = update as PositionTable;

        layer = (int)table.layer;

        position2D = new Vector2((int)table.x, (int)table.y);
        position3D = new Vector3(position2D.x, 0f, position2D.y);
        position3DLayer = new Vector3(position2D.x, layer, position2D.y);

        posInt = new Vector3Int((int)position3D.x, 0, (int)position3D.z);

        transform.position = position3D;

    }

}
