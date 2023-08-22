using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class WorldScroll : MonoBehaviour {

    public static float PositionToMile(Vector3 position) {return Mathf.Floor(position.z / (float)RoadConfigComponent.Height);}
    public static WorldScroll Instance;
    public static float Mile {get { return Instance.currentMile; } }

    [Header("World Scroll")]
    public SPWindowParent mileUI;
    public UnityEngine.UI.ScrollRect scrollUI;
    public UnityEngine.UI.Scrollbar barUI;
    public UnityEngine.UI.Scrollbar playerUI;
    public SPHeading mileHeading;
    public SPButton recapButton;
    public SPButton resetCameraButton;

    [Header("Game State")]
    [SerializeField] private TableManager chunkTable;
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;

    [Header("Debug")]
    [SerializeField] float maxMile = 0f;
    [SerializeField] float currentMile = -1f;
    [SerializeField] float mileScroll, lastScroll = -100f;
    float playerMile = 0f, lastPlayerMile = -1f;
    bool playerFocus = false;
    float targetScroll, targetPlayer;

    public float MileTotal { get { return currentMile * GameStateComponent.MILE_DISTANCE; } }
    public float MileTotalScroll { get { return mileScroll * GameStateComponent.MILE_DISTANCE; } }
    public static float GetMileLerp(float newMile) {return GameStateComponent.MILE_COUNT == 0 ? 1f : Mathf.Clamp01(newMile/GameStateComponent.MILE_COUNT);}

    void Awake() {

        Instance = this;
        enabled = false;
        currentMile = -1;
        playerUI.gameObject.SetActive(false);
        mileUI.ToggleWindowClose();

        GameStateComponent.OnGameStateUpdated += Init;
        SPEvents.OnLocalPlayerSpawn += InitPlayer;
    }

    void OnDestroy() {
        Instance = null;
        GameStateComponent.OnGameStateUpdated -= Init;
        SPEvents.OnLocalPlayerSpawn -= InitPlayer;

        if(PlayerMUD.LocalPlayer)
            (PlayerMUD.LocalPlayer as PlayerMUD).Position.OnUpdated -= UpdatePlayerPosition;
    }

    void Init() {
        SetMaxMile(GameStateComponent.MILE_COUNT);
    }

    void InitPlayer() {

        enabled = true;

        (PlayerMUD.LocalPlayer as PlayerMUD).Position.OnUpdated += UpdatePlayerPosition;
        
        playerUI.gameObject.SetActive(true);
        mileUI.ToggleWindowOpen();

        SetMile(PositionToMile((PlayerMUD.LocalPlayer as PlayerMUD).Position.Pos));

        ToggleCameraOnPlayer(true);
        SetToPlayerMile();

        mileScroll = playerMile;

    }
        
    
    void Update() {

        UpdateInput();
        UpdateScroll();
        UpdateUI();
    }

    void UpdateInput() {
        if (SPUIBase.CanInput && SPUIBase.IsMouseOnScreen && Input.GetKey(KeyCode.LeftShift)) {
            mileScroll = Mathf.Clamp(mileScroll + Input.mouseScrollDelta.y * 10f * Time.deltaTime, -.5f, maxMile + .5f);
            // scrollLock = Mathf.Round(mileScroll / 90) * 90;
        }
        
        if (SPUIBase.CanInput && Input.GetMouseButtonDown(1)) {

            ToggleCameraOnPlayer(false);

            //focus on whatever we select
            if (CursorMUD.Base) {
                SPCamera.SetFollow(CursorMUD.Base.Root);
            } else {
                SPCamera.SetFollow(null);
                float x = Mathf.Clamp(SPInput.MousePlanePos.x, BoundsComponent.Left, BoundsComponent.Right);
                float z = Mathf.Clamp(SPInput.MousePlanePos.z, BoundsComponent.Down, BoundsComponent.Up);
                SPCamera.SetTarget(new Vector3(x, 0f, z));
            }

        }

        if (SPUIBase.CanInput && Input.GetKeyDown(KeyCode.Space)) {
            ResetCameraToPlayer();
        }
    }

    void UpdateScroll() {

        //magnetism, lerp back to current mile
        mileScroll = Mathf.MoveTowards(mileScroll, currentMile, 1f * Time.deltaTime);
        targetScroll = GetMileLerp(mileScroll);

        //if we're more than halfway to the next mile, magnet over to it
        if (Mathf.Abs((mileScroll * GameStateComponent.MILE_DISTANCE) - MileTotal) > GameStateComponent.MILE_DISTANCE * .5f) {
            SetToScrollMile();
        }

        if (!playerFocus && lastScroll != mileScroll) {
            SPCamera.SetTarget(Vector3.forward * (MileTotalScroll + GameStateComponent.MILE_DISTANCE * .5f));
        }

        lastScroll = mileScroll;
    }

    void UpdateUI() {
        
        if(barUI.value != targetScroll) { barUI.value = Mathf.MoveTowards(barUI.value, targetScroll, 5f * Time.deltaTime); }
        if (playerUI.value != targetPlayer) { playerUI.value = Mathf.MoveTowards(playerUI.value, targetPlayer, 5f * Time.deltaTime); }
    }

    void UpdatePlayerPosition() {
        playerMile = PositionToMile((PlayerMUD.LocalPlayer as PlayerMUD).Position.Pos);
        targetPlayer = GetMileLerp(playerMile);
        if (playerMile != lastPlayerMile) {
            SetToPlayerMile();
        }

        lastPlayerMile = playerMile;

    }

    void SetToScrollMile() {
        SetMile(Mathf.RoundToInt(mileScroll));
        ToggleCameraOnPlayer(false);
    }

    void SetToPlayerMile() {

        playerFocus = true;
        mileScroll = playerMile;

        SetMile(playerMile);
        mileHeading.UpdateField("Mile " + (int)(playerMile+1));
        mileUI.ToggleWindowOpen();
        
        ToggleCameraOnPlayer(true);
    }

    public void ResetCameraToPlayer() {
        if (PlayerComponent.LocalPlayer == null) { return; }
        SetToPlayerMile();
    }

    public void ToggleCameraOnPlayer(bool toggle) {

        playerFocus = toggle; 
        resetCameraButton.ToggleWindow(!playerFocus);

        if(toggle) {
            SPCamera.SetFollow(PlayerComponent.LocalPlayer.PlayerScript.Root);
        } else {
            SPCamera.SetFollow(null);
        }

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
        recapButton.ToggleWindow(newChunk.Completed);


        currentMile = Mathf.Clamp(newMile, 0f, maxMile);

        front.transform.position = Vector3.forward * (currentMile * RoadConfigComponent.Height + RoadConfigComponent.Height);
        back.transform.position = Vector3.forward * (currentMile * RoadConfigComponent.Height - RoadConfigComponent.Height);

    }


}
