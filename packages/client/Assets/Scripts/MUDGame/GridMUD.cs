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

    [Header("Grid")]
    public bool useQuery = false; 
    [SerializeField] Vector3 position;
    [SerializeField] PositionComponent firstComponent;
    TableManager positionTable;
    private Dictionary<string, PositionComponent> positions = new Dictionary<string, PositionComponent>();
    private Dictionary<PositionComponent, string> components = new Dictionary<PositionComponent, string>();

    void Awake() {
        Instance = this;
        TableDictionary.OnTableToggle += Init;
        NetworkManager.OnInitialized += Setup;
    }

    void Init(bool toggle, TableManager newTable) {
        if (toggle && newTable.ComponentType == typeof(PositionComponent)) {
            positionTable = newTable;
            positionTable.OnComponentSpawned += AddPosition;
            TableDictionary.OnTableToggle -= Init;
        }
    }

    void AddPosition(MUDComponent newPos) {
        UpdatePosition(newPos as PositionComponent, newPos.UpdateInfo);
        newPos.OnUpdatedInfo += UpdatePosition;
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

    }
    

    void UpdateComponents(Vector3 newPos) {

        if(GameState.GamePlaying == false) {return;}
        position = newPos;
        firstComponent = GetComponentAt(newPos);
        
    }

    public static MUDEntity GetEntityAt(Vector3 newPos) {
        return GetComponentAt(newPos)?.Entity;
    }    

    public static PositionComponent GetComponentAt(Vector3 newPos) {

        if(Instance.useQuery) {
            List<PositionComponent> comps = GetComponentsAtPosition(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.z), Mathf.RoundToInt(newPos.y));
            return comps.Count > 0 ? comps[0] : null;
        } else {
            Vector3Int round = new Vector3Int(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y), Mathf.RoundToInt(newPos.z));
            Instance.positions.TryGetValue(round.ToString(), out PositionComponent c); 
            return c; 
        }
    }
    
    static List<PositionComponent> GetComponentsAtPosition(float x, float y, float layer) {
        return Instance.GetComponents(Mathf.RoundToInt(x),Mathf.RoundToInt(y),Mathf.RoundToInt(layer));
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
            PositionComponent pos = MUDWorld.FindComponent<PositionTable, PositionComponent>(r.Key);
            if(pos == null) {Debug.LogError("Could not find entity");continue;}
            components.Add(pos);
        }

        return components;
    }

    void UpdatePosition(MUDComponent c, UpdateInfo info) {

        PositionComponent pos = c as PositionComponent;
        string key = pos.PosInt.ToString();

        //remove old pos
        if(components.TryGetValue(pos, out string oldPos)) {
            //remove old position
            positions.Remove(oldPos);
            components[pos] = key;
        } else {
            components.Add(pos, key);
        }

        //only listen to onchain updates
        if(info.Source != UpdateSource.Onchain) { return;}

        //if we deleted the position, do not add it back 
        if(info.UpdateType == UpdateType.DeleteRecord) {
            positions.Remove(key);
        } else {
            // Store the position in the dictionary
            if (positions.ContainsKey(key)) {
                positions[key] = pos;
            } else {
                positions.Add(key, pos);
            }
        }


    }

    void OnDrawGizmosSelected() {
        if(!Application.isPlaying) {
            return;
        }

        if(firstComponent) Gizmos.DrawWireSphere(firstComponent.Pos, .1f);
        

    }


}
