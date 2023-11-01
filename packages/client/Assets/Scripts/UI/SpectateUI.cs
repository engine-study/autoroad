using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectateUI : SPWindowParent
{
    [SerializeField] MainMenuUI menu;

    void Update() {
        
        if(Input.GetKeyDown(KeyCode.Escape)) {
            ToggleSpectate(false);
        }   

    }
    public void ToggleSpectate(bool toggle) {
        ToggleWindow(toggle);
        menu.ToggleWindow(!toggle);
    }
}
