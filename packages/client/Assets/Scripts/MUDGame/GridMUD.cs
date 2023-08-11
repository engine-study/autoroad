using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using mud.Unity;
using IWorld.ContractDefinition;
using DefaultNamespace;

public class GridMUD : MonoBehaviour {
    public static GridMUD Instance;
    public static Dictionary<string, MUDComponent> Grid { get { return Instance.positionDictionary; } }

    [Header("Grid")]
    [SerializeField] PositionComponent firstComponent;
    [SerializeField] List<MUDComponent> positions;
    [SerializeField] List<PositionComponent> componentsAt;
    TableManager positionTable;
    private Dictionary<string, MUDComponent> positionDictionary = new Dictionary<string, MUDComponent>();
    private Dictionary<MUDComponent, string> componentDictionary = new Dictionary<MUDComponent, string>();

    
    void Awake() {
        Instance = this;
        positions = new List<MUDComponent>();
        TableManager.OnTableToggle += Init;
        CursorMUD.OnGridPosition += UpdateComponents;
    }

    void OnDestroy() {
        Instance = null;
        TableManager.OnTableToggle -= Init;
        CursorMUD.OnGridPosition -= UpdateComponents;

        if (positionTable) {
            positionTable.OnComponentToggle -= AddPosition;
        }

        for (int i = 0; i < positions.Count; i++) {
            positions[i].OnUpdatedInfo -= UpdatePosition;
        }
    }

    void UpdateComponents(Vector3 newPos) {
        componentsAt = GetComponentsAtPosition((int)newPos.x, (int)newPos.z);
        firstComponent = componentsAt.Count > 0 ? componentsAt[0] : null;
    }

    static List<PositionComponent> GetComponentsAtPosition(int x, int y) {

        var ds = NetworkManager.Instance.ds;
        var allComponentsAtPosition = new Query().In(PositionTable.ID).In(PositionTable.ID, new Condition[] { Condition.Has("x", System.Convert.ToInt64(x)), Condition.Has("y", System.Convert.ToInt64(y)) });
        var recordsWithPosition = ds.RunQuery(allComponentsAtPosition);

        List<PositionComponent> components = new List<PositionComponent>();
        foreach(Record r in recordsWithPosition) {
            PositionComponent pos = TableManager.FindComponent<PositionComponent>(r.key);

            if(pos == null) {
                Debug.LogError("Could not find entity");
                continue;
            }
            components.Add(pos);
        }

        return components;
    }

    public static MUDEntity GetEntityAt(Vector3 newPos) {
        // MUDComponent c; Grid.TryGetValue(newPos.ToString(), out c); return c?.Entity; 
        List<PositionComponent> comps = GetComponentsAtPosition((int)newPos.x, (int)newPos.z);
        return comps.Count > 0 ? comps[0].Entity : null;
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

        //only listen to onchain updates
        if(info.UpdateSource != UpdateSource.Onchain) {
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

        //if we deleted the position, do not add it back 
        if(info.UpdateType == UpdateType.DeleteRecord) {
            return;
        }

        // Store the position in the dictionary
        if (positionDictionary.ContainsKey(position)) {
            positionDictionary[position] = component;
        } else {
            positionDictionary.Add(position, component);
        }

    }

    void OnDrawGizmosSelected() {
        if(!Application.isPlaying) {
            return;
        }

        foreach(PositionComponent p in componentsAt) {
            Gizmos.DrawWireSphere(p.Pos, .1f);
        }

    }


}
