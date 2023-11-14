using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : SPWindowParent
{
    public static bool IsNewGame;

    [Header("Menu")]
    [SerializeField] GameObject title;
    [SerializeField] GameObject buttons;
    [SerializeField] SpectateUI spectate;

    [Header("Buttons")]
    [SerializeField] SPButton newGameButton;
    [SerializeField] SPButton continueButton;

    protected override void OnEnable() {
        base.OnEnable();

        continueButton.ToggleWindow(AccountUI.HAS_SIGNED);
        newGameButton.ToggleWindow(true);
    }

    public void Play() {
        IsNewGame = false;
        GameState.PlayGame();
        ToggleWindowClose();
    }

    public void PlayNewGame() {
        IsNewGame = true;
        GameState.PlayGame();
        ToggleWindowClose();
    }

    public void Spectate() {
        spectate.ToggleSpectate(true);
    }

    public void BackToMenu() {
        spectate.ToggleSpectate(false);
    }

}
