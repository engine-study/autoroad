using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class RecieverMUD : SPReciever
{
    public GameObject EntityGO;
    
    void Start() {
        SPEvents.OnLocalPlayerSpawn += StartReceiver;
    }

    void StartReceiver() {
        SPEvents.OnLocalPlayerSpawn -= StartReceiver;
        
        CursorMUD.OnHoverEntity += AddActions;
        CursorMUD.OnLeaveEntity += RemoveActions;
    }

    void OnDestroy() {
        SPEvents.OnLocalPlayerSpawn -= StartReceiver;
        CursorMUD.OnHoverEntity -= AddActions;
        CursorMUD.OnLeaveEntity -= RemoveActions;
    }

    void AddActions(MUDEntity newEntity) {
        ToggleActions(true, newEntity);
    }

    void RemoveActions(MUDEntity newEntity) {
        ToggleActions(false, newEntity);
    }

    void ToggleActions(bool toggle, MUDEntity newEntity) {

        mud.Client.MUDEntity m = (mud.Client.MUDEntity)newEntity;
        if(m == null) {
            return;
        }

        PlayerComponent player = m.GetMUDComponent<PlayerComponent>();
        if(player && player.IsLocalPlayer) {
            return;
        }

        SPInteract [] interacts = m.GetComponentsInChildren<SPInteract>(true);
        if(interacts == null) {
            return;
        }

        for(int i = 0; i < interacts.Length; i++) {

            //these interacts havent init yet
            if(interacts[i].GameObject() == null) {
                continue;
            }

            if(toggle) {
                bool canDoAction = interacts[i].gameObject.activeInHierarchy && interacts[i].Action().TryAction(PlayerMUD.LocalPlayer.Actor, interacts[i]);
                ToggleInteractable(canDoAction, interacts[i]);
            } else {
                ToggleInteractable(false, interacts[i]);
            }
        }

    }

    void Update() {
        
    }
}
