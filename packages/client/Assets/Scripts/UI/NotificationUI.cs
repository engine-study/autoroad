using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : SPWindowParent
{
    public static NotificationUI Instance;
    public List<SPButton> notifications;

    public override void Init() {
        if(HasInit) return;
        base.Init();

        Instance = this;

        for(int i = 0; i < notifications.Count; i++) {
            notifications[i].ToggleWindowClose();
        }
    }

    public static void AddNotification(string newText) {
        if(Instance == null) return;
        Instance.ToggleNotification(newText);
    }

    public void ToggleNotification(string newText) {
        SPButton newButton = null;
        for(int i = 0; i < notifications.Count; i++) {
            if(notifications[i].gameObject.activeInHierarchy == false) { 
                newButton = notifications[i]; 
                break;
            }
        }

        if(newButton == null) {return;}

        newButton.UpdateField(newText);
        newButton.ToggleWindowOpen();
        
    }

    protected override void Destroy() {
        base.Destroy();
        Instance = null;
    }
}
