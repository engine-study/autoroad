using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class HoverPolish : MonoBehaviour
{
    
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

        SPBase baseObject = newEntity.GetComponentInChildren<SPBase>();

        if(baseObject != null) {
            if(baseObject is PlayerMUD) {
                TogglePlayer(toggle, baseObject as PlayerMUD);
            }
        }

    }

    void TogglePlayer(bool toggle, PlayerMUD player) {
        player.Animator.ik.SetLook(toggle ? SPUIBase.Camera.transform : null);
    }
}
