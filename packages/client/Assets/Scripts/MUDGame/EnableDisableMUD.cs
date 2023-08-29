using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class EnableDisableMUD : SPEnableDisable
{

    [Header("MUD")]
    [SerializeField] bool hasInit = false;
    [SerializeField] MUDComponent component;

    public override bool CanPlay(bool enable, SPEffects effect) { return base.CanPlay(enable, effect) && hasInit && component.Loaded && (!enable || hasPlayed || (!hasPlayed && component.SpawnInfo.Source != SpawnSource.Load));}

    protected override void Awake() {
        base.Awake();
        Init();
    }

    void Init() {

        if(hasInit) {
            return;
        }

        if(component == null) {
            component = GetComponentInParent<MUDComponent>();
            if (component == null) { Debug.LogError("No component", this); return; }
        }

        hasInit = true;
    }   



}
