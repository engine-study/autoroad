using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using IWorld.ContractDefinition;
using mud.Client;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum TerrainType {None, Rock, Mine, Tree, Player, HeavyBoy, HeavyHeavyBoy, Pillar, Ox, Militia, Road, Ditch}
public class GaulDebug : MonoBehaviour
{

    [Header("UI")]
    public GameObject debugParent;
    public SPHeading actionName;

    int terrain = 0;

    void Start() {
        debugParent.SetActive(SPGlobal.IsDebug);
    }

    void Update() {

        if(SPGlobal.IsDebug) {
            UpdateDebug();
        } 

    }

    string debugString;
    void UpdateDebug() {


        debugString = "";

        if (Input.GetKey(KeyCode.RightControl) && Input.GetKey(KeyCode.RightAlt)) {
            debugString += "Spawn Mile";
            if( Input.GetMouseButtonDown(0)) { TxManager.Send<SpawnMileAdminFunction>();}
        } else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt)) {
            debugString += "Finish Current Mile";
            if (Input.GetMouseButtonDown(0)) { TxManager.Send<FinishMileAdminFunction>(); }
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
                terrain = 0;
            } else if (Input.GetKey(KeyCode.Alpha2)) {
                terrain = 1;
            } else if (Input.GetKey(KeyCode.Alpha3)) {
                terrain = 2;
            } else if (Input.GetKey(KeyCode.Alpha4)) {
                terrain = 3;
            } else if (Input.GetKey(KeyCode.Alpha5)) {
                terrain = 4;
            } else if (Input.GetKey(KeyCode.Alpha6)) {
                terrain = 5;
            } else if (Input.GetKey(KeyCode.Alpha7)) {
                terrain = 6;
            } else if (Input.GetKey(KeyCode.Alpha8)) {
                terrain = 7;
            } else if (Input.GetKey(KeyCode.Alpha9)) {
                terrain = 8;
            } else if (Input.GetKey(KeyCode.Alpha0)) {
                terrain = 9;
            } else if (Input.GetKey(KeyCode.F1)) {
                terrain = 10;
            } else if (Input.GetKey(KeyCode.F2)) {
                terrain = 11;
            } else {
                terrain = -1;
            }

            if(terrain == -1) {
                debugString += "LMB: Spawn [1-9] \nRMB: Delete";
            } else {
                debugString += "Spawn " + (TerrainType)terrain;
            }

            if (Input.GetMouseButtonDown(0) && terrain > -1) { TxManager.Send<SpawnTerrainAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z), System.Convert.ToByte((TerrainType)terrain)); }
            else if (Input.GetMouseButtonDown(1)) { TxManager.Send<DeleteAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z), System.Convert.ToInt32(0)); }
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

}
