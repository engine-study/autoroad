using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using UniRx;
using IWorld.ContractDefinition;
using mudworld;
using System.Linq;

public class GridMUD : MonoBehaviour {
    public static GridMUD Instance;
    public static Dictionary<string, MUDComponent> Grid { get { return Instance.positionDictionary; } }

    [Header("Grid")]
    public bool useQuery = false; 
    [SerializeField] Vector3 position;
    [SerializeField] PositionComponent firstComponent;
    [SerializeField] List<MUDComponent> positions;
    [SerializeField] List<PositionComponent> componentsAt;
    [SerializeField] int recordsFound;
    TableManager positionTable;
    private Dictionary<string, MUDComponent> positionDictionary = new Dictionary<string, MUDComponent>();
    private Dictionary<MUDComponent, string> componentDictionary = new Dictionary<MUDComponent, string>();

    void Awake() {
        Instance = this;
        positions = new List<MUDComponent>();
        // TableDictionary.OnTableToggle += Init;
        NetworkManager.OnInitialized += Setup;
    }

    void Setup() {
        CursorMUD.OnGridPosition += UpdateComponents;
    }

    void OnDestroy() {

        Instance = null;
        // TableDictionary.OnTableToggle -= Init;
        CursorMUD.OnGridPosition -= UpdateComponents;

        if (positionTable) {
            positionTable.OnComponentSpawned -= AddPosition;
        }

        for (int i = 0; i < positions.Count; i++) {
            positions[i].OnUpdatedInfo -= UpdatePosition;
        }
    }

    void UpdateComponents(Vector3 newPos) {
        if(GameState.GamePlaying == false) {return;}
        position = newPos;
        componentsAt = GetComponentsAtPosition(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z), Mathf.RoundToInt(position.y));
        firstComponent = componentsAt.Count > 0 ? componentsAt[0] : null;
    }

    List<PositionComponent> GetComponents(int x, int y, int layer) {

        Condition[] conditions = new Condition[3];
        conditions[0] = Condition.Has("x", System.Convert.ToInt32(x)); 
        conditions[1] = Condition.Has("y", System.Convert.ToInt32(y));
        conditions[2] = Condition.Has("layer", System.Convert.ToInt32(layer));

        // Debug.Log("All positions");

        // var allPositions = new Query().In(PositionTable.Table);
        // var r1 = NetworkManager.Datastore.RunQuery(allPositions);
        // Debug.Log(r1.Count());

        // Debug.Log("All positions in positions");

        // var allPosInPos = new Query().In(PositionTable.Table).In(PositionTable.Table);
        // var r2 = NetworkManager.Datastore.RunQuery(allPosInPos);
        // Debug.Log(r2.Count());

        // Debug.Log("All positions in positions with condition");

        var allPosCondition = new Query().In(PositionTable.Table).In(PositionTable.Table, conditions );
        var r3 = NetworkManager.Datastore.RunQuery(allPosCondition);
        // Debug.Log(r3.Count());

        // Debug.Log("All positions with condition");

        // var pCon = new Query().In(PositionTable.Table, conditions );
        // var r4 = NetworkManager.Datastore.RunQuery(pCon);
        // Debug.Log(r4.Count());


        List<PositionComponent> components = new List<PositionComponent>();
        
        foreach(RxRecord r in r3) {
            recordsFound++;
            PositionComponent pos = MUDWorld.FindComponent<PositionTable, PositionComponent>(r.Key);
            if(pos == null) {Debug.LogError("Could not find entity");continue;}
            components.Add(pos);
        }

        return components;
    }

    static List<PositionComponent> GetComponentsAtPosition(float x, float y, float layer) {
        return Instance.GetComponents(Mathf.RoundToInt(x),Mathf.RoundToInt(y),Mathf.RoundToInt(layer));
    }

    public static MUDEntity GetEntityAt(Vector3 newPos) {

        if(Instance.useQuery) {
            List<PositionComponent> comps = GetComponentsAtPosition(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.z), Mathf.RoundToInt(newPos.y));
            return comps.Count > 0 ? comps[0].Entity : null;
        } else {
            MUDComponent c; Grid.TryGetValue(newPos.ToString(), out c); return c?.Entity; 
        }

    }


    void Init(bool toggle, TableManager newTable) {
        if (toggle && newTable.ComponentType == typeof(PositionComponent)) {
            positionTable = newTable;
            positionTable.OnComponentSpawned += AddPosition;
        }
    }

    void AddPosition(MUDComponent newPos) {
        positions.Add(newPos);
        newPos.OnUpdatedInfo += UpdatePosition;
    }
    
    void UpdatePosition(MUDComponent c, UpdateInfo info) {

        PositionComponent component = c as PositionComponent;

        if (component == null) {
            Debug.LogError("Not a position", this);
            return;
        }

        //only listen to onchain updates
        if(info.Source != UpdateSource.Onchain) {
            return;
        }

        string newPosition = component.PosInt.ToString();
        string oldPosition = componentDictionary.ContainsKey(component) ? componentDictionary[component] : "";

        //Remove the old position from the dictionary
        if(string.IsNullOrEmpty(oldPosition) == false) {

            if(positionDictionary.ContainsKey(oldPosition)) {
                Debug.Assert(positionDictionary[oldPosition] == component, "Different component found at old position", component);
                positionDictionary.Remove(oldPosition);
            }

            componentDictionary[component] = newPosition;

        } else {
            componentDictionary.Add(component, newPosition);
        }

        //if we deleted the position, do not add it back 
        if(info.UpdateType == UpdateType.DeleteRecord) {
            return;
        }

        // Store the position in the dictionary
        if (positionDictionary.ContainsKey(newPosition)) {
            Debug.Assert(positionDictionary[newPosition] == component, "New position not empty", positionDictionary[newPosition]);
        } else {
            positionDictionary.Add(newPosition, component);
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
