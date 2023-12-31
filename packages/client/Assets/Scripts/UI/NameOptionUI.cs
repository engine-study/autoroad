using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using mud;
using IWorld.ContractDefinition;

public class NameOptionUI : SPWindowParent {

    bool spawning = false;
    int selection = -1;

    public GameObject parent;

    public static string PlayerName;
    public static NameClass Name;
    public NameClass[] names;
    public SPButton[] buttons;
    public AudioClip[] sfx_rollPlayer, sfx_acceptPlayer, sfx_select;
    public AudioClip sfx_wagon;
    public SPButton confirm;

    // bool spawning = false;
    protected override void Start() {
        base.Start();

        // Debug.Log( $"Name lengths: {NameUI.adjectives.Length} {NameUI.praenomen.Length} {NameUI.nomen.Length}" );

        Roll();
    }

    public void Roll() {

        names = new NameClass[5];
        
        if(selection > -1) {
            buttons[selection].ButtonText.fontStyle = TMPro.FontStyles.Normal;
        }

        selection = -1;
        confirm.ToggleWindowClose();

        Name = null;

        for (int i = 0; i < buttons.Length; i++) {

            bool duplicateName = true;

            while (duplicateName) {

                names[i] = new NameClass();
                duplicateName = false; 

                for (int j = i-1; j > -1; j--) {
                    if (names[i].first == names[j].first || names[i].second == names[j].second || names[i].third == names[j].third) {
                        duplicateName = true;
                        break;
                    }
                }
                
            }

            buttons[i].UpdateField(NameUI.TableToName(names[i].first, names[i].second, names[i].third));
        }

        SPUIBase.PlaySound(sfx_rollPlayer);

    }

    public async void SpawnPlayer() {

        if (selection < 0 || Name == null) {
            return;
        }

        if (spawning) {
            return;
        }

        SPUIBase.PlaySound(sfx_acceptPlayer);
        spawning = true;

        parent.SetActive(false);

        MotherUI.ToggleNameSelection(false);

        //spawn transaction
        bool didSpawn = await SpawnTx();

        if (didSpawn) {
            // SPUIBase.PlaySound(sfx_wagon);
        } else {
            MotherUI.ToggleNameSelection(true);
            spawning = false;
            parent.SetActive(true);
        }

    }

    public async UniTask<bool> MakeName() {
        Name = new NameClass();
        return await SpawnTx();
    }
    
    public async UniTask<bool> SpawnTx() {
        return await TxManager.SendDirect<NameFunction>(System.Convert.ToUInt32(Name.first), System.Convert.ToUInt32(Name.second), System.Convert.ToUInt32(Name.third));
    }

    public void Selected(int newSelection) {

        if(selection > -1) {
            buttons[selection].ButtonText.fontStyle = TMPro.FontStyles.Normal;
        }

        selection = newSelection;

        PlayerName = buttons[selection].Text;
        Name = names[selection];

        buttons[selection].ButtonText.fontStyle = TMPro.FontStyles.Underline | TMPro.FontStyles.Normal;

        confirm.ToggleWindowOpen();
        confirm.ToggleState(SPSelectableState.Default);

        SPUIBase.PlaySound(sfx_select);
    }

}

public class NameClass {
    public int first;
    public int second;
    public int third;

    public NameClass() {
        first = Random.Range(0, NameUI.adjectives.Length);
        second = Random.Range(0, NameUI.praenomen.Length);
        third = Random.Range(0, NameUI.nomen.Length);
    }
}
