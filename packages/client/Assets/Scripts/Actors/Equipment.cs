using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using mud.Client;
public abstract class Equipment : MonoBehaviour
{

    public SPActor Sender {get{return sender;}}
    public SPInteract Interact {get{return interact;}}

    [Header("Debug")]
    public bool canUse = false;
    SPActor sender;
    [SerializeField] SPInteract interact;

    void Awake() {
        Interact.OnInteractToggle += UseEquipment;
    }

    void UseEquipment(bool toggle, IActor actor) {
        if(toggle) {
            Use();
        }
    }

    public void SetSender(SPActor newActor) {
        sender = newActor;
    }
    


    public virtual bool CanUse() {
        return gameObject.activeInHierarchy && interact.Action().TryAction(sender, interact);
    }

    public abstract UniTask<bool> Use();

}
