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
        
        if(newEntity == null) {
            return;
        }

        SPInteract [] interacts = newEntity.GetComponentsInChildren<SPInteract>();

        if(interacts == null) {
            return;
        }

        for(int i = 0; i < interacts.Length; i++) {
            ToggleInteractable(toggle, interacts[i]);
        }

    }

    void Update() {
        
    }
}
