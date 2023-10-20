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
        

    }

    void UpdateInput() {
        int newTool = SPInput.GetNumber();
        if(newTool == -1) {return;}
    

    }

    public void AddTool(Equipment e) {
        
    }

    public void SetTool(ToolSlotUI newTool) {

    }
}

