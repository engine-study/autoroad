using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class RewardsUI : EntityUI
{
    [Header("Move")]
    public StatUI reward;

    protected override void UpdateEntity() {
        base.UpdateEntity();

        HasReward [] rewards = Entity.GetComponentsInChildren<HasReward>();

        if(rewards.Length < 0) {
            ToggleWindowClose();
            return;
        }

        UpdateComponent();
    }

    public virtual void UpdateComponent() {
            
    }

}
