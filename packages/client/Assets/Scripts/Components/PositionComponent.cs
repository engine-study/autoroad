using UnityEngine;
using mudworld;
using mud;
using System.Collections.Generic;
using System;
public class PositionComponent : MUDComponent {

    public Action<bool> OnPositionToggle;
    public Vector3Int PosInt { get { return posInt; } }
    public Vector3 Pos { get { return position3D; } }
    public Vector3 LastPos { get { return lastPos; } }
    public Vector3 PosLayer { get { return position3DLayer; } }
    public Transform Target {
        get {   
            if(syncer == null) {syncer = Entity.GetRootComponent<PositionSync>();}
            return syncer?.Target;
        }
    }

    public bool IsVisible {get { return isVisible; } }
    public static object[] PositionToOptimistic(Vector3 newPos) { return new object[] { System.Convert.ToInt32(newPos.x), System.Convert.ToInt32(newPos.z), System.Convert.ToInt32(newPos.y) }; }
    public static object[] PositionToTransaction(Vector3 newPos) { return new object[] { System.Convert.ToInt32(newPos.x), System.Convert.ToInt32(newPos.z)}; }
    public static float PositionToMile(Vector3 position) {return Mathf.Floor(position.z / (float)MapConfigComponent.Height);}

    public static bool OnWorldOrMap(MUDEntity e, Vector3 pos, bool showFX = false) {
        bool legal = MapConfigComponent.OnWorldOrMap(e, pos);
        
        if(!legal && showFX) BoundsComponent.ShowBorder();
        return legal;
    }

    public static bool OnMap(Vector3 pos, bool showFX = false) {
        bool legal = MapConfigComponent.OnMap(pos);
        if(!legal && showFX) BoundsComponent.ShowBorder();
        return legal;
    }

    public static bool OnWorld(Vector3 pos, bool showFX = false) {
        bool legal = MapConfigComponent.OnWorld(pos);
        if(!legal && showFX) BoundsComponent.ShowBorder();
        return legal;
    }

    [Header("Position")]
    [SerializeField] bool isVisible = true;
    [SerializeField] int layer = 0;
    [SerializeField] Vector2 position2D;
    [SerializeField] Vector3 position3D;
    [SerializeField] Vector3Int posInt;
    [SerializeField] Vector3 position3DLayer;
    [SerializeField] Vector3 lastPos;
    [SerializeField] PositionSync syncer;
    [SerializeField] ActionComponent action;
    // [SerializeField] AnimationComponent anim;

    protected override void Init(SpawnInfo newSpawnInfo) {
        base.Init(newSpawnInfo);
        // anim = MUDWorld.FindOrMakeComponent<AnimationComponent>(newSpawnInfo.Entity.Key);
        isVisible = true;
        action = MUDWorld.FindOrMakeComponent<ActionTable, ActionComponent>(newSpawnInfo.Entity);
    }

    protected override void PostInit() {
        base.PostInit();

        OnPositionToggle?.Invoke(IsVisible);

    }

    protected override MUDTable GetTable() {return new PositionTable();}
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {

        lastPos = position3D;

        PositionTable table = update as PositionTable;

        layer = (int)table.Layer;

        position2D = new Vector2((int)table.X, (int)table.Y);
        position3D = new Vector3(position2D.x, 0f, position2D.y);
        position3DLayer = new Vector3(position2D.x, layer, position2D.y);

        posInt = new Vector3Int((int)position3D.x, 0, (int)position3D.z);

        transform.position = position3D;

        //check our position is on the map
        bool newVisible = newInfo.UpdateType != UpdateType.DeleteRecord && layer >= -1;
        if(newVisible != isVisible) {
            isVisible = newVisible;

            if(Loaded) {
                OnPositionToggle?.Invoke(isVisible);
            }
        }
        

    }

}
