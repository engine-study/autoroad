using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : SPWindowParent
{
    [Header("Menu")]
    [SerializeField] GameObject title;
    [SerializeField] GameObject buttons;
    [SerializeField] SpectateUI spectate;

    protected override void Awake() {
        base.Awake();
        spectate.gameObject.SetActive(false);
    }

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
