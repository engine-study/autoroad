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

    public GameObject cursor; 
    public GameObject ok, bad;

    protected override void OnEnable() {
        base.OnEnable();

        nameButton.UpdateField(NameComponent.LocalName);

        CursorMUD.OnGridPosition += UpdateCursor;
    }

    protected override void OnDisable() {
        base.OnDisable();
        CursorMUD.OnGridPosition -= UpdateCursor;
    }

    void Update() {
        if(Input.GetMouseButton(0)) {
            Spawn();
        }
    }

    void UpdateCursor(Vector3 newPos) {

        cursor.transform.position = newPos;

        x = (int)newPos.x;
        y = (int)newPos.z;

        bool goodSpawn = true; 

        goodSpawn = y < BoundsComponent.Up && y > BoundsComponent.Down;
        goodSpawn = goodSpawn && x >= BoundsComponent.Left && x < RoadConfigComponent.Left && x <= BoundsComponent.Right && x > RoadConfigComponent.Right;
        goodSpawn = goodSpawn && CursorMUD.Entity == null;

        ok.SetActive(goodSpawn);
        bad.SetActive(!goodSpawn);

        spawnButton.ToggleState(goodSpawn ? SPSelectableState.Default : SPSelectableState.Disabled);
        
        if(goodSpawn) {
            Spawn();
        }
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
