using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : SPWindowParent
{
    [Header("Menu")]
    [SerializeField] GameObject title;
    [SerializeField] GameObject buttons;
    [SerializeField] SpectateUI spectate;

    public void Play() {
        GameState.PlayGame();
    }

    public void Spectate() {
        spectate.ToggleSpectate(true);
    }

    public void BackToMenu() {
        spectate.ToggleSpectate(false);
    }

}
