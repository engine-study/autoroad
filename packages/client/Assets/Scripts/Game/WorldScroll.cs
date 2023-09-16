using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class WorldScroll : MonoBehaviour {

    public static float PositionToMile(Vector3 position) {return Mathf.Floor(position.z / (float)MapConfigComponent.Height);}
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
    [SerializeField] bool playerFocus = false;
    [SerializeField] float maxMile = 0f;
    [SerializeField] int currentMile = -1;
    [SerializeField] float mileScroll, lastScroll = -100f;
    float playerMile = 0f, lastPlayerMile = -1f;
    float targetScroll, targetPlayer;

    public float MileDistance { get { return currentMile * GameStateComponent.MILE_DISTANCE; } }
    public float MileTotalScroll { get { return mileScroll * GameStateComponent.MILE_DISTANCE; } }
    public static float GetMileLerp(float newMile) {return GameStateComponent.MILE_COUNT == 0 ? 1f : Mathf.Clamp01(newMile/GameStateComponent.MILE_COUNT);}

    void Awake() {

        Instance = this;
        currentMile = -1;
        playerUI.gameObject.SetActive(false);
        mileUI.ToggleWindowClose();

        GameStateComponent.OnGameStateUpdated += Init;
        SPEvents.OnLocalPlayerSpawn += InitPlayer;

        enabled = false;

    }

    void OnDestroy() {
        Instance = null;
        GameStateComponent.OnGameStateUpdated -= Init;
        SPEvents.OnLocalPlayerSpawn -= InitPlayer;

        if(PlayerMUD.LocalPlayer)
            (PlayerMUD.LocalPlayer as PlayerMUD).Position.OnUpdated -= UpdatePlayerPosition;
    }

    void Init() {
        SetMaxMile((int)GameStateComponent.MILE_COUNT);
    }

    void InitPlayer() {

        enabled = true;

        (PlayerMUD.LocalPlayer as PlayerMUD).Position.OnUpdated += UpdatePlayerPosition;
        
        playerUI.gameObject.SetActive(true);
        mileUI.ToggleWindowOpen();

        UpdatePlayerPosition();
    }
        
    
    void Update() {

        UpdateInput();
        UpdateScroll();
        UpdateUI();
    }

    float GetClampedMile(float newMile) {
        return Mathf.Clamp(newMile, 0f, maxMile);
    }
    float GetSoftClampedMile(float newMile) {
        return Mathf.Clamp(newMile, -.5f, maxMile+.5f);
    }

    void UpdateInput() {
        
        if (SPUIBase.CanInput && SPUIBase.IsMouseOnScreen && Input.GetKey(KeyCode.LeftShift)) {
            mileScroll = GetSoftClampedMile(mileScroll + Input.mouseScrollDelta.y * 10f * Time.deltaTime);
            // scrollLock = Mathf.Round(mileScroll / 90) * 90;
        }
        
        if (SPUIBase.CanInput && !SPInput.ModifierKey && Input.GetMouseButtonDown(1)) {

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

        bool isInNewMile = Mathf.Abs((mileScroll * GameStateComponent.MILE_DISTANCE) - MileDistance) > GameStateComponent.MILE_DISTANCE * .5f;
        int newMile = Mathf.RoundToInt(mileScroll);
        //if we're more than halfway to the next mile, magnet over to it
        if (isInNewMile && newMile != currentMile) {
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

        playerMile = GetClampedMile(PositionToMile((PlayerMUD.LocalPlayer as PlayerMUD).Position.Pos));
        targetPlayer = GetMileLerp(playerMile);

        if (playerMile != lastPlayerMile) {
            SetToPlayerMile();
        }

        lastPlayerMile = playerMile;

    }

    void SetToScrollMile() {
        Debug.Log("SCROLL: Scroll", this);
        ToggleCameraOnPlayer(false);
        SetMile(Mathf.RoundToInt(mileScroll));
    }

    void SetToPlayerMile() {

        Debug.Log("SCROLL: Player", this);

        mileScroll = playerMile;

        ToggleCameraOnPlayer(true);
        SetMile((int)playerMile);

        mileHeading.UpdateField("Mile " + (int)(playerMile+1));
        mileUI.ToggleWindowOpen();
        
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

    public void SetMaxMile(int newMaxMile) {
        maxMile = newMaxMile;
    }

    public void SetMile(int newMile) {

        Debug.Log("WORLD: (" + currentMile + ") " + (int)newMile + " / " + (int)maxMile, this);

        //out of range
        if(newMile > maxMile || newMile < 0f) {
            Debug.LogError("Not valid: " + (int)newMile + " / " + (int)maxMile, this);
            return;
        }

        LoadMile(newMile);

        mileHeading.UpdateField("Mile " + (int)(newMile+1));
        mileUI.ToggleWindowOpen();

        currentMile = newMile;

        front.transform.position = Vector3.forward * (currentMile * MapConfigComponent.Height + MapConfigComponent.Height);
        back.transform.position = Vector3.forward * (currentMile * MapConfigComponent.Height - MapConfigComponent.Height);

    }

    public void LoadMile(int newMile) {
        
        string chunkEntity = MUDHelper.Keccak256("Chunk", (int)newMile);
        ChunkComponent newChunk = MUDWorld.FindComponent<ChunkComponent>(chunkEntity);

        if(newChunk == null) {
            return;
        }
           
        newChunk.gameObject.SetActive(true);
        recapButton.ToggleWindow(newChunk.Completed);



    }


}
