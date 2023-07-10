using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class CursorUI : MonoBehaviour
{
    public static CursorUI Instance;
    public static SPBase CursorObject { get { return Instance.baseObject; } }
    public static System.Action CursorUpdate;

    [Header("Cursor")]
    public ActorUI actor;
    public InfoUI info;

    [Header("Debug")]
    public SPBase baseObject;
    public MUDEntity entity;

    void Awake()
    {

        Instance = this;

        info.ToggleWindowClose();
        actor.ToggleWindowClose();

        CursorMUD.OnHoverEntity += UpdateHoverEntity;
        CursorMUD.OnUpdateCursor += OnCursorPosition;
    }

    void OnDestroy()
    {
        Instance = null;
        CursorMUD.OnHoverEntity -= UpdateHoverEntity;
        CursorMUD.OnUpdateCursor -= OnCursorPosition;
    }

    void OnCursorPosition(Vector3 newPos)
    {

        info.UpdateCoordinate((int)newPos.x, (int)newPos.z);
        CursorUpdate?.Invoke();

    }

    //CHECK FOR INTERACTIONS
    void UpdateHoverEntity(Entity newEntity)
    {

        SPBase newObject = newEntity != null ? newEntity.GetComponentInChildren<SPBase>() : null;
        baseObject = newObject;

        actor.ToggleWindow(baseObject != null);
        info.ToggleWindow(baseObject != null);

        if (baseObject != null)
        {
            info.UpdateInfo(newEntity);
            actor.UpdateObject(baseObject);
        }

        CursorUpdate?.Invoke();
    }



}
