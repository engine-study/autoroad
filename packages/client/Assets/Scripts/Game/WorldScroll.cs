using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class WorldScroll : MonoBehaviour
{
    public static WorldScroll Instance;

    [Header("World Scroll")]
    public SPHeading mileHeading;

    [Header("Game State")]
    [SerializeField] private TableManager chunkTable;
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;

    [Header("Debug")]
    [SerializeField] float maxMile = 0f;
    [SerializeField] float currentMile = -1f;
    [SerializeField] float mileScroll, lastScroll = -100f;

    public float MileTotal { get { return currentMile * GameStateComponent.MILE_DISTANCE; } }
    public float MileTotalScroll { get { return mileScroll * GameStateComponent.MILE_DISTANCE; } }

    void Awake() {
        Instance = this;
    }

    void Start() {
        currentMile = -1;
        GameStateComponent.OnGameStateUpdated += UpdateMile;
    }
    
    void OnDestroy() {
        Instance = null;
        GameStateComponent.OnGameStateUpdated -= UpdateMile;
    }
    

    void UpdateMile() {
        SetMaxMile(GameStateComponent.MILE_COUNT);
    }

    void Update() {

        if (SPUIBase.CanInput && SPUIBase.IsMouseOnScreen && Input.GetKey(KeyCode.LeftAlt)) {
            mileScroll += Input.mouseScrollDelta.y * .1f;
            // scrollLock = Mathf.Round(mileScroll / 90) * 90;
        }

        //magnetism, lerp back to current mile
        mileScroll = Mathf.MoveTowards(mileScroll, currentMile, 1f * Time.deltaTime);

        //if we're more than halfway to the next mile, magnet over to it
        if (Mathf.Abs((mileScroll * GameStateComponent.MILE_DISTANCE) - MileTotal) > GameStateComponent.MILE_DISTANCE * .5f) {
            SetMile(Mathf.RoundToInt(mileScroll));
        }

        if (mileScroll != lastScroll) {
            SPCamera.SetTarget(Vector3.forward * MileTotalScroll);
        }

        lastScroll = mileScroll;

    }

    public void SetMaxMile(float newMaxMile) {
        maxMile = newMaxMile;
    }

    public void SetMile(float newMile) {

        //out of range
        if(newMile > maxMile || newMile < 0f) {
            return;
        }

        string chunkEntity = MUDHelper.Keccak256("Chunk", (int)newMile);
        ChunkComponent newChunk = MUDWorld.FindComponent<ChunkComponent>(chunkEntity);

        if(newChunk == null) {
            newChunk = MUDWorld.FindOrMakeComponent<ChunkComponent>(chunkEntity);
        } else {
            newChunk.gameObject.SetActive(true);
        }

        // mileHeading.UpdateField("Mile " + newMile);

        currentMile = Mathf.Clamp(newMile,0f, maxMile);
        SPCamera.SetTarget(Vector3.forward * MileTotal);

        front.transform.position = Vector3.forward * (currentMile * RoadConfigComponent.Height + RoadConfigComponent.Height);
        back.transform.position = Vector3.forward * (currentMile * RoadConfigComponent.Height - RoadConfigComponent.Height);

    }


}
