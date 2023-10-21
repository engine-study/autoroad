using System.Collections;
using System.Collections.Generic;
using mud;
using System;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static Inventory LocalInventory;
    public Action OnUpdated;
    public Action<InventorySlot> OnUpdatedSlot;

    [Header("Debug")]
    public bool localInventory;
    public MUDEntity entity;
    public List<InventorySlot> items;
    public Dictionary<string, InventorySlot> itemDict;

    public bool HasItem(GaulItem item) {
        itemDict.TryGetValue(item.Name, out InventorySlot slot);
        return slot?.amount > 0;
    }

    void Awake() {

        entity = GetComponentInParent<MUDEntity>();
        if(entity == null) return;

        if(entity.Loaded) Init();
        else entity.OnLoaded += Init;

    
    }

    void Init() {

        if(entity.IsLocal) {LocalInventory = this; localInventory = true;}

        items = new List<InventorySlot>();
        itemDict = new Dictionary<string, InventorySlot>();

        for(int i = 0; i < entity.Components.Count; i++) {
            AddToInventory(entity.Components[i]);
        }

        entity.OnComponentAdded += AddToInventory;
        entity.OnComponentUpdated += UpdateInventory;
        
    }

    void OnDestroy() {

        if(entity) {
            entity.OnLoaded -= Init;
            entity.OnComponentAdded -= AddToInventory;
            entity.OnComponentUpdated -= UpdateInventory;
        }
    }

    void AddToInventory(MUDComponent c) {
        if(c.GetType().IsSubclassOf(typeof(ValueComponent)) == false) {return;}
        Ingest((ValueComponent)c);
    }

    void UpdateInventory(MUDComponent c, UpdateInfo i) {
        if(c.GetType().IsSubclassOf(typeof(ValueComponent)) == false) {return;}
        Ingest((ValueComponent)c);
    }

    void Ingest(ValueComponent v) {

        if(v == null) return;
        if(v.Item == null) return;

        InventorySlot slot = null;

        if(itemDict.ContainsKey(v.ItemName)) {
            slot = itemDict[v.ItemName];
        } else {
            slot = new InventorySlot { item = v.Item};

            itemDict.Add(v.ItemName, slot);
            items.Add(slot);
        }

        slot.amount = (int)v.Value;        

        OnUpdated?.Invoke();
        OnUpdatedSlot?.Invoke(slot);
    }



}


[System.Serializable]
public class InventorySlot{
    public GaulItem item;
    public int amount = -1; 
}