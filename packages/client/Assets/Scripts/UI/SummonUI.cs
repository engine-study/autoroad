using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUI : SPWindow
{
    public override void Init() {
        if(HasInit) {return;}
        base.Init();

        ToggleWindowOpen();
        ToggleWindowClose();

        UpdateSummon();
        ChunkComponent.OnChunkUpdate += UpdateSummon;
    }
    protected override void Destroy() {
        base.Destroy();
        ChunkComponent.OnChunkUpdate -= UpdateSummon;
    }

    public void UpdateSummon() {
        if(!HasInit) return;
        ToggleWindow(ChunkLoader.ActiveChunk?.Spawned == false);
    }

}
