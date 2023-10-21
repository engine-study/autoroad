using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToolUI : SPWindowParent
{
    public static ToolUI Instance;
    public ToolSlotUI Tool {get{return tool;}}
    
    [Header("Tools")]
    public List<ToolSlotUI> tools;
    public AudioClip [] sfx_equip;

    [Header("Debug")]
    public ToolSlotUI tool;
    public Inventory inventory;
    public ActionsMUD equipment;

    public override void Init() {
        if(HasInit) {return;}
        base.Init();

        Instance = this;
        SPEvents.OnLocalPlayerSpawn += SetupInventory;

    }

    void SetupInventory() {
        if(PlayerMUD.MUDPlayer.Equipment.hasInit) SetupEquipment();
        PlayerMUD.MUDPlayer.Equipment.OnInit += SetupEquipment;
    }


    protected override void OnDestroy() {
        base.OnDestroy();
        Instance = null;
        SPEvents.OnLocalPlayerSpawn -= SetupInventory;
    }


    void SetupEquipment() {

        equipment = PlayerMUD.MUDPlayer.Equipment;
        inventory = PlayerMUD.MUDPlayer.GetComponent<Inventory>();

        if(equipment.EquipmentList.Count < 1) {Debug.LogError("No equipment");}

        var equipments = equipment.Equipment.Values.ToArray();

        for(int i = 0; i < tools.Count; i++) {

            if(i >= equipments.Length) {tools[i].ToggleWindowClose(); continue;}

            tools[i].Setup(inventory);
            tools[i].input.SetKey(SPInput.GetAlphaKey(i));
            tools[i].ToggleSelected(false);

            tools[i].SetEquipment(equipments[i]);
        }

    }


    void Update() {
        UpdateTools();
    }

    void UpdateTools() {

        int newTool = SPInput.GetNumber();
        if(newTool == -1) return;
        if(newTool >= tools.Count || tools[newTool] == null) return;

        SetActiveTool(tools[newTool]);
    }

    public void UpdateAllActions() {

        if(!HasInit) return;

        for(int i = 0; i < tools.Count; i++) {
            tools[i].UpdateDisplay();
        }
    }

    public void SetActiveTool(ToolSlotUI newTool) {

        if(tool != null) {tool.ToggleSelected(false);}
        if(newTool.Unlocked == false) {tool = null; return;}

        tool = newTool;
        tool.ToggleSelected(true);


        newTool.equipment.ActionScript.EndAction(PlayerMUD.LocalPlayer.Actor, ActionEndState.Canceled);
        // PlayerMUD.LocalPlayer.Animator.ToggleProp(newTool.equipment.item)

        SPUIBase.PlaySound(sfx_equip);

    }
}

