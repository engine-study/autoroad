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
    public bool canUse = false;


    public override bool IsInteractable() {

        canUse = base.IsInteractable() && gameObject.activeInHierarchy && CursorMUD.Entity != PlayerComponent.LocalPlayer.Entity && Action().TryAction(Actor, this);
        bool hasItem = canUse && (item == null || Inventory.LocalInventory.HasItem(item));
        bool hasRequiredComponent = canUse && ((requiredComponent == null && CursorMUD.Entity == null) || (CursorMUD.Entity != null && CursorMUD.Entity.ExpectedComponents.Contains(requiredComponent?.GetType())));
        return canUse && hasRequiredComponent && hasItem; 
    }

    protected override void Awake() {
        base.Awake();
        us = GetComponentInParent<MUDComponent>();
        ToggleActor(true, us.GetComponent<IActor>());
    }

    public override void Interact(bool toggle, IActor newActor) {
        base.Interact(toggle, newActor);

        if(toggle) {
            Use();
        }
    }
    
    public void UseState(bool toggle) {
        Action().DoCast(toggle, Actor);
    }
    
    public virtual async UniTask<bool> Use() {

        int x = (int)CursorMUD.GridPos.x;
        int y = (int)CursorMUD.GridPos.z;

        Debug.Log(actionName.ToString() + ": " + x + "," + y);
                
        return await ActionsMUD.ActionTx(PlayerComponent.LocalPlayer.Entity, actionName, new Vector3(x, 0, y));
    }

}
