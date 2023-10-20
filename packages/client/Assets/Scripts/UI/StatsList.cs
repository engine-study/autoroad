using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using System;

public class StatsList : EntityUI
{
    [Header("Stats")]
    [SerializeField] bool showRewards;
    [SerializeField] bool showWeight;
    [SerializeField] MoveTypeUI moveUI;
    [SerializeField] StatUI statPrefab;
    [SerializeField] StatType[] visibleStats;
    StatType[] rewardStats = new StatType[2] {StatType.RoadCoin, StatType.Gem};

    [Header("Debug")]
    [SerializeField] ValueComponent component;

    [EnumNamedArray( typeof(StatType) )]
    [SerializeField] StatUI [] stats;
    [SerializeField] HasReward [] rewards;

    public override void Init() {
        if(hasInit) {return;}
        base.Init();

        moveUI.ToggleWindowClose();
        statPrefab.ToggleWindowClose();

        stats = new StatUI[(int)StatType._Count];
    }

    protected override void UpdateEntity() {
        base.UpdateEntity();

        if(visibleStats.Length > 0) {ShowStats();}
        if(showWeight) {ShowMove();}
        if(showRewards) {ShowRewards();}

        bool showRect = false;
        for(int i = 0; i < rect.childCount; i++) {
            if(rect.GetChild(i).gameObject.activeSelf) {showRect = true; break;}
        }

        ToggleWindow(showRect);

    }

    public void ShowMove() {
        moveUI.SetEntity(Entity);
    }

    public void SetupStat(bool toggle, StatType stat) {
        component = (ValueComponent)Entity.GetMUDComponent(StatUI.StatToComponent(stat));
        SetStat(component != null, stat, component?.Value ?? 0);
    }

    public void SetStat(bool toggle, StatType stat, float value) {

        if(stat == StatType.None) {return;}

        if(toggle) {
            //find or create a stat
            int index = (int)stat;
            if(stats[index] == null) { stats[index] = Instantiate(statPrefab, rect);}

            StatUI statUI = stats[index];
            statUI.SetValue(stat, value);
            statUI.ToggleWindowOpen();

        } else {
            //hide a stat if it exists
            StatUI statUI = stats[(int)stat];
            if(statUI) { statUI.ToggleWindowClose();}
        }


    }

    public void ShowRewards() {

        rewards = Entity.GetComponentsInChildren<HasReward>();

        if(rewards.Length > 1) {Debug.Log("Multiple HasRewards", Entity);}
        if(rewards.Length > 0) { 
            
            HasReward reward = rewards[0];

            for(int i = 0; i < rewardStats.Length; i++) { 
                float value = reward.Value.Value.StatToValue(rewardStats[i]);
                SetStat(value > 0, rewardStats[i], value);
            }
        } else {
            for(int i = 0; i < rewardStats.Length; i++) { SetStat(false, rewardStats[i], 0);}
        }

    }

    public void ShowStats() {

        for(int i = 0; i < visibleStats.Length; i++) { 
            SetupStat(true, visibleStats[i]);
        }

    }

}
