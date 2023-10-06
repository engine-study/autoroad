using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using System.Linq;

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

        MUDEntity m = newEntity;
        if(m == null) {
            return;
        }

        PlayerComponent player = m.GetMUDComponent<PlayerComponent>();
        if(player && player.IsLocalPlayer) {
            return;
        }

        List<SPInteract> interacts;
        SPActionProvider ap = m.GetComponentInChildren<SPActionProvider>();

        if(ap) { interacts = ap.Interacts;} 
        else { interacts = m.GetComponentsInChildren<SPInteract>(true).ToList();}
        
        if(interacts == null) { return;}

        for(int i = 0; i < interacts.Count; i++) {

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
