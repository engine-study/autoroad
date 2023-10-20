using System.Collections;
using System.Collections.Generic;
using mud;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [Header("Debug")]
    public MUDEntity entity;
    public List<InventorySlot> items;
    public Dictionary<string, InventorySlot> itemDict;

    void Start() {
        entity = GetComponentInParent<MUDEntity>();
        if(entity == null) return;

        for(int i = 0; i < entity.Components.Count; i++) {
            AddToInventory(entity.Components[i]);
        }

        entity.OnComponentAdded += AddToInventory;
        entity.OnComponentUpdated += UpdateInventory;
    }

    void OnDestroy() {
        entity.OnComponentAdded -= AddToInventory;
        entity.OnComponentUpdated -= UpdateInventory;
    }

    void AddToInventory(MUDComponent c) {
        if(c.GetType() != typeof(ValueComponent)) {return;}
        Ingest((ValueComponent)c);
    }

    void UpdateInventory(MUDComponent c, UpdateInfo i) {
        if(c.GetType() != typeof(ValueComponent)) {return;}
        Ingest((ValueComponent)c);
    }

    void Ingest(ValueComponent v) {

        if(v == null) return;
        if(v.Item == null) return;

        InventorySlot slot = null;

        if(itemDict.ContainsKey(v.Item.name)) {
            slot = itemDict[v.Item.name];
        } else {
            slot = new InventorySlot { item = v.Item};

            itemDict.Add(v.Item.name, slot);
            items.Add(slot);
        }

        slot.amount = (int)v.Value;        

    }



}


[System.Serializable]
public class InventorySlot{
    public GaulItem item;
    public int amount = -1; 
}