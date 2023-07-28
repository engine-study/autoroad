using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using IWorld.ContractDefinition;
using DefaultNamespace;

public class SpawningUI : SPWindowParent
{
    [SerializeField] private SPButton nameButton, spawnButton;
    bool spawning;
    int x;
    int y;
    bool goodSpawn;

    public GameObject cursor; 
    public GameObject spawnZone; 
    public GameObject ok, bad;

    protected override void OnEnable() {
        base.OnEnable();

        SPCamera.SetFollow(null);
        SPCamera.SetFOVGlobal(20f);
        SPCamera.SetTarget(Vector3.forward * (BoundsComponent.Down + RoadConfigComponent.Height * .5f));

        nameButton.UpdateField(NameComponent.LocalName);

        spawnZone.transform.parent = null;
        spawnZone.transform.position = Vector3.forward * BoundsComponent.Down;
        spawnZone.transform.rotation = Quaternion.identity;
        spawnZone.transform.localScale =  Vector3.one;
        
        cursor.transform.parent = CursorMUD.CursorTransform;
        cursor.transform.localPosition = Vector3.zero;
        cursor.transform.localRotation = Quaternion.identity;
        cursor.transform.localScale = Vector3.one;

        cursor.SetActive(true);
        spawnZone.SetActive(true);

        CursorMUD.OnGridPosition += UpdateCursor;
    }

    protected override void OnDisable() {
        base.OnDisable();
        CursorMUD.OnGridPosition -= UpdateCursor;
        cursor.SetActive(false);
        spawnZone.SetActive(false);
    }

    void Update() {
        if(!spawning && Input.GetMouseButtonDown(0) && goodSpawn) {
            Spawn();
        }
    }

    void UpdateCursor(Vector3 newPos) {

        x = (int)newPos.x;
        y = (int)newPos.z;

        goodSpawn = true; 

        goodSpawn = y <= BoundsComponent.Up && y >= BoundsComponent.Down && (x < BoundsComponent.Left || x > BoundsComponent.Right);
        goodSpawn = goodSpawn && x >= -MapConfigComponent.SpawnWidth && x <= MapConfigComponent.SpawnWidth;
        goodSpawn = goodSpawn && CursorMUD.Entity == null;

        ok.SetActive(goodSpawn);
        bad.SetActive(!goodSpawn);

        spawnButton.ToggleState(goodSpawn ? SPSelectableState.Default : SPSelectableState.Disabled);
    }

    async void Spawn() {
        spawning = true; 
        bool success = await TxManager.Send<SpawnFunction>(System.Convert.ToInt32(x),System.Convert.ToInt32(y));
        spawning = false;

        if(success) {
            ToggleWindowClose();
        }
    }

}
