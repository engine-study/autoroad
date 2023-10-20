using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUI : SPWindowParent
{

    [Header("Tools")]
    public List<ToolSlotUI> tools;

    [Header("Debug")]
    public ToolSlotUI active;

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

