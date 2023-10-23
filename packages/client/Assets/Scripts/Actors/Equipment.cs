using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using mud;

public class Equipment : SPInteract {

    public string Name {get{return item.Name;}}
    public MUDEntity Entity {get{return us?.Entity;}}
    public GameObject Sender {get{return Actor.Owner().gameObject;}}

    [Header("Item")]
    public ActionName actionName;
    public GaulItem item;
    public MUDComponent requiredComponent;

    [Header("Debug")]
    [SerializeField] protected MUDComponent us; //could be player, barbarian, soldier, etc.
    [SerializeField] protected bool canUse = false;


    public override bool IsInteractable() {

        canUse = base.IsInteractable() && MapConfigComponent.OnWorld(transform.position) && gameObject.activeInHierarchy && CursorMUD.Entity != PlayerComponent.LocalPlayer.Entity && Action().TryAction(Actor, this);
        bool hasItem = canUse && (item == null || Inventory.LocalInventory.ItemIsUsable(item));
        bool hasRequiredComponent = canUse && ((requiredComponent == null && CursorMUD.Entity == null) || (CursorMUD.Entity != null && CursorMUD.Entity.ExpectedComponents.Contains(requiredComponent?.GetType())));
        canUse = canUse && hasRequiredComponent && hasItem;
        return canUse; 
    }

    protected override void Awake() {
        base.Awake();
        us = GetComponentInParent<MUDComponent>();
        ToggleActor(true, us.GetComponent<IActor>());
    }

    public override void Interact(bool toggle, IActor newActor) {
        base.Interact(toggle, newActor);

        if(toggle) {
            SendTx();
        }
    }
    
    public void UseState(bool toggle) {
        Action().DoCast(toggle, Actor);
    }
    
    public virtual async UniTask<bool> SendTx() {

        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.z);

        Debug.Log(actionName.ToString() + ": " + x + "," + y);
                
        return await ActionsMUD.ActionTx(PlayerComponent.LocalPlayer.Entity, actionName, new Vector3(x, 0, y));
    }

}
