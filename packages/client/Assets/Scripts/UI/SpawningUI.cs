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
    public GameObject ok, bad;

    protected override void OnEnable() {
        base.OnEnable();

        nameButton.UpdateField(NameComponent.LocalName);

        cursor.transform.parent = CursorMUD.CursorTransform;

        cursor.transform.localPosition = Vector3.zero;
        cursor.transform.localRotation = Quaternion.identity;
        cursor.transform.localScale = Vector3.one;

        cursor.SetActive(true);

        CursorMUD.OnGridPosition += UpdateCursor;
    }

    protected override void OnDisable() {
        base.OnDisable();
        CursorMUD.OnGridPosition -= UpdateCursor;
        cursor.SetActive(false);
    }

    void Update() {
        if(Input.GetMouseButton(0) && goodSpawn) {
            Spawn();
        }
    }

    void UpdateCursor(Vector3 newPos) {

        x = (int)newPos.x;
        y = (int)newPos.z;

        goodSpawn = true; 

        goodSpawn = y < BoundsComponent.Up && y > BoundsComponent.Down && (x < BoundsComponent.Left || x > BoundsComponent.Right);
        goodSpawn = x >= -MapConfigComponent.SpawnWidth && x <= MapConfigComponent.SpawnWidth;
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
