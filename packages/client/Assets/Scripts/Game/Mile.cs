using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Mile : MonoBehaviour
{
    public RowComponent[] Rows { get { return rows; } }

    [Header("Mile")]
    [SerializeField] Transform roadParent;
    [SerializeField] GameObject roadMesh;
    [SerializeField] Transform terrainParent;
    [SerializeField] Transform groundParent;
    [SerializeField] Transform groundLeft, groundRight;
    [SerializeField] Transform spawnLeft, spawnRight;
    [SerializeField] Transform rowParent;
    [SerializeField] Borders border;
    [SerializeField] GameObject [] highlights;
    [SerializeField] RowComponent rowPrefab;
    [SerializeField] RowComponent[] rows;

    [Header("Debug")]
    [SerializeField] ChunkComponent chunk; 

    bool isRealChunk = false; 
    int rowTotal;
    int widthSize;
    bool startedInit = false;

    void Start() {
        Init();
    }

    public void Init(ChunkComponent newChunk) {
        chunk = newChunk;
        Init();
    }

    public void Toggle(bool toggle) {
        rowParent.gameObject.SetActive(toggle);
        roadMesh.SetActive(!toggle);
    }

    public void Highlight(bool toggle) {
        border.gameObject.SetActive(toggle);
        for(int i = 0; i < highlights.Length; i++) {highlights[i].SetActive(toggle);}
    }

    public async void Init() {
        if(startedInit) return;
        startedInit = true;
        await SetupMileAsync();
    }

    async UniTask SetupMileAsync() {

        while (MapConfigComponent.Instance == null || RoadConfigComponent.Instance == null) { await UniTask.Delay(100); }

        Highlight(false);

        isRealChunk = chunk != null;
        roadMesh.SetActive(!isRealChunk);

        if(isRealChunk) {
                    
            rowTotal = MapConfigComponent.Height;
            rows = new RowComponent[rowTotal];
            for (int i = 0; i < rowTotal; i++)
            {
                RowComponent newRow = Instantiate(rowPrefab, transform.position + Vector3.forward * (i+.5f), Quaternion.identity, rowParent);
                newRow.chunk = chunk;
                newRow.name = "Row " + i;
                newRow.SpawnRoad(RoadConfigComponent.Width);

                rows[i] = newRow;
            }
        } else {

        }


        border.SetBorder(new Vector4(-MapConfigComponent.Width, MapConfigComponent.Width, MapConfigComponent.Height-1, 0));

        roadParent.localScale = Vector3.one + Vector3.right * (RoadConfigComponent.Width-1f);
        groundParent.localScale = Vector3.one + Vector3.forward * (MapConfigComponent.Height-1f);
        terrainParent.localScale = Vector3.one + Vector3.right * (MapConfigComponent.SpawnWidth * 2 + 1);

        groundLeft.localPosition = Vector3.right * (RoadConfigComponent.Left - .5f);
        groundRight.localPosition = Vector3.right * (RoadConfigComponent.Right + .5f);

        groundLeft.localScale = Vector3.one + Vector3.right * (MapConfigComponent.Width - RoadConfigComponent.Right - 1f);
        groundRight.localScale = Vector3.one + Vector3.right * (MapConfigComponent.Width - RoadConfigComponent.Right - 1f);

        spawnLeft.localPosition = Vector3.right * (-MapConfigComponent.Width - .5f);
        spawnRight.localPosition = Vector3.right * (MapConfigComponent.Width + .5f);
        spawnLeft.localScale = Vector3.one + Vector3.right * (MapConfigComponent.SpawnWidth - MapConfigComponent.Width - 1f);
        spawnRight.localScale = Vector3.one + Vector3.right * (MapConfigComponent.SpawnWidth - MapConfigComponent.Width - 1f);
    }

    public void AddRoadComponent(string entity, RoadComponent c, int x, int y) {
        // Debug.Log("Adding [" + x + "," + y + "] Rows: " + rows.Length + " Mile: " + c.Mile, c);
        rows[y].SetRoadBlock(entity, x + RoadConfigComponent.Right, c);
    }
}
