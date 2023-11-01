using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnUI : SPWindowParent {

    public SPButton button;

    public override void ToggleWindow(bool toggle) {
        base.ToggleWindow(toggle);

        if(toggle) {
            button.ToggleWindowClose();
            StartCoroutine(ShowRespawn());
        }

    }

    IEnumerator ShowRespawn() {
        yield return new WaitForSeconds(2.5f);
        button.ToggleWindowOpen();
    }
    public void Respawn() {
        ToggleWindowClose();
        MotherUI.TogglePlayerSpawning(true);
    }
}
