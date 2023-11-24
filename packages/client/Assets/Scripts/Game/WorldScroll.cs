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
    public static float MaxMile {get{return Instance.maxMile;}}
    public static float MinMile {get{return Instance.minMile;}}

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
    [SerializeField] GameObject updateMileText;
    [SerializeField] GameObject newMileText;
    [SerializeField] GameObject [] difficultyStars;

    [Header("Debug")]
    [SerializeField] bool playerFocus = false;
    [SerializeField] float maxMile = 0f;
    float minMile = -1f;
    [SerializeField] int mile = 0;
    [SerializeField] float playerMile = 0f;
    [SerializeField] float scrollMile;
    [SerializeField] RoadComponent completedRoad;
    [SerializeField] ChunkComponent focusedChunk;
    float lastPlayerMile = -10000f, lastScroll = -100f;
    float targetScroll, targetPlayer;
    bool ready = false;
    bool hasPlayer = false;

    public float MileDistance { get { return mile * GameStateComponent.MILE_DISTANCE; } }
    public float MileTotalScroll { get { return scrollMile * GameStateComponent.MILE_DISTANCE; } }
    public static float GetMileLerp(float newMile) {return GameStateComponent.MILE_COUNT == 0 ? 1f : Mathf.InverseLerp(MinMile, MaxMile, newMile);}

    void Awake() {

        Instance = this;

        mile = -1000;
        playerUI.gameObject.SetActive(false);
        updateMileText.SetActive(true);
        newMileText.SetActive(false);

        GameStateComponent.OnGameStateUpdated += GameStateUpdate;
        SPEvents.OnServerLoaded += InitWorld;
        SPEvents.OnLocalPlayerSpawn += InitPlayer;

        TutorialUI.OnTutorial += ToggleTutorial;
        RoadComponent.OnCompletedRoad += CompleteRoad;

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
        SetMile(Mathf.RoundToInt(minMile), false, true);

        enabled = true;
    }

    void GameStateUpdate() {
        
        // Debug.Log("[SCROLL] Moving furthest mile to " + (int)GameStateComponent.MILE_COUNT);
        maxMile = (int)GameStateComponent.MILE_COUNT;

        if(!ready) {return;}

        front.transform.position = Vector3.forward * (maxMile * MapConfigComponent.Height + MapConfigComponent.Height);
        StartCoroutine(NewMileSummonedCoroutine());
         
    }

    IEnumerator NewMileSummonedCoroutine() {
        updateMileText.SetActive(false);
        newMileText.SetActive(true);

        //show difficulty
        int difficulty = (int)maxMile % 5;
        int showDifficulty = 0;

        for(int i = 0; i < difficultyStars.Length; i++) {
            difficultyStars[i].SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        while(showDifficulty < difficulty) {
            difficultyStars[showDifficulty].SetActive(true);
            yield return new WaitForSeconds(.25f);
            showDifficulty++;
        }

        yield return new WaitForSeconds(5f);

        updateMileText.SetActive(true);
        newMileText.SetActive(false);
    }

    void InitPlayer() {

        Debug.Log("Init player world scroll");
        Debug.Log($"Max mile {maxMile}");
        
        hasPlayer = true;

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
        return Mathf.Clamp(newMile, minMile, maxMile);
    }
    float GetSoftClampedMile(float newMile) {
        return Mathf.Clamp(newMile, minMile - .5f, maxMile+.5f);
    }

    void UpdateInput() {
        
        if (SPUIBase.CanInput && Input.GetKey(KeyCode.LeftShift) && SPUIBase.IsMouseOnScreen && Input.mouseScrollDelta.y != 0f) {
            if(Input.mouseScrollDelta.y != 0f) {mileUI.ToggleWindowOpen();}
            scrollMile = GetSoftClampedMile(scrollMile + Input.mouseScrollDelta.y * 10f * Time.deltaTime);
            // scrollLock = Mathf.Round(mileScroll / 90) * 90;
        }
        
        // if (SPUIBase.CanInput && !SPUIBase.IsPointerOverUIElement && !SPInput.ModifierKey && Input.GetMouseButtonDown(1)) {
        //     ToggleCameraOnPlayer(false);

        //     //focus on whatever we select
        //     if (CursorMUD.Base) {
        //         SPCamera.SetFollow(CursorMUD.Base.Root);
        //     } else {
        //         SPCamera.SetFollow(null);
        //         float x = Mathf.Clamp(SPInput.MousePlanePos.x, BoundsComponent.Left, BoundsComponent.Right);
        //         float z = Mathf.Clamp(SPInput.MousePlanePos.z, BoundsComponent.Down, BoundsComponent.Up);
        //         SPCamera.SetTarget(new Vector3(x, 0f, z));
        //     }
        // }

        if (hasPlayer && SPUIBase.CanInput && Input.GetKeyDown(KeyCode.Space)) {
            UpdatePlayerPosition();
            SetToPlayerMile(true);
            SPCamera.SetTarget(Quaternion.Euler(Vector3.up * 45f));
        }
    }

    void UpdateScroll() {

        //magnetism, lerp back to current mile
        scrollMile = Mathf.MoveTowards(scrollMile, mile, 1f * Time.deltaTime);
        targetScroll = GetMileLerp(scrollMile);

        bool isInNewMile = Mathf.Abs((scrollMile * GameStateComponent.MILE_DISTANCE) - MileDistance) > GameStateComponent.MILE_DISTANCE * .5f;
        int newMile = Mathf.RoundToInt(scrollMile);
        //if we're more than halfway to the next mile, magnet over to it
        if (isInNewMile && newMile != mile) {
            SetToScrollMile(true);
        }

        if (!playerFocus && lastScroll != scrollMile) {
            if(scrollMile < 0) {
                //focus on world column
                SPCamera.SetTarget(Vector3.forward * MileTotalScroll);
            } else {
                SPCamera.SetTarget(Vector3.forward * (MileTotalScroll + GameStateComponent.MILE_DISTANCE * .5f));
            }
        }

        lastScroll = scrollMile;
    }

    void UpdateUI() {
        
        if(barUI.value != targetScroll) { barUI.value = Mathf.Lerp(barUI.value, targetScroll, .1f); }
        if (playerUI.value != targetPlayer) { playerUI.value = Mathf.Lerp(playerUI.value, targetPlayer, .1f); }
    }

    void UpdatePlayerPosition() {

        float newMile = PositionComponent.PositionToMile(PlayerMUD.MUDPlayer.Position.Pos);

        Debug.Log($"UPDATE PLAYER: ({playerMile} --> {newMile}) (Max={maxMile})", this);

        playerMile = GetClampedMile(newMile);
        targetPlayer = GetMileLerp(playerMile);

        if (playerMile != lastPlayerMile) {
            SetToPlayerMile(true);
        }

        lastPlayerMile = playerMile;

    }

    public void ToggleTutorial(bool toggle) {
        enabled = !toggle;
    }

    public void CompleteRoad(RoadComponent completed) {

        //don't show this update on mile completions
        if(RoadComponent.CompletedRoadCount % MapConfigComponent.Height == 0) return;

        completedRoad = completed;

        //TODO use ChunkComponent road completed count
        string stepCount = ((int)((RoadComponent.CompletedRoadCount%MapConfigComponent.Height) * 100)).ToString();
        string mileReadout = $"{GameStateComponent.MILE_COUNT} Milia {stepCount} Passus";

        NotificationUI.AddNotification($"Road advanced by {completed.FilledBy.Entity.Name}");

    }

    public void SetToScrollMile() { SetToScrollMile(false);}
    public void SetToPlayerMile() {SetToPlayerMile(false);}
    public void SetToRoadFinisher() {
        if(!completedRoad?.FilledBy) return; 
        SetToObject(completedRoad.FilledBy.PlayerScript.Root, true);
    }

    public void SetToScrollMile(bool withFX) {
        Debug.Log("SCROLL: Scroll", this);
        ToggleCameraOnObject(false, false, null);
        SetMile(Mathf.RoundToInt(scrollMile), withFX);
    }

    public void SetToObject(Transform target, bool withFX) {
        
        scrollMile = GetClampedMile(PositionComponent.PositionToMile(target.position));
        ToggleCameraOnObject(true, false, target);
        SetMile(Mathf.RoundToInt(scrollMile), withFX);

    }

    public void SetToPlayerMile(bool withFX) {

        Debug.Log("SCROLL: Player", this);

        scrollMile = playerMile;

        ToggleCameraOnObject(true, false, PlayerMUD.LocalPlayer.Root);
        SetMile((int)playerMile, withFX);

        mileHeading.UpdateField("Mile " + (int)(playerMile+1));
        mileHeading.ToggleWindowOpen();
        mileUI.ToggleWindowOpen();
        
    }

    public void ToggleCameraOnObject(bool toggle, bool changeZoom, Transform target) {

        if(toggle) {
            MotherUI.FollowTarget(target, changeZoom);
        } else {
            if(playerFocus && changeZoom) SPCamera.SetFOVGlobal(10f);
            SPCamera.SetFollow(null);
            SPCamera.SetTarget(Vector3.forward * (MileTotalScroll + GameStateComponent.MILE_DISTANCE * .5f));
        }

        playerFocus = toggle; 

        playerButton.ToggleWindow(!playerFocus);
        landscapeButon.ToggleWindow(playerFocus);


    }

    public void SetMile(int newMile, bool withFX = false, bool updateScroll = false) {

        Debug.Log($"WORLD: ({mile} --> {newMile}) (Max={maxMile})", this);

        //out of range
        if(newMile > maxMile || newMile < minMile) {
            Debug.LogError("Not valid: " + (int)newMile + " / " + (int)maxMile, this);
            return;
        }

        if(newMile < 0) {

            mileHeading.UpdateField("Polis");
            mileHeading.ToggleWindowOpen();
            mileUI.ToggleWindowOpen();

            recapButton.ToggleWindow(false);

        } else {

            bool loaded = ChunkLoader.LoadMile(newMile);
            if(loaded == false) { return; }

            if(focusedChunk != null) {focusedChunk.ToggleCurrentMile(false);}

            focusedChunk = ChunkLoader.Chunks[newMile];
            focusedChunk.ToggleCurrentMile(true);

            mileHeading.UpdateField("Mile " + (int)(newMile+1));
            mileHeading.ToggleWindowOpen();
            mileUI.ToggleWindowOpen();

            recapButton.ToggleWindow(ChunkLoader.Chunk.Completed);

        }

        mile = newMile;

        if(updateScroll) {
            scrollMile = mile;
            lastScroll = mile;
        }

        if(withFX) {
            // mileNumber.SetActive(true);
            // mileText.text = (newMile+1).ToString();
            SPUIBase.PlaySound(sfx_mile);
        }

        OnMile?.Invoke();

    }

}
