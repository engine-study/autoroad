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
    public Dictionary<int, InventorySlot> itemDict;

    public bool ItemIsUsable(GaulItem item) {
        InventorySlot slot = GetItemSlot(item);
        return slot != null ? slot.amount > 0 : false;
    }

    public bool ItemUnlocked(GaulItem item) {
        return GetItemSlot(item)?.amount > 0;
    }

    public InventorySlot GetItemSlot(GaulItem item) {
        itemDict.TryGetValue(item.ID, out InventorySlot slot);
        return slot;
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
        itemDict = new Dictionary<int, InventorySlot>();

        for(int i = 0; i < entity.Components.Count; i++) {
            AddValueComponent(entity.Components[i]);
        }

        entity.OnComponentAdded += AddValueComponent;
        entity.OnComponentUpdated += IngestValueComponent;
        
    }

    void OnDestroy() {

        if(entity) {
            entity.OnLoaded -= Init;
            entity.OnComponentAdded -= AddValueComponent;
            entity.OnComponentUpdated -= IngestValueComponent;
        }
    }

    bool IsValueComponent(MUDComponent c) {return c is ValueComponent;}

    void AddValueComponent(MUDComponent c) {
        if(!IsValueComponent(c)) {return;}
        Ingest((ValueComponent)c);
    }

    void IngestValueComponent(MUDComponent c, UpdateInfo i) {
        if(!IsValueComponent(c)) {return;}
        Ingest((ValueComponent)c);
    }

    void Ingest(ValueComponent v) {

        if(v == null) return;
        if(v.Items == null || v.Items.Count == 0) return;

        for(int i = 0; i < v.Items.Count; i++) {
            if(v.Items[i].item == null) continue;
            ToggleItem(true, v.Items[i]);
        }

    }

    void ToggleItem(bool toggle, InventorySlot slot) {

        if(itemDict.ContainsKey(slot.item.ID)) {

        } else {
            itemDict.Add(slot.item.ID, slot);
            items.Add(slot);
        }

        OnUpdated?.Invoke();
        OnUpdatedSlot?.Invoke(slot);
    }



}


[System.Serializable]
public class InventorySlot{
    public GaulItem item;
    public int amount = -1; 

    public float StatToValue(StatType statType) {

        if(statType == StatType.RoadCoin) {
            return item.StatToValue(statType) + amount * item.StatToValue(statType);
        } else {
            return item.StatToValue(statType);
        }

    } 
}