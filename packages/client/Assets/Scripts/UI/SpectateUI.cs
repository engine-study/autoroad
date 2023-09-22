using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectateUI : SPWindowParent
{
    [SerializeField] MainMenuUI menu;

    public void ToggleSpectate(bool toggle) {
        ToggleWindow(toggle);
        menu.ToggleWindow(!toggle);
    }
}
