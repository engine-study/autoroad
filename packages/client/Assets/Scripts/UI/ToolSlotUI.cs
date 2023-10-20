using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class ToolSlotUI : SPWindow
{
    [Header("Slot")]
    public SPButton button;
    public SPHoverDescription hoverText;
    public GameObject selected;
    public SPInputPrompt input;
    public CanvasGroup group;

    [Header("Debug")]
    public Inventory inv;
    public Equipment equipment;
    public InventorySlot invSlot;


    public void ToggleSelected(bool toggle) {
        selected.SetActive(toggle);
        input.ToggleWindow(!toggle);
    }

    public void Setup(Inventory i) {
        inv = i;
        inv.OnUpdated += UpdateCanUse;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        if(inv) inv.OnUpdated -= UpdateCanUse;
    }

    public void SetEquipment(Equipment e) {

        equipment = e;

        if(e.item == null) {Debug.LogError($"{e.name} does not have an item."); return;}

        button.Image.sprite = e.item.itemSprite ?? button.Image.sprite;
        hoverText.description = e.item.FullDescription();

        UpdateCanUse();
        UpdateVisibility();
    }

    bool CanDisplay() {
        return inv.HasItem(equipment.item);
    }

    bool CanUse() {
         return equipment.IsInteractable();
    }

    void UpdateCanUse() {
        UpdateCanUse(CanUse());
    }

    void UpdateCanUse(bool toggle) {
        group.alpha = toggle ? 1f : .5f;
    }

    void UpdateVisibility() {
        ToggleWindow(CanDisplay());
    }


   
}
