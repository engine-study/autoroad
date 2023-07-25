using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class GridMUD : MonoBehaviour {
    public static GridMUD Instance;
    public static Dictionary<Vector3, MUDComponent> Grid { get { return Instance.componentPositions; } }

    [Header("Grid")]
    [SerializeField] private List<MUDComponent> positions;
    TableManager positionTable;
    private Dictionary<Vector3, MUDComponent> componentPositions = new Dictionary<Vector3, MUDComponent>();

    public static MUDEntity GetEntityAt(Vector3 newPos) {MUDComponent c; Grid.TryGetValue(newPos, out c); return c?.Entity;}

    void Awake() {
        Instance = this;
        positions = new List<MUDComponent>();
        TableManager.OnTableToggle += Init;
    }

    void OnDestroy() {
        Instance = null;
        TableManager.OnTableToggle -= Init;
        if (positionTable) {
            positionTable.OnComponentToggle -= AddPosition;
        }

        for (int i = 0; i < positions.Count; i++) {
            positions[i].OnUpdatedInfo -= AddToTable;
        }
    }

    void Init(bool toggle, TableManager newTable) {
        if (toggle && newTable.ComponentType == typeof(PositionComponent)) {
            positionTable = newTable;
            positionTable.OnComponentToggle += AddPosition;
        }
    }

    void AddPosition(bool toggle, MUDComponent newPos) {
        if(toggle) {
            positions.Add(newPos);
            newPos.OnUpdatedInfo += AddToTable;
        }
    }
    void AddToTable(MUDComponent c, UpdateInfo info) {
        PositionComponent p = c as PositionComponent;
        if (p != null) {
            // Store the position in the dictionary
            if (componentPositions.ContainsKey(p.PosLayer)) {
                componentPositions[p.PosLayer] = p;
            } else {
                componentPositions.Add(p.PosLayer, p);
            }
        }
    }



}
