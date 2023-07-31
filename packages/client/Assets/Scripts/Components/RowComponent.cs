using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class RowComponent : MUDComponent
{
    [Header("Row")]
    [SerializeField] private ChunkComponent chunk;
    [SerializeField] private GameObject complete;
    [SerializeField] private RoadComponent [] roadFiller;

    [Header("Debug")]
    [SerializeField] private RoadComponent [] spawnedRoads;



    protected override void Awake() {
        spawnedRoads = new RoadComponent[5];
    }

    public void SetComplete() {
        for(int i = 0; i < roadFiller.Length; i++) {
            roadFiller[i].SetStage(RoadState.Paved);
        }

        complete.SetActive(true);
    }

    public void SetRoadBlock(string entityName, int x, RoadComponent road) {
        
        roadFiller[x].gameObject.SetActive(road == null);
        roadFiller[x].gameObject.name = MUDHelper.TruncateHash(entityName);

        spawnedRoads[x] = road;

        if(road != null) {
            road.Entity.transform.parent = transform;
        }
    }

    
    public bool CheckIfCompleted(){

        for(int i = 0; i < spawnedRoads.Length; i++) {
            if(spawnedRoads[i] == null || spawnedRoads[i].state != RoadState.Paved) {
                return false;
            }
        }

        return true;

    }


    protected override IMudTable GetTable() {throw new System.NotImplementedException();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

    }
}
