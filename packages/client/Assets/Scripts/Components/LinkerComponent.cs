using UnityEngine;
using mudworld;
using mud;
using System;
using System.Collections;

public class LinkerComponent : MUDComponent {


    [Header("Link")]
    public SPLine line;
    public bool alwaysVisible;

    [Header("Debug")]
    [SerializeField] string targetKey;
    [SerializeField] MUDEntity targetEntity;
    [SerializeField] PositionSync posSync;

    [SerializeField] PositionSync ourPos;
    Coroutine start;

    protected override void Init(SpawnInfo si) {
        base.Init(si);
        if(line) line.Toggle(false);
    }

    protected override void PostInit() {
        base.PostInit();
    
        ourPos = Entity.GetRootComponent<PositionSync>();
        if(ourPos == null) {Debug.LogError("We don't have pos", this); return;}

        if(alwaysVisible) {

        }

    }

    protected override void OnEnable() {
        base.OnEnable();

        if(!string.IsNullOrEmpty(targetKey)) {
            CreateLink();
        }
    }

    void CreateLink() {
        if(start != null) {StopCoroutine(start);}
        start = StartCoroutine(SetCoroutine());
    }
    
    protected virtual string SetValue(MUDTable update) {return (string)MUDTable.GetRecord(Entity.Key, MUDTableType)?.RawValue["value"];}
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {

        targetKey = SetValue(update);
        targetEntity = MUDWorld.FindEntity(targetKey);

        if(gameObject.activeInHierarchy) {
            CreateLink();
        }
    
    }

    IEnumerator SetCoroutine() {
        while(targetEntity == null) { targetEntity = MUDWorld.FindEntity(targetKey); yield return new WaitForSeconds(1f);}
        ToggleTarget(true, targetEntity);
        start = null;
    }

    public void ToggleTarget(bool toggle, MUDEntity entity) {

        //disable old target
        if(!toggle) {
            if(entity != null) {
                targetEntity.OnLoaded -= Setup;
                targetEntity = null;
            }
            return;
        }

        //turn off old
        if(targetEntity != null && entity != targetEntity) {ToggleTarget(false, targetEntity);}

        targetEntity = entity;
        
        if(targetEntity.Loaded) {Setup();}
        else{targetEntity.OnLoaded += Setup;}
    }   

    public void Setup() {

        targetEntity.OnLoaded -= Setup;

        posSync = targetEntity?.GetRootComponent<PositionSync>();
        
        if(posSync == null) {Debug.LogError("They don't have pos", this); return;}
        if(ourPos == null) {Debug.LogError("We don't have a position"); return;}
        
        if(line) line.SetTarget(ourPos.Target, posSync.Target);
        if(alwaysVisible) {
            line.Toggle(true);
        }
    }

    public void Hover(bool toggle) {
        
        if(alwaysVisible) {
            return;
        }
        
        if(line) {
            if(toggle) {
                if(!ourPos || !posSync) {return;}
                line.Toggle(true);
            } else {
                line.Toggle(false);
            }
        }
    }

}
