using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : SPWindowParent
{
    public void ToggleQuality() {
        GameState.Instance.ToggleQuality();
    }
}
