using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using mud.Client;
using IWorld.ContractDefinition;

public class NameOptionUI : MonoBehaviour {

    bool spawning = false; 
    public static string PlayerName;
    public static NameClass Name;
    public NameClass[] names;
    public SPButton[] buttons;
    public AudioClip [] sfx_rollPlayer, sfx_acceptPlayer;

    // bool spawning = false;
    void OnEnable() {
        Roll();
    }

    void OnDisable() {
        Name = null;
    }

    public void Roll() {

        names = new NameClass[5];
        for(int i = 0; i < buttons.Length; i++) {
            names[i] = new NameClass();
            buttons[i].UpdateField(NameUI.TableToName(names[i].first, names[i].second, names[i].third));
        }
        
        SPUIBase.PlaySound(sfx_rollPlayer);

    }

    public async void SpawnPlayer() {

        if(Name == null) {
            return;
        }

        if(spawning) {
            return;
        }

        SPUIBase.PlaySound(sfx_acceptPlayer);
        spawning = true;

        bool didSpawn = await SpawnTx();

        if(didSpawn) {
            
        } else {
            spawning = false;
        }

    }

    public static async UniTask<bool> SpawnTx() {
        return await TxManager.Send<SpawnFunction>(System.Convert.ToUInt32(Name.first), System.Convert.ToUInt32(Name.second), System.Convert.ToUInt32(Name.third));
    }

    public void Selected(int button) {
        PlayerName = buttons[button].Text;
        Name = names[button];
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
