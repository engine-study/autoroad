using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using IWorld.ContractDefinition;
using mud.Client;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

public enum TerrainType {None, Rock, Mine, Tree, HeavyBoy, HeavyHeavyBoy, Pillar, Road, Hole}
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

        SPGlobal.OnDebug += ToggleDebug;

        on = !SPGlobal.IsDebug;
        ToggleDebug(SPGlobal.IsDebug);
    }
    void Destroy() {
        ToggleDebug(false);
    }
    
    void ToggleDebug(bool toggle) {
        
        if(toggle == on) return;

        on = toggle;
        debugParent.SetActive(toggle);   

        if(toggle) {
            CursorMUD.OnGridPosition += UpdateDebug;
        } else {
            CursorMUD.OnGridPosition += UpdateDebug;
        }
                
    }
    
    void Update() {
          debugString = "";

        if (Input.GetKey(KeyCode.RightControl) && Input.GetKey(KeyCode.RightAlt)) {
            debugString += "Spawn Mile";
            if( Input.GetMouseButtonDown(0)) { TxManager.SendQueue<SpawnMileAdminFunction>();}
        } else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt)) {
            debugString += "Finish Current Mile";
            if (Input.GetMouseButtonDown(0)) { TxManager.SendQueue<FinishMileAdminFunction>(); }
        } else if (Input.GetKey(KeyCode.LeftAlt)) {
            debugString += "LMB: Select \nRMB: Teleport";

            if (Input.GetMouseButtonDown(0)) {
                #if UNITY_EDITOR
                Selection.activeGameObject = CursorMUD.Entity?.gameObject;
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

        // if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0)) {
        //     actionName.UpdateField("Spawn Shoveled Road");
        //     if (Input.GetMouseButtonDown(0)) { TxManager.Send<SpawnShoveledRoadAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z)); }
        // }

        // if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0)) {
        //     actionName.UpdateField("Spawn Shoveled Road");
        //     if (Input.GetMouseButtonDown(0)) { TxManager.Send<SpawnShoveledRoadAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z)); }
        // }
    }


    void UpdateDebug(Vector3 newPos) {

      
    }

}
