using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class CursorUI : MonoBehaviour {
    public static CursorUI Instance;
    public static SPBase CursorObject { get { return Instance.baseObject; } }
    public static System.Action CursorUpdate;

    [Header("Cursor")]
    public ActorUI actor;
    public InfoUI info;

    [Header("Debug")]
    public SPBase baseObject;
    public MUDEntity entity;

    void Awake() {

        Instance = this;

        actor.ToggleWindowClose();

        CursorMUD.OnHoverEntity += UpdateHoverEntity;
        CursorMUD.OnGridPosition += OnCursorPosition;
    }

    void OnDestroy() {
        Instance = null;
        CursorMUD.OnHoverEntity -= UpdateHoverEntity;
        CursorMUD.OnGridPosition -= OnCursorPosition;
    }

    void OnCursorPosition(Vector3 newPos) {

        info.UpdateCoordinate((int)newPos.x, (int)newPos.z);
        CursorUpdate?.Invoke();

    }

    //CHECK FOR INTERACTIONS
    void UpdateHoverEntity(MUDEntity newEntity) {

        entity = newEntity;
        
        SPCursorTexture.UpdateCursor(newEntity == null ? SPCursorState.Default : SPCursorState.Hand);

        info.SetEntity(newEntity);
        actor.SetEntity(newEntity);

        CursorUpdate?.Invoke();
    }



}
