using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MileColumn : MonoBehaviour
{

    public int Mile {get{return Mathf.RoundToInt(mile);}}
    public float MileRaw {get{return mile;}}
    public bool IsInputting {get{return mouseInput || scrollInput;}}

    [Header("Column")]
    [SerializeField] float segmentHeight = 1.5f;
    [SerializeField] Transform segmentParent;
    [SerializeField] Transform columnTop;
    [SerializeField] MileSegment segmentPrefab;
    public Dictionary<int, MileSegment> Segments = new Dictionary<int, MileSegment>();


    [Header("Rotate")]
    [SerializeField] Transform camera;
    [SerializeField] float horizontalSpeed = 2.0f;
    [SerializeField] float wheelSpeed = 10.0f;
    [SerializeField] float verticalSpeed = 2.0f;
    [SerializeField] float drag = 1.0f;
    [SerializeField] float round = 180f;

    [Header("Debug")]
    [SerializeField] float mile;

    float scroll, lastScroll;
    float velocity;
    float totalRot;
    float lerpRot;
    float roundedRot;
    Vector3 startPos;
    float axis, startAxis = 0f;
    bool mouseInput, scrollInput;

    void Awake() {

        segmentPrefab.gameObject.SetActive(false);

        ChunkLoader.OnChunkRegistered += AddChunk;
        GameStateComponent.OnGameStateUpdated += UpdateProgress;
        GameStateComponent.OnMileCompleted += UpdateComplete;
    }

    void OnDestroy() {
        ChunkLoader.OnChunkRegistered -= AddChunk;
        GameStateComponent.OnGameStateUpdated -= UpdateProgress;
        GameStateComponent.OnMileCompleted -= UpdateComplete;
    }
    public void SetMile(float newMile) {
        totalRot = Mathf.Clamp(newMile * round, -.25f * round, (GameStateComponent.MILE_COUNT + .25f) * round);
    }

    public void SetRot(float newMile) {
        lerpRot = newMile * round;
        camera.localRotation = Quaternion.Euler(Vector3.up * newMile * round);
        camera.localPosition = Vector3.down * newMile * round * verticalSpeed;
    }

    void AddChunk(ChunkComponent newChunk) {
        int mile = newChunk.MileNumber;
        MileSegment ms = Instantiate(segmentPrefab, segmentParent.position + Vector3.up * mile * segmentHeight, Quaternion.Euler(0f, (mile % 2) * 180f,0f), segmentParent);
        
        ms.gameObject.SetActive(true);
        ms.ToggleFinished(mile < GameStateComponent.MILE_COUNT);

        Segments.Add(mile, ms);
    }

    void UpdateProgress() {
        columnTop.position = segmentParent.position + Vector3.up * GameStateComponent.MILE_COUNT * segmentHeight;
    }

    void UpdateComplete(int completedMile) {

        Segments.TryGetValue(completedMile, out MileSegment ms);
        if(ms == null) {Debug.LogError("Could not find segment " + completedMile, this); return;}
        ms.ToggleFinished(true);
        
    }

    void Update() {

        lastScroll = scroll;
        axis = Input.GetAxis("Mouse X");

        mouseInput = false && Input.GetMouseButton(0);
        scrollInput = Input.mouseScrollDelta.y != 0f;

        if(mouseInput) {
            scroll = Mathf.Lerp(scroll, horizontalSpeed * -(axis - startAxis) * Time.deltaTime, .25f);
        } else if(scrollInput) {
            startAxis = axis;
            scroll = Mathf.Lerp(scroll, wheelSpeed * Input.mouseScrollDelta.y * Time.deltaTime, .25f);
        } else {
            startAxis = axis;
            scroll = Mathf.MoveTowards(scroll, 0f, Time.deltaTime * drag);
        }

        // float newVel = (scroll - lastScroll) / Time.deltaTime;
        // velocity = Mathf.Lerp(velocity, Mathf.Clamp01(newVel - drag), .25f);

        totalRot = Mathf.Clamp(totalRot + scroll, round * -.25f, (GameStateComponent.MILE_COUNT + .25f) * round);
        lerpRot = Mathf.Lerp(lerpRot, totalRot, .1f);
        roundedRot = Mathf.Round(totalRot / round) * round;

        mile = lerpRot/round;

        SetRot(mile);

    }

}
