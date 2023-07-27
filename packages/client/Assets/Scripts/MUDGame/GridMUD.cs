using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class GridMUD : MonoBehaviour {
    public static GridMUD Instance;
    public static Dictionary<string, MUDComponent> Grid { get { return Instance.positionDictionary; } }

    [Header("Grid")]
    [SerializeField] private List<MUDComponent> positions;
    TableManager positionTable;
    private Dictionary<string, MUDComponent> positionDictionary = new Dictionary<string, MUDComponent>();
    private Dictionary<MUDComponent, string> componentDictionary = new Dictionary<MUDComponent, string>();

    public static MUDEntity GetEntityAt(Vector3 newPos) { MUDComponent c; Grid.TryGetValue(newPos.ToString(), out c); return c?.Entity; }

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
            positions[i].OnUpdatedInfo -= UpdatePosition;
        }
    }

    void Init(bool toggle, TableManager newTable) {
        if (toggle && newTable.ComponentType == typeof(PositionComponent)) {
            positionTable = newTable;
            positionTable.OnComponentToggle += AddPosition;
        }
    }

    void AddPosition(bool toggle, MUDComponent newPos) {
        if (toggle) {
            positions.Add(newPos);
            newPos.OnUpdatedInfo += UpdatePosition;
        } else {

        }
    }
    void UpdatePosition(MUDComponent c, UpdateInfo info) {

        PositionComponent component = c as PositionComponent;

        if (component == null) {
            Debug.LogError("Not a position", this);
            return;
        }

        string position = component.PosLayer.ToString();
        string oldPosition = componentDictionary.ContainsKey(component) ? componentDictionary[component] : "";

        //Remove the old position from the dictionary
        if(string.IsNullOrEmpty(oldPosition) == false) {
            componentDictionary[component] = position;

            if(positionDictionary.ContainsKey(oldPosition) && positionDictionary[oldPosition] == component) {
                positionDictionary.Remove(oldPosition);
            }

        } else {
            componentDictionary.Add(component, position);
        }

        // Store the position in the dictionary
        if (positionDictionary.ContainsKey(position)) {
            positionDictionary[position] = component;
        } else {
            positionDictionary.Add(position, component);
        }

    }



}
