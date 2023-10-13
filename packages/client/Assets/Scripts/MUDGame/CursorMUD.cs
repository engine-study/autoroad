using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using System;

public class CursorMUD : MonoBehaviour {
    public static CursorMUD Instance;
    public static Transform CursorTransform { get { return Instance.visuals; } }
    public static Transform CursorTransformRaw { get { return Instance.rawTransform; } }
    public static Transform LookTarget { get { return Instance.lookTarget; } }
    public static Vector3 WorldPos { get { return Instance.mousePos; } }
    public static Vector3 GridPos { get { return Instance.gridPos; } }
    public static MUDEntity Entity { get { return Instance.hover; } }
    public static PositionComponent Position { get { return Instance.pos; } }
    public static SPBase Base { get { return Instance.baseObject; } }
    public static Action<MUDEntity> OnHoverEntity;
    public static Action<MUDEntity> OnLeaveEntity;
    public static Action<Vector3> OnGridPosition;
    public static Action<Vector3> OnCursorPosition;

    [Header("Cursor")]
    public bool grid;
    [SerializeField] private Transform visuals;
    [SerializeField] private Transform rawTransform;
    [SerializeField] private Transform lookTarget;

    [Header("Debug")]
    [SerializeField] Vector3 rawMousePos;
    [SerializeField] Vector3 mousePos, lastPos;
    [SerializeField] Vector3 gridPos, lastGridPos;
    [SerializeField] mud.MUDEntity hover, lastHover;
    [SerializeField] PositionComponent pos;
    [SerializeField] SPBase baseObject;


    void Awake() {
        Instance = this;
    }

    void OnDestroy() {
        Instance = null;
    }

    void Update() {
        UpdateMouse();
    }

    void UpdateMouse() {

        rawMousePos = SPInput.MousePlanePos;
        rawTransform.position = rawMousePos;

        if (grid) {
            gridPos = new Vector3(Mathf.Round(rawMousePos.x), Mathf.Round(rawMousePos.y), Mathf.Round(rawMousePos.z));
            mousePos = gridPos;
        } else {
            mousePos = Vector3.MoveTowards(visuals.position, rawMousePos, 50f * Time.deltaTime);
        }

        if(SPUIBase.IsPointerOverUIElement) {
            return;
        }
        
        visuals.position = mousePos;

        if (mousePos != lastPos) {
            UpdateHover();
        }

        if (lastPos != rawMousePos) {OnCursorPosition?.Invoke(rawMousePos);}
        if (gridPos != lastGridPos) {OnGridPosition?.Invoke(gridPos);}

        lastGridPos = gridPos;
        lastPos = mousePos;

    }

    public static void ForceCursorUpdate() {
        Instance.UpdateHover();
        OnCursorPosition?.Invoke(Instance.rawMousePos);
        OnGridPosition?.Invoke(Instance.gridPos);
    }

    void UpdateHover() {

        // hover = MUDHelper.GetMUDEntityFromRadius(mousePos, .1f);
        
        if(!mud.Unity.NetworkManager.Initialized)
            return;

        if(SPUIBase.IsPointerOverUIElement) {
            hover = null;
        } else {
            hover = GridMUD.GetEntityAt(mousePos);
        }

        if (lastHover != hover) {

            pos = hover != null && hover is mud.MUDEntity ? (hover as mud.MUDEntity).GetMUDComponent<PositionComponent>() : null;
            baseObject = hover != null ? hover.GetComponentInChildren<SPBase>() : null;

            OnLeaveEntity?.Invoke(lastHover);
            OnHoverEntity?.Invoke(hover);
        }

        lastHover = hover;

    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(mousePos, .25f);
    }

}
