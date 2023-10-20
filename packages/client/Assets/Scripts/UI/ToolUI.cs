using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToolUI : SPWindowParent
{
    public static ToolUI Instance;
    [Header("Tools")]
    public List<ToolSlotUI> tools;

    [Header("Debug")]
    public ToolSlotUI active;
    public Inventory inventory;
    public ActionsMUD equipment;

    public override void Init() {
        if(HasInit) {return;}
        base.Init();

        Instance = this;

        SPEvents.OnLocalPlayerSpawn += SetupInventory;

    }

    void SetupInventory() {

        equipment = PlayerMUD.MUDPlayer.Equipment;
        inventory = PlayerMUD.MUDPlayer.GetComponent<Inventory>();

        var equipments = equipment.Equipment.Values.ToArray();

        for(int i = 0; i < tools.Count; i++) {

            if(i >= equipments.Length) {tools[i].ToggleWindowClose(); continue;}

            tools[i].Setup(inventory);
            tools[i].input.SetKey(SPInput.GetAlphaKey(i));
            tools[i].ToggleSelected(i == 0);

            tools[i].SetEquipment(equipments[i]);
        }

    }

    protected override void OnDestroy() {
        base.OnDestroy();
        Instance = null;
    }


    void Update() {
        UpdateTools();
    }

    public void AddTool(Equipment e) {

    }

    void UpdateTools() {
        int newTool = SPInput.GetNumber();
        if(newTool == -1) return;
        if(newTool >= tools.Count || tools[newTool] == null) return;

        SetTool(tools[newTool]);
    }


    public void SetTool(ToolSlotUI newTool) {

    }
}

