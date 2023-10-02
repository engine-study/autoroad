using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using System;
using TMPro;

public class WorldScroll : MonoBehaviour {

    public static Action OnMile;
    public static WorldScroll Instance;
    public static float Mile {get { return Instance.mile; } }

    [Header("World Scroll")]
    public SPWindowParent mileUI;
    public UnityEngine.UI.ScrollRect scrollUI;
    public UnityEngine.UI.Scrollbar barUI;
    public UnityEngine.UI.Scrollbar playerUI;
    public SPHeading mileHeading;
    public SPButton recapButton;
    public SPButton playerButton, landscapeButon;
    public GameObject mileNumber;
    public TextMeshProUGUI mileText;
    public AudioClip sfx_mile;

    [Header("Game State")]
    [SerializeField] GameObject front;
    [SerializeField] GameObject back;

    [Header("Debug")]
    [SerializeField] bool playerFocus = false;
    [SerializeField] float maxMile = 0f;
    [SerializeField] int mile = -1;
    [SerializeField] float mileScroll, lastScroll = -100f;
    float playerMile = 0f, lastPlayerMile = -1f;
    float targetScroll, targetPlayer;
    bool ready = false;

    public float MileDistance { get { return mile * GameStateComponent.MILE_DISTANCE; } }
    public float MileTotalScroll { get { return mileScroll * GameStateComponent.MILE_DISTANCE; } }
    public static float GetMileLerp(float newMile) {return GameStateComponent.MILE_COUNT == 0 ? 1f : Mathf.Clamp01(newMile/GameStateComponent.MILE_COUNT);}

    void Awake() {

        Instance = this;
        mile = -1;
        playerUI.gameObject.SetActive(false);
        mileUI.ToggleWindowClose();

        GameStateComponent.OnGameStateUpdated += GameStateUpdate;
        SPEvents.OnServerLoaded += InitWorld;
        SPEvents.OnLocalPlayerSpawn += InitPlayer;

        TutorialUI.OnTutorial += ToggleTutorial;

        enabled = false;

    }

    void OnDestroy() {
        Instance = null;
        GameStateComponent.OnGameStateUpdated -= GameStateUpdate;
        SPEvents.OnServerLoaded -= InitWorld;
        SPEvents.OnLocalPlayerSpawn -= InitPlayer;

        if(PlayerMUD.LocalPlayer)
            (PlayerMUD.LocalPlayer as PlayerMUD).Position.OnUpdated -= UpdatePlayerPosition;
    }

    void InitWorld() {
        ready = true;
        GameStateUpdate();
    }

    void GameStateUpdate() {
        if(!ready) {return;}
        Debug.Log("[SCROLL] Moving furthest mile to " + (int)GameStateComponent.MILE_COUNT);
        SetFurthestMile((int)GameStateComponent.MILE_COUNT);
    }

    void InitPlayer() {

        enabled = true;

        (PlayerMUD.LocalPlayer as PlayerMUD).Position.OnUpdated += UpdatePlayerPosition;
        
        playerUI.gameObject.SetActive(true);
        mileUI.ToggleWindowOpen();

        UpdatePlayerPosition();
        SetToScrollMile();
        
    }
    
    void Update() {

        if(ChunkLoader.HasLoadedAllChunks == false) {return;}

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
        
        if (SPUIBase.CanInput && !SPUIBase.IsPointerOverUIElement && SPUIBase.IsMouseOnScreen && SPInput.ModifierKey == false && Input.mouseScrollDelta.y != 0f) {
            if(Input.mouseScrollDelta.y != 0f) {mileUI.ToggleWindowOpen();}
            mileScroll = GetSoftClampedMile(mileScroll + Input.mouseScrollDelta.y * 10f * Time.deltaTime);
            // scrollLock = Mathf.Round(mileScroll / 90) * 90;
        }
        
        if (SPUIBase.CanInput && !SPUIBase.IsPointerOverUIElement && !SPInput.ModifierKey && Input.GetMouseButtonDown(1)) {

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
            SetToPlayerMile(true);
        }
    }

    void UpdateScroll() {

        //magnetism, lerp back to current mile
        mileScroll = Mathf.MoveTowards(mileScroll, mile, 1f * Time.deltaTime);
        targetScroll = GetMileLerp(mileScroll);

        bool isInNewMile = Mathf.Abs((mileScroll * GameStateComponent.MILE_DISTANCE) - MileDistance) > GameStateComponent.MILE_DISTANCE * .5f;
        int newMile = Mathf.RoundToInt(mileScroll);
        //if we're more than halfway to the next mile, magnet over to it
        if (isInNewMile && newMile != mile) {
            SetToScrollMile(true);
        }

        if (!playerFocus && lastScroll != mileScroll) {
            SPCamera.SetTarget(Vector3.forward * (MileTotalScroll + GameStateComponent.MILE_DISTANCE * .5f));
        }

        lastScroll = mileScroll;
    }

    void UpdateUI() {
        
        if(barUI.value != targetScroll) { barUI.value = Mathf.Lerp(barUI.value, targetScroll, .1f); }
        if (playerUI.value != targetPlayer) { playerUI.value = Mathf.Lerp(playerUI.value, targetPlayer, .1f); }
    }

    void UpdatePlayerPosition() {

        playerMile = GetClampedMile(PositionComponent.PositionToMile((PlayerMUD.LocalPlayer as PlayerMUD).Position.Pos));
        targetPlayer = GetMileLerp(playerMile);

        if (playerMile != lastPlayerMile) {
            SetToPlayerMile(true);
        }

        lastPlayerMile = playerMile;

    }

    public void ToggleTutorial(bool toggle) {
        enabled = !toggle;
    }

    public void SetToScrollMile() { SetToScrollMile(false);}
    public void SetToPlayerMile() {SetToPlayerMile(false);}

    public void SetToScrollMile(bool withFX) {
        Debug.Log("SCROLL: Scroll", this);
        ToggleCameraOnPlayer(false);
        SetMile(Mathf.RoundToInt(mileScroll), withFX);
    }

    public void SetToPlayerMile(bool withFX) {

        Debug.Log("SCROLL: Player", this);

        mileScroll = playerMile;

        ToggleCameraOnPlayer(true);
        SetMile((int)playerMile, withFX);

        mileHeading.UpdateField("Mile " + (int)(playerMile+1));
        mileUI.ToggleWindowOpen();
        
    }

    public void ToggleCameraOnPlayer(bool toggle) {

        if(toggle) {
            MotherUI.FollowPlayer();
        } else {
            if(playerFocus) SPCamera.SetFOVGlobal(10f);
            SPCamera.SetFollow(null);
            SPCamera.SetTarget(Vector3.forward * (MileTotalScroll + GameStateComponent.MILE_DISTANCE * .5f));
        }

        playerFocus = toggle; 

        playerButton.ToggleWindow(!playerFocus);
        landscapeButon.ToggleWindow(playerFocus);


    }

    public void SetFurthestMile(int newMaxMile) {
        maxMile = newMaxMile;
        front.transform.position = Vector3.forward * (maxMile * MapConfigComponent.Height + MapConfigComponent.Height);
        // back.transform.position = Vector3.forward * (mile * MapConfigComponent.Height - MapConfigComponent.Height);
    }

    public void SetMile(int newMile, bool withFX = false) {

        Debug.Log("WORLD: (" + mile + ") " + (int)newMile + " / " + (int)maxMile, this);

        //out of range
        if(newMile > maxMile || newMile < 0f) {
            Debug.LogError("Not valid: " + (int)newMile + " / " + (int)maxMile, this);
            return;
        }

        //LOAD MILE
        bool loaded = ChunkLoader.LoadMile(newMile);
        if(loaded == false) { return; }

        mile = newMile;

        mileHeading.UpdateField("Mile " + (int)(newMile+1));
        mileUI.ToggleWindowOpen();

        recapButton.ToggleWindow(ChunkLoader.Chunk.Completed);
    
        // front.transform.position = Vector3.forward * (mile * MapConfigComponent.Height + MapConfigComponent.Height);
        // back.transform.position = Vector3.forward * (mile * MapConfigComponent.Height - MapConfigComponent.Height);
        
        if(withFX) {
            // mileNumber.SetActive(true);
            // mileText.text = (newMile+1).ToString();
            SPUIBase.PlaySound(sfx_mile);
        }

        OnMile?.Invoke();

    }

}
