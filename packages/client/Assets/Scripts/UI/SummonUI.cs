using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUI : SPWindow
{
    protected override void Awake() {
        base.Awake();
        UpdateSummon();
        ChunkComponent.OnChunkUpdate += UpdateSummon;
    }
    protected override void OnDestroy() {
        base.OnDestroy();
        ChunkComponent.OnChunkUpdate -= UpdateSummon;
    }

    public void UpdateSummon() {
        gameObject.SetActive(ChunkLoader.ActiveChunk?.Spawned == false);
    }

}
