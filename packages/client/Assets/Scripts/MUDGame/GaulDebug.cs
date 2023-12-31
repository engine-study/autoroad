using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mudworld;
using IWorld.ContractDefinition;
using mud;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

public enum TerrainType {None, Rock, Trap, Tree, HeavyBoy, HeavyHeavyBoy, Pillar, Road, Hole}

public class GaulDebug : MonoBehaviour
{

    [Header("UI")]
    public GameObject debugParent;
    public SPHeading actionName;

    int terrain = 0;
    int npc = 0;

    bool on = false;
    string debugString;

    void Start() {

        if(Application.isPlaying) {
            SPGlobal.OnDebug += ToggleDebug;

            on = !SPGlobal.IsDebug;
            ToggleDebug(SPGlobal.IsDebug);
        }

    }
    void Destroy() {
        ToggleDebug(false);
    }
    
    void ToggleDebug(bool toggle) {
        
        if(toggle == on) return;

        enabled = toggle;

        on = toggle;
        debugParent.SetActive(toggle);   

        if(toggle) {
            CursorMUD.OnGridPosition += UpdateDebug;
        } else {
            CursorMUD.OnGridPosition -= UpdateDebug;
        }
                
    }
    
    void Update() {

        UpdatePlayMode();

    }

    void UpdateEditMode() {

        int mileShortcut = SPInput.GetNumber();
        if(mileShortcut != -1) {
            SPCamera camera = FindObjectOfType<SPCamera>();
            camera.transform.position = Vector3.forward * (mileShortcut * 10f + 5f);
        }

        if(Input.GetKeyDown(KeyCode.Minus)) {
            SPCamera camera = FindObjectOfType<SPCamera>();
            camera.SetFOV(camera.FOV - 5f);
        }

        if(Input.GetKeyDown(KeyCode.Equals)) {
            SPCamera camera = FindObjectOfType<SPCamera>();
            camera.SetFOV(camera.FOV + 5f);    
        }

    }

    void UpdatePlayMode() {
        
        debugString = "";

        if (Input.GetKey(KeyCode.RightControl) && Input.GetKey(KeyCode.RightAlt)) {
            debugString += "Spawn Shoveled Road";
            if (Input.GetMouseButtonDown(0)) { TxManager.SendDirect<SpawnShoveledRoadAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z)); }

        } else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt)) {
            debugString += "Spawn Finished Road";
        if (Input.GetMouseButtonDown(0)) { TxManager.SendDirect<SpawnFinishedRoadAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z)); }
        } else if (Input.GetKey(KeyCode.LeftAlt)) {
            debugString += "LMB: Select \nRMB: Teleport";

            if (Input.GetMouseButtonDown(0)) {
                #if UNITY_EDITOR
                if(CursorMUD.Entity) Selection.activeGameObject = CursorMUD.Entity?.gameObject;
                else Selection.activeGameObject = CursorMUD.EntityUnder?.gameObject;
                #endif
            }
            if (Input.GetMouseButtonDown(1)) {(PlayerMUD.LocalPlayer.Controller as ControllerMUD).TeleportMUD(CursorMUD.GridPos, true);}
            
        } else if (Input.GetKey(KeyCode.LeftShift)) {

            if(Input.GetKey(KeyCode.Alpha1)) {
                npc = 0;
                terrain = 0;
            } else if (Input.GetKey(KeyCode.Alpha2)) {
                npc = 1;
                terrain = 1;
            } else if (Input.GetKey(KeyCode.Alpha3)) {
                npc = 2;
                terrain = 2;
            } else if (Input.GetKey(KeyCode.Alpha4)) {
                npc = 3;
                terrain = 3;
            } else if (Input.GetKey(KeyCode.Alpha5)) {
                npc = 4;
                terrain = 4;
            } else if (Input.GetKey(KeyCode.Alpha6)) {
                npc = 5;
                terrain = 5;
            } else if (Input.GetKey(KeyCode.Alpha7)) {
                npc = 6;
                terrain = 6;
            } else if (Input.GetKey(KeyCode.Alpha8)) {
                npc = 7;
                terrain = 7;
            } else if (Input.GetKey(KeyCode.Alpha9)) {
                npc = 8;
                terrain = 8;
            } else if (Input.GetKey(KeyCode.Alpha0)) {
                npc = 9;
                terrain = 9;
            } else if (Input.GetKey(KeyCode.F1)) {
                npc = 10;
                terrain = 10;
            } else if (Input.GetKey(KeyCode.F2)) {
                npc = 11;
                terrain = 11;
            } else {
                npc = -1;
                terrain = -1;
            }

            if(terrain == -1) {
                debugString += "LMB: Spawn [1-9] \nRMB: Delete";
            } else {
                if(Input.GetKey(KeyCode.Tab)) debugString += "Spawn " + (NPCType)npc;
                else debugString += "Spawn " + (TerrainType)terrain;
               
            }

            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Tab) && npc > -1) { TxManager.SendQueue<SpawnNPCAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z), System.Convert.ToByte((NPCType)npc)); }
            else if (Input.GetMouseButtonDown(0) && terrain > -1) { TxManager.SendQueue<SpawnTerrainAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z), System.Convert.ToByte((TerrainType)terrain)); }
            else if (Input.GetMouseButtonDown(1)) { TxManager.SendQueue<DeleteAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z), System.Convert.ToInt32(0)); }
        }


        if(CursorMUD.Entity is MUDEntity) {
            MUDEntity m = (MUDEntity)CursorMUD.Entity;
            PlayerComponent player = m.GetMUDComponent<PlayerComponent>();
            // if(player) {debugString += "\n" + playerCom}

            ActionComponent state = m.GetMUDComponent<ActionComponent>();
            if (state) { debugString += "\nAction: " + state.Action; }
            // AnimationComponent anim = m.GetMUDComponent<AnimationComponent>();
            // if (anim) { debugString += "\nAnim: " + anim.Anim; }
        }

        actionName.UpdateField(debugString);

    }


    void UpdateDebug(Vector3 newPos) {

      
    }

    void GoMile(int i) {
        SPCamera camera = FindObjectOfType<SPCamera>();
        camera.transform.position = Vector3.forward * (i * 10f + 5f);
    }
    
    void Zoom(bool zoomIn) {
        SPCamera camera = FindObjectOfType<SPCamera>();
        camera.SetFOV(zoomIn ? camera.FOV - 2.5f : camera.FOV + 2.5f, true);
    }
    
    #if UNITY_EDITOR

    [MenuItem("Engine/ZoomIn _PGUP")]
    static void ZoomIn() {
        if(Application.isPlaying) { return; }
        var gd = FindObjectOfType<GaulDebug>();
        gd.Zoom(true);
    }

    [MenuItem("Engine/ZoomOut _PGDN")]
    static void ZoomOut() {
        if(Application.isPlaying) { return; }
        var gd = FindObjectOfType<GaulDebug>();
        gd.Zoom(false);
    }


    [MenuItem("Engine/Mile1 &1")]
    static void Mile0() {
        if(Application.isPlaying) { return; }
        var gd = FindObjectOfType<GaulDebug>();
        gd.GoMile(0);
    }
    [MenuItem("Engine/Mile2 &2")]
    static void Mile1() {
        if(Application.isPlaying) { return; }
        var gd = FindObjectOfType<GaulDebug>();
        gd.GoMile(1);
    }
    [MenuItem("Engine/Mile3 &3")]
    static void Mile2() {
        if(Application.isPlaying) { return; }
        var gd = FindObjectOfType<GaulDebug>();
        gd.GoMile(2);
    }
    [MenuItem("Engine/Mile4 &4")]
    static void Mile3() {
        if(Application.isPlaying) { return; }
        var gd = FindObjectOfType<GaulDebug>();
        gd.GoMile(3);
    }
    [MenuItem("Engine/Mile5 &5")]
    static void Mile4() {
        if(Application.isPlaying) { return; }
        var gd = FindObjectOfType<GaulDebug>();
        gd.GoMile(4);
    }
    [MenuItem("Engine/Mile6 &6")]
    static void Mile5() {
        if(Application.isPlaying) { return; }
        var gd = FindObjectOfType<GaulDebug>();
        gd.GoMile(5);
    }



    #endif

}
