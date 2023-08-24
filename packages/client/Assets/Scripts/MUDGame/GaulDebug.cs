using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using IWorld.ContractDefinition;
using mud.Client;

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

    void UpdateDebug() {

        actionName.UpdateField("");

        if (Input.GetKey(KeyCode.RightControl) && Input.GetKey(KeyCode.RightAlt)) {
            actionName.UpdateField("Spawn Mile");
            if( Input.GetMouseButtonDown(0)) { TxManager.Send<SpawnMileAdminFunction>();}
            return;
        } else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt)) {
            actionName.UpdateField("Finish Current Mile");
            if (Input.GetMouseButtonDown(0)) { TxManager.Send<FinishMileAdminFunction>(); }
            return;
        } else if (Input.GetKey(KeyCode.LeftShift)) {
            actionName.UpdateField("Teleport");
            if (Input.GetMouseButtonDown(0)) {(PlayerMUD.LocalPlayer.Controller as ControllerMUD).TeleportMUD(CursorMUD.GridPos, true);}
        } else if (Input.GetKey(KeyCode.LeftAlt)) {

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
            }

            actionName.UpdateField("Spawn " + (TerrainType)terrain);

            if (Input.GetMouseButtonDown(0)) { TxManager.Send<SpawnTerrainAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z), System.Convert.ToUInt32(terrain)); }
        }

        // if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0)) {
        //     actionName.UpdateField("Spawn Shoveled Road");
        //     if (Input.GetMouseButtonDown(0)) { TxManager.Send<SpawnShoveledRoadAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z)); }
        // }

        // if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0)) {
        //     actionName.UpdateField("Spawn Shoveled Road");
        //     if (Input.GetMouseButtonDown(0)) { TxManager.Send<SpawnShoveledRoadAdminFunction>(System.Convert.ToInt32(CursorMUD.GridPos.x), System.Convert.ToInt32(CursorMUD.GridPos.z)); }
        // }
    }

    void UpdateCursor() {

    }
}