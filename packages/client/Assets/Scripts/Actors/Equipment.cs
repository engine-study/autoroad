using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using mud;

public abstract class Equipment : MonoBehaviour {

    public SPActor Sender {get{return sender;}}
    public SPInteract Interact {get{return interact;}}

    [Header("Item")]
    public GaulItem item;

    [Header("Debug")]
    public MUDComponent ourComponent;
    public bool canUse = false;
    SPActor sender;
    [SerializeField] SPInteract interact;

    protected virtual void Awake() {
        ourComponent = GetComponentInParent<MUDComponent>();
        Interact.OnInteractToggle += UseEquipment;
    }

    void UseEquipment(bool toggle, IActor actor) {
        if(toggle) {
            Use();
        }
    }

    public void SetActor(SPActor newActor) {
        sender = newActor;
    }
    
    public void UseState(bool toggle) {
        interact.Action().DoCast(toggle, sender);
    }

    public virtual bool CanUse() {
        return gameObject.activeInHierarchy && CursorMUD.Entity != PlayerComponent.LocalPlayer.Entity && interact.Action().TryAction(sender, interact);
    }

    public abstract UniTask<bool> Use();

}
