using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : SPWindowParent
{
    [Header("Player")]
    public SPActionUI actions;
    public LevelUI level;
    public SPBar slider;

    public override void Init() {
        if(HasInit) {return;}
        base.Init(); 

        SPEvents.OnLocalPlayerSpawn += SetupPlayer;
        XPComponent.OnLocalXPUpdate += UpdateXP;
        XPComponent.OnLocalLevelUp += UpdateLevel;

        slider.SetFill(0f);

    }
    
    protected override void Awake() {
        base.Awake();

    }

    public void ShowLevel() {
        slider.ToggleWindowOpen();
        level.ToggleWindowOpen();
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        SPEvents.OnLocalPlayerSpawn -= SetupPlayer;
        XPComponent.OnLocalXPUpdate -= UpdateXP;
        XPComponent.OnLocalLevelUp -= UpdateLevel;

    }

    void SetupPlayer() {
        level.SetEntity(PlayerComponent.LocalPlayer.Entity);
        actions.Setup(SPPlayer.LocalPlayer.Actor);
    }


    void UpdateXP() {
        slider.ToggleWindowOpen();
        slider.SetFill(XPComponent.XPToLevelProgress(XPComponent.LocalXP));
        SPStrobeUI.ToggleStrobe(slider);
    }
    
    void UpdateLevel() {

        UpdateXP();

        level.ToggleWindowOpen();
        SPStrobeUI.ToggleStrobe(level);
    }
}
