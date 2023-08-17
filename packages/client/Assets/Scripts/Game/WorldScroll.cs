using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class WorldScroll : MonoBehaviour {

    public static float PositionToMile(Vector3 position) {return Mathf.Floor(position.z / (float)RoadConfigComponent.Height);}
    public static WorldScroll Instance;

    [Header("World Scroll")]
    public SPWindowParent mileUI;
    public UnityEngine.UI.ScrollRect scrollUI;
    public UnityEngine.UI.Scrollbar barUI;
    public UnityEngine.UI.Scrollbar playerUI;
    public SPHeading mileHeading;

    [Header("Game State")]
    [SerializeField] private TableManager chunkTable;
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;

    [Header("Debug")]
    [SerializeField] float maxMile = 0f;
    [SerializeField] float currentMile = -1f;
    [SerializeField] float mileScroll, lastScroll = -100f;
    float playerMile = 0f, lastPlayerMile = -1f; 

    public float MileTotal { get { return currentMile * GameStateComponent.MILE_DISTANCE; } }
    public float MileTotalScroll { get { return mileScroll * GameStateComponent.MILE_DISTANCE; } }
    public static float GetMileLerp(float newMile) {return GameStateComponent.MILE_COUNT == 0 ? 1f : Mathf.Clamp01(newMile/GameStateComponent.MILE_COUNT);}

    void Awake() {

        Instance = this;
        enabled = false;
        currentMile = -1;
        playerUI.gameObject.SetActive(false);
        mileUI.ToggleWindowClose();

        GameStateComponent.OnGameStateUpdated += UpdateMile;
        SPEvents.OnLocalPlayerSpawn += AddPlayerMile;
    }

    void OnDestroy() {
        Instance = null;
        GameStateComponent.OnGameStateUpdated -= UpdateMile;
        SPEvents.OnLocalPlayerSpawn -= AddPlayerMile;

        if(PlayerMUD.LocalPlayer)
            (PlayerMUD.LocalPlayer as PlayerMUD).Position.OnUpdated -= UpdatePlayerMile;
    }
    

    void UpdateMile() {
        enabled = true;

        SetMaxMile(GameStateComponent.MILE_COUNT);
        SetMile(GameStateComponent.MILE_COUNT);
    }

    void AddPlayerMile() {

        (PlayerMUD.LocalPlayer as PlayerMUD).Position.OnUpdated += UpdatePlayerMile;
        UpdatePlayerMile();
        
        playerUI.gameObject.SetActive(true);
        mileUI.ToggleWindowOpen();

    }
        

    void UpdatePlayerMile() {
        playerMile = PositionToMile((PlayerMUD.LocalPlayer as PlayerMUD).Position.Pos);
        playerUI.value = GetMileLerp(playerMile);

        if(playerMile != lastPlayerMile) {
            mileHeading.UpdateField("Mile " + (int)(playerMile+1));
            mileUI.ToggleWindowOpen();
        }

        lastPlayerMile = playerMile;
    }

    
    void Update() {

        if (SPUIBase.CanInput && SPUIBase.IsMouseOnScreen && Input.GetKey(KeyCode.LeftShift)) {
            mileScroll = Mathf.Clamp(mileScroll + Input.mouseScrollDelta.y * 10f * Time.deltaTime, -.5f, maxMile + .5f);
            // scrollLock = Mathf.Round(mileScroll / 90) * 90;
        }

        //magnetism, lerp back to current mile
        mileScroll = Mathf.MoveTowards(mileScroll, currentMile, 1f * Time.deltaTime);

        float mileLerp = GetMileLerp(mileScroll);
        barUI.value = Mathf.Lerp(barUI.value, mileLerp, .1f);

        //if we're more than halfway to the next mile, magnet over to it
        if (Mathf.Abs((mileScroll * GameStateComponent.MILE_DISTANCE) - MileTotal) > GameStateComponent.MILE_DISTANCE * .5f) {
            SetMile(Mathf.RoundToInt(mileScroll));
        }

        if (lastScroll != mileScroll) {
            SPCamera.SetTarget(Vector3.forward * (MileTotalScroll + GameStateComponent.MILE_DISTANCE * .5f));
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
            // newChunk = MUDWorld.FindOrMakeComponent<ChunkComponent>(chunkEntity);
            return;
        } else {
            newChunk.gameObject.SetActive(true);
        }
        
        mileHeading.UpdateField("Mile " + (int)(newMile+1));
        mileUI.ToggleWindowOpen();

        //unlock the camera
        if(currentMile != -1 && SPCamera.Follow != null) {
            SPCamera.SetFollow(null);
        }

        currentMile = Mathf.Clamp(newMile, 0f, maxMile);
        SPCamera.SetTarget(Vector3.forward * MileTotal);

        front.transform.position = Vector3.forward * (currentMile * RoadConfigComponent.Height + RoadConfigComponent.Height);
        back.transform.position = Vector3.forward * (currentMile * RoadConfigComponent.Height - RoadConfigComponent.Height);

    }


}
