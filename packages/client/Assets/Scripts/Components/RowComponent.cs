using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class RowComponent : MonoBehaviour {

    public RoadComponent [] Roads {get { return spawnedRoads; } }
    bool isCompleted = false; 

    [Header("Row")]
    [SerializeField] private GameObject complete;
    [SerializeField] private GameObject completeEffects;

    [Header("Reference")]
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private Transform placeHolderParent;
    [SerializeField] private Transform leftSide;
    [SerializeField] private Transform rightSide;
    [SerializeField] private Transform groundCompletePlane;
    

    [Header("Debug")]
    [SerializeField] public ChunkComponent chunk;
    [SerializeField] private RoadComponent[] spawnedRoads;
    [SerializeField] private GameObject[] roadFiller;



    void Awake() {

    }

    public void SpawnRoad(int width) {

        complete.SetActive(false);
        completeEffects.SetActive(false);

        roadFiller = new GameObject[width];
        spawnedRoads = new RoadComponent[width];

        int left = Mathf.CeilToInt(-width * .5f);
        int right = Mathf.FloorToInt(width * .5f);
        int index = 0;
        
        for (int i = left; i <= right; i++) {
            GameObject newRoad = Instantiate(roadPrefab, transform.position + Vector3.right * i, Quaternion.identity, placeHolderParent);
            newRoad.name = "Road " + i;
            newRoad.SetActive(true);
            roadFiller[index] = newRoad;
            index++;
        }

        leftSide.localPosition = Vector3.right * (left - .5f); 
        rightSide.localPosition = Vector3.right * (right + .5f); 
        groundCompletePlane.localScale = Vector3.forward + Vector3.up + Vector3.right * width;
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
            if (spawnedRoads[i] == null || spawnedRoads[i].State < RoadState.Pavimentum) {
                return false;
            }
        }

        return true;

    }

}
