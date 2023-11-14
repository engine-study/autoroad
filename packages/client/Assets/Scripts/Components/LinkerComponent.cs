using UnityEngine;
using mudworld;
using mud;
using System;
using System.Collections;

public class LinkerComponent : MUDComponent {


    [Header("Link")]
    [SerializeField] SPLine line;

    [Header("Debug")]
    [SerializeField] string targetKey;
    [SerializeField] MUDEntity targetEntity;
    [SerializeField] PositionSync posSync;

    [SerializeField] PositionSync ourPos;
    Coroutine start;

    protected override void Init(SpawnInfo si) {
        base.Init(si);
        line.Toggle(false);
    }

    protected override void PostInit() {
        base.PostInit();
    
        ourPos = Entity.GetRootComponent<PositionSync>();
        if(ourPos == null) {Debug.LogError("We don't have pos", this); return;}

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
    
    protected override MUDTable GetTable() {return new LinkerTable();}
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {

        LinkerTable table = update as LinkerTable;
        targetKey = (string)table.Value;
        targetEntity = MUDWorld.FindEntity(targetKey);

        if(gameObject.activeInHierarchy)
            CreateLink();
    
    }

    IEnumerator SetCoroutine() {
        while(targetEntity == null) { targetEntity = MUDWorld.FindEntity(targetKey); yield return new WaitForSeconds(1f);}
        ToggleTarget(true, targetEntity);
        start = null;
    }

    public void ToggleTarget(bool toggle, MUDEntity entity) {

        //turn off old
        if(targetEntity != null && entity != targetEntity) {ToggleTarget(false, targetEntity);}

        targetEntity = entity;
        
        if(targetEntity.Loaded) {Setup();}
        else{targetEntity.OnLoaded += Setup;}
    }   

    public void Setup() {

        posSync = targetEntity?.GetRootComponent<PositionSync>();
        
        if(posSync == null) {Debug.LogError("They don't have pos", this); return;}

        line.SetTarget(ourPos.Target, posSync.Target);
    }

    public void Hover(bool toggle) {
        
        if(toggle) {
            if(!ourPos || !posSync) {return;}
            line.Toggle(true);
        } else {
            line.Toggle(false);
        }
    }

}
