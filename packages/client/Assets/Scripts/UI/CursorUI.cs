using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorUI : MonoBehaviour
{
    public static CursorUI Instance;
    public static Entity CursorEntity {get{return Instance.entity;}}
    public static Ground CursorGround {get{return Instance.terrain;}}
    public static System.Action CursorUpdate;

    [Header("Cursor")]
    public StatsUI stats;
    public InfoUI info;

    [Header("Debug")]
    public Entity entity;
    public Ground terrain;

    void Awake() {
        Instance = this;
        MUDCursor.OnHover += UpdateHover;
        MUDCursor.OnUpdateCursor += OnCursorPosition;
    }

    void OnDestroy() {
        Instance = null;
        MUDCursor.OnHover -= UpdateHover;
        MUDCursor.OnUpdateCursor -= OnCursorPosition;
    }

    void OnCursorPosition(Vector3 newPos) {
        
        // Entity newEntity = MapGenerator.GetEntityAtPosition((Vector3)newPos);
        // entity = newEntity;
        entity = null;

        if(entity == null) {
            stats.ToggleWindow(false);
        } else {
            stats.ToggleWindow(true);
            stats.UpdateEntity(entity);
        }

        // Ground newTerrain = MapGenerator.GetTerrainAtPosition((Vector3)newPos);
        terrain = null;
        
        if(terrain == null) {
            info.ToggleWindow(false);
        } else {
            info.ToggleWindow(true);
            info.UpdateEntity(terrain);
        }

        CursorUpdate?.Invoke();

    }

    void UpdateHover(Entity newEntity) {
      
    }



}
