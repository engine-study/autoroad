using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : PhaseUI
{
    public TextMeshProUGUI phaseText;
    public void UpdatePhase(GamePhase newPhase) {
        phaseText.text = newPhase.ToString() + " Phase";
    }
}
