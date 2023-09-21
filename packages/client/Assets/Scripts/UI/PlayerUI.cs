using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public SPActionUI actions;
    public SPButton level;
    public SPBar slider;
    void Awake() {
        SPEvents.OnLocalPlayerSpawn += SetupPlayer;
        XPComponent.OnLocalXPUpdate += UpdateXP;
        XPComponent.OnLocalLevelUp += UpdateLevel;

        slider.SetFill(0f);
        level.UpdateField("0");
    }

    void OnDestroy() {
        SPEvents.OnLocalPlayerSpawn -= SetupPlayer;
        XPComponent.OnLocalXPUpdate -= UpdateXP;
        XPComponent.OnLocalLevelUp -= UpdateLevel;

    }

    void SetupPlayer() {

        
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
        level.UpdateField(XPComponent.LocalLevel.ToString());
        SPStrobeUI.ToggleStrobe(level);
    }
}
