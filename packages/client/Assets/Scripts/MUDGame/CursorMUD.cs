using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class CursorMUD : MonoBehaviour {
    public static CursorMUD Instance;
    public static Vector3 WorldPos { get { return Instance.mousePos; } }
    public static Vector3 GridPos { get { return Instance.gridPos; } }
    public static Entity Entity { get { return Instance.hover; } }
    public static PositionComponent Position { get { return Instance.pos; } }
    public static System.Action<Entity> OnHoverEntity;
    public static System.Action<Entity> OnLeaveEntity;
    public static System.Action<Vector3> OnGridPosition;
    public static System.Action<Vector3> OnUpdateCursor;

    [Header("Cursor")]
    public bool grid;
    public Transform visuals;

    [Header("Debug")]
    [SerializeField] Vector3 rawMousePos;
    [SerializeField] Vector3 mousePos, lastPos;
    [SerializeField] Vector3 gridPos, lastGridPos;
    [SerializeField] Entity hover, lastHover;
    [SerializeField] PositionComponent pos;


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
        lastPos = rawMousePos;
        rawMousePos = SPInput.MousePlanePos;

        if (grid) {

            lastGridPos = gridPos;

            gridPos = new Vector3(Mathf.Round(rawMousePos.x), Mathf.Round(rawMousePos.y), Mathf.Round(rawMousePos.z));
            mousePos = gridPos;

            if (gridPos != lastGridPos) {
                if (OnGridPosition != null)
                    OnGridPosition.Invoke(gridPos);
            }

        } else {
            mousePos = Vector3.MoveTowards(visuals.position, rawMousePos, 50f * Time.deltaTime);
        }

        visuals.position = mousePos;

        if (lastPos != rawMousePos) {
            OnUpdateCursor?.Invoke(rawMousePos);
        }

        if (mousePos != lastPos) {
            UpdateHover();
        }
    }

    void UpdateHover() {

        hover = GetEntityFromRadius(mousePos + Vector3.up * .5f, .25f);

        if (lastHover != hover) {

            pos = hover != null && hover is MUDEntity ? (hover as MUDEntity).GetMUDComponent<PositionComponent>() : null;

            OnLeaveEntity?.Invoke(lastHover);
            OnHoverEntity?.Invoke(hover);
        }

        lastHover = hover;

    }


    Collider[] hits;
    public Entity GetEntityFromRadius(Vector3 position, float radius) {
        if (hits == null) { hits = new Collider[10]; }

        int amount = Physics.OverlapSphereNonAlloc(position, radius, hits, LayerMask.NameToLayer("Nothing"), QueryTriggerInteraction.Ignore);
        int selectedItem = -1;
        float minDistance = 999f;
        Entity bestItem = null;
        List<Entity> entities = new List<Entity>();

        for (int i = 0; i < amount; i++) {
            Entity checkItem = hits[i].GetComponentInParent<Entity>();

            if (!checkItem)
                continue;

            entities.Add(checkItem);

            float distance = Vector3.Distance(position, hits[i].ClosestPoint(position));
            if (distance < minDistance) {
                minDistance = distance;
                selectedItem = i;
                bestItem = checkItem;
            }
        }

        return bestItem;
    }

    public Entity[] GetEntitiesFromRadius(Vector3 position, float radius) {

        if (hits == null) { hits = new Collider[10]; }

        int amount = Physics.OverlapSphereNonAlloc(position, radius, hits, LayerMask.NameToLayer("Nothing"), QueryTriggerInteraction.Ignore);
        int selectedItem = -1;
        float minDistance = 999f;
        Entity bestItem = null;
        List<Entity> entities = new List<Entity>();

        for (int i = 0; i < amount; i++) {
            Entity checkItem = hits[i].GetComponentInParent<Entity>();

            if (!checkItem)
                continue;

            entities.Add(checkItem);

            float distance = Vector3.Distance(position, checkItem.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                selectedItem = i;
                bestItem = checkItem;
            }
        }

        // return bestItem;

        return entities.ToArray();

    }
}
