using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class CursorUI : MonoBehaviour
{
    public static CursorUI Instance;
    public static SPBase CursorObject {get{return Instance.baseObject;}}
    public static System.Action CursorUpdate;

    [Header("Cursor")]
    public ActorUI actor;
    public InfoUI info;

    [Header("Debug")]
    public SPBase baseObject;
    public MUDEntity entity;

    void Awake() {
        
        Instance = this;

        info.ToggleWindowClose();
        actor.ToggleWindowClose();

        CursorMUD.OnHoverEntity += UpdateHoverEntity;
        CursorMUD.OnUpdateCursor += OnCursorPosition;
    }

    void OnDestroy() {
        Instance = null;
        CursorMUD.OnHoverEntity -= UpdateHoverEntity;
        CursorMUD.OnUpdateCursor -= OnCursorPosition;
    }

    void OnCursorPosition(Vector3 newPos) {
        

        //useless to do it this way.. need reverse mapping
        // string positionKey = MUDHelper.GetSha3ABIEncoded(newPos.x, newPos.z);
        // MUDEntity mudEntity = PositionManager.Instance.EntityHasComponent(positionKey) ? EntityDictionary.GetEntity(positionKey) : null;
        // SPBase newObject = mudEntity != null ? mudEntity.GetComponentInChildren<SPBase>() : null;

        //check the terrain and reverse mappings of units in the position table at ths position
        return;

        // INFO OF THE CURRENT GRID SLOT
        // Ground newTerrain = MapGenerator.GetTerrainAtPosition((Vector3)newPos);
        
        if(entity == null) {
            info.ToggleWindow(false);
        } else {
            info.ToggleWindow(true);
            info.UpdateInfo(null, (int)newPos.x, (int)newPos.z);
        }

        CursorUpdate?.Invoke();

    }

    //CHECK FOR INTERACTIONS
    void UpdateHoverEntity(Entity newEntity) {
        
        SPBase newObject = newEntity != null ? newEntity.GetComponentInChildren<SPBase>() : null;
        baseObject = newObject;

        if(baseObject == null) {
            actor.ToggleWindow(false);
        } else {
            actor.ToggleWindow(true);
            actor.UpdateObject(baseObject);
        }

        CursorUpdate?.Invoke();
    }



}
