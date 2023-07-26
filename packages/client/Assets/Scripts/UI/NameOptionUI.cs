using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameOptionUI : MonoBehaviour {

    bool spawning = false; 
    public static string PlayerName;
    public NameClass selectedName;
    public NameClass[] names;
    public SPButton[] buttons;
    public AudioClip [] sfx_rollPlayer, sfx_acceptPlayer;

    // bool spawning = false;
    void OnEnable() {
        Roll();
    }

    public void Roll() {

        names = new NameClass[5];
        for(int i = 0; i < buttons.Length; i++) {
            names[i] = new NameClass();
            buttons[i].UpdateField(NameUI.TableToName(names[i].first, names[i].second, names[i].third));
        }
        
        SPUIBase.PlaySound(sfx_rollPlayer);

    }

    public void SpawnPlayer() {

        if(spawning) {
            return;
        }

        spawning = true;
        SPUIBase.PlaySound(sfx_acceptPlayer);
        // TxManager
    }

    public void Selected(int button) {
        PlayerName = buttons[button].Text;
    }

}

public class NameClass {
    public int first;
    public int second;
    public int third;

    public NameClass() {
        first = Random.Range(0, NameUI.praenomen.Length);
        second = Random.Range(0, NameUI.nomen.Length);
        third = Random.Range(0, NameUI.cognomina.Length);
    }
}
