using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUI : SPWindowParent
{
 
    void Update() {
        if(SPUIBase.CanInput && Input.GetMouseButtonDown(0) && SPUIBase.IsPointerOverUIElement == false) {
            (PlayerMUD.LocalPlayer.Controller as ControllerMUD).TeleportMUD(CursorMUD.GridPos);
            ToggleWindowClose();
        }
    }

}
