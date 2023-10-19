using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;

public class HoverPolish : MonoBehaviour
{
    
    void Start() {
        SPEvents.OnServerLoaded += Init;
    }

    void Init() {
        CursorMUD.OnHoverEntity += AddActions;
        CursorMUD.OnLeaveEntity += RemoveActions;
    }

    void OnDestroy() {
        SPEvents.OnServerLoaded -= Init;
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
        
        ProfileUI.Instance.SetEntity(newEntity);

        if(newEntity == null) {
            return;
        }


        SPBase baseObject = newEntity.GetComponentInChildren<SPBase>();

        if(baseObject != null) {
            if(baseObject is PlayerMUD) {
                HoverPlayer(toggle, baseObject as PlayerMUD);
            }
        }

    }

    void HoverPlayer(bool toggle, PlayerMUD player) {
        if(player.IsLocalPlayer) {

        } else {
            // player.Animator.IK.SetLook(toggle ? SPUIBase.Camera.transform : null);
        }
    }
}
