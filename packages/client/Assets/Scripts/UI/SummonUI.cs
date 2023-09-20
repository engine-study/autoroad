using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUI : MonoBehaviour
{
    void Awake() {
        ChunkComponent.OnChunkUpdate += UpdateSummon;
    }
    void OnDestroy() {
        ChunkComponent.OnChunkUpdate -= UpdateSummon;
    }

    void UpdateSummon() {
        gameObject.SetActive(ChunkComponent.ActiveChunk.Spawned == false);
    }

}
