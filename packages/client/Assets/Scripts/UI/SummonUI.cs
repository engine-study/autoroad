using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUI : SPWindow
{
    protected override void Awake() {
        base.Awake();
        ChunkComponent.OnChunkUpdate += UpdateSummon;
        UpdateSummon();
    }
    protected override void OnDestroy() {
        base.OnDestroy();
        ChunkComponent.OnChunkUpdate -= UpdateSummon;
    }

    public void UpdateSummon() {
        gameObject.SetActive(ChunkComponent.ActiveChunk?.Spawned == false);
    }

}
