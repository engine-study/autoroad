using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSlotUI : SPWindow
{
    [Header("Slot")]
    public SPButton button;
    public SPHoverDescription hoverText;
    public GameObject selected;
    public SPInputPrompt input;
    public StatUI number;
    public GameObject unlocked;
    public CanvasGroup group;
    public Sprite unknown;

    [Header("Debug")]
    public bool HasEnough;
    public bool Unlocked;
    public bool Usable;
    public Inventory inv;
    public Equipment equipment;
    public InventorySlot invSlot;

    public void ToggleSelected(bool toggle) {
        selected.SetActive(toggle);
        input.ToggleWindow(!toggle);
    }

    public void Setup(Inventory i) {
        inv = i;
        inv.OnUpdated += UpdateDisplay;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        if(inv) inv.OnUpdated -= UpdateDisplay;
    }

    public void SetEquipment(Equipment e) {

        equipment = e;
        if(e.item == null) {Debug.LogError($"{e.name} does not have an item."); return;}


        UpdateDisplay();
    }

    bool HasUnlocked() {
        return inv.ItemUnlocked(equipment.item);
    }

    bool IsUsable() {
        return equipment.IsInteractable();
    }

    public void UpdateDisplay() {

        Usable = IsUsable();
        Unlocked = HasUnlocked();

        UpdateInteractable(Usable);
        SetUnlocked(Unlocked);

        bool showStatNumber = Unlocked && equipment.item.itemType == ItemType.GameplayStashable;
        number.ToggleWindow(showStatNumber);

        if(showStatNumber) { number.SetValue(StatType.Level, inv.GetItemSlot(equipment.item).amount);} 
    }

    void UpdateInteractable(bool toggle) {
        group.alpha = toggle ? 1f : .5f;
    }

    void SetUnlocked(bool toggle) {
        if(toggle) {
            button.Image.sprite = equipment.item.itemSprite ?? button.Image.sprite;            
            button.ToggleState(SPSelectableState.Default);
            hoverText.description = equipment.item.FullDescription();
        } else {
            button.Image.sprite = unknown;
            button.ToggleState(SPSelectableState.Disabled);
            hoverText.description = "";
        }

        unlocked.SetActive(toggle);

        group.alpha = toggle ? group.alpha : .2f;

    }


   
}
