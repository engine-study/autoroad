using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mile : MonoBehaviour
{
    public RowComponent[] Rows { get { return rows; } }

    [Header("Mile")]
    [SerializeField] Transform groundParent;
    [SerializeField] Transform groundLeft, groundRight;
    [SerializeField] Transform spawnLeft, spawnRight;
    [SerializeField] RowComponent rowPrefab;
    [SerializeField] RowComponent[] rows;

    [Header("Debug")]
    [SerializeField] ChunkComponent chunk; 

    bool isRealChunk = false; 
    int rowTotal;
    int widthSize;

    public void SetupMile(ChunkComponent newChunk) {

        if (MapConfigComponent.Instance == null || RoadConfigComponent.Instance == null) { Debug.LogError("Can't setup chunk"); return; }

        chunk = newChunk;
        isRealChunk = chunk != null;

        if(isRealChunk) {
                    
            rowTotal = MapConfigComponent.Height;
            rows = new RowComponent[rowTotal];
            for (int i = 0; i < rowTotal; i++)
            {
                RowComponent newRow = Instantiate(rowPrefab, transform.position + Vector3.forward * (i+.5f), Quaternion.identity, transform);
                newRow.chunk = chunk;
                newRow.name = "Row " + i;
                newRow.SpawnRoad(RoadConfigComponent.Width);

                rows[i] = newRow;
            }
        }  

        groundParent.localScale = Vector3.one + Vector3.forward * (rowTotal-1);

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
        // Debug.Log("Adding " + x + " " + y + " Rows: " + rows.Length + " Mile: " + c.Mile + " " + mileNumber, c);
        rows[y].SetRoadBlock(entity, x + RoadConfigComponent.Right, c);
    }
}
