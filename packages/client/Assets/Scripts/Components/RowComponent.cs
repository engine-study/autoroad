using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class RowComponent : MUDComponent
{
    [Header("Row")]
    [SerializeField] public RoadComponent [] spawnedRoads;
    [SerializeField] public RoadComponent [] roadFiller;


    protected override void Awake() {
        spawnedRoads = new RoadComponent[5];
    }

    public void SetComplete() {
        for(int i = 0; i < roadFiller.Length; i++) {
            roadFiller[i].SetStage(RoadState.Paved);
        }
    }

    public void SetRoadBlock(string entityName, int x, RoadComponent road) {
        
        roadFiller[x].gameObject.SetActive(road == null);
        roadFiller[x].gameObject.name = MUDHelper.TruncateHash(entityName);

        spawnedRoads[x] = road;

        if(road != null) {
            road.Entity.transform.parent = transform;
        }
    }

    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

    }
}
