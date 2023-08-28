using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class EnableDisableMUD : SPEnableDisable
{

    [Header("MUD")]
    [SerializeField] bool hasInit = false;
    [SerializeField] MUDComponent component;

    public override bool CanPlay(SPEffects effect) { return base.CanPlay(effect) && hasInit && component.Loaded && component.SpawnInfo.Source == SpawnSource.InGame;}

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
