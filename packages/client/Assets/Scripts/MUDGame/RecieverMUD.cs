using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class RecieverMUD : SPReciever
{
    public GameObject EntityGO;
    
    void Start() {
        CursorMUD.OnHoverEntity += AddActions;
        CursorMUD.OnLeaveEntity += RemoveActions;
    }
    void OnDestroy() {
        CursorMUD.OnHoverEntity -= AddActions;
        CursorMUD.OnLeaveEntity -= RemoveActions;
    }

    void AddActions(Entity newEntity) {
        ToggleActions(true, newEntity);
    }

    void RemoveActions(Entity newEntity) {
        ToggleActions(false, newEntity);
    }

    void ToggleActions(bool toggle, Entity newEntity) {
        
        MUDEntity m = (MUDEntity)newEntity;
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
