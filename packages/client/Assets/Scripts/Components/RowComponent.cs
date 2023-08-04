using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class RowComponent : MonoBehaviour
{
    bool isCompleted = false; 

    [Header("Row")]
    [SerializeField] private ChunkComponent chunk;
    [SerializeField] private GameObject complete;
    [SerializeField] private GameObject completeEffects;
    [SerializeField] private RoadComponent[] roadFiller;

    [Header("Debug")]
    [SerializeField] private RoadComponent[] spawnedRoads;



    void Awake() {
        spawnedRoads = new RoadComponent[5];
    }

    void OnDestroy() {
        for (int i = 0; i < spawnedRoads.Length; i++) {
            if (spawnedRoads[i] != null)
                spawnedRoads[i].OnUpdated -= CheckIfCompleted;
        }
    }

    public void SetRoadBlock(string entityName, int x, RoadComponent road) {

        if (road == spawnedRoads[x] || road == null) {
            Debug.LogError(road == null ? "null" : "Adding twice", this);
            return;
        }

        roadFiller[x].gameObject.SetActive(road == null);
        roadFiller[x].gameObject.name = MUDHelper.TruncateHash(entityName);

        spawnedRoads[x] = road;

        road.Entity.transform.parent = transform;
        road.OnUpdated += CheckIfCompleted;

        CheckIfCompleted();
    }

    void CheckIfCompleted() {

        if(IsCompleted()) {
            if(chunk.Loaded) {
                PlayCompletedEvent();
            } else {
                SetCompleted();
            }
        }

    }

    void PlayCompletedEvent() {
        SetCompleted();
        completeEffects.SetActive(true);
    }

    void SetCompleted() {
        isCompleted = true;
        complete.gameObject.SetActive(true);
    }

    public bool IsCompleted() {

        if(isCompleted) {
            return false;
        }

        for (int i = 0; i < spawnedRoads.Length; i++) {
            if (spawnedRoads[i] == null || spawnedRoads[i].state < RoadState.Paved) {
                return false;
            }
        }

        return true;

    }

}
