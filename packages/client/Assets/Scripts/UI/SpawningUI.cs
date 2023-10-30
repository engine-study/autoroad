using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;
using mudworld;
using Cysharp.Threading.Tasks;

public class SpawningUI : SPWindowParent
{
    [Header("Spawning")]
    [SerializeField] private SPButton nameButton;
    [SerializeField] private SPButton spawnButton;
    bool spawning;
    int x;
    int y;
    bool goodSpawn;

    public GameObject scene;
    public GameObject cursor; 
    public GameObject spawnZone; 
    public GameObject ok, bad;

    public AudioClip sfx_spawnChoose;
    public AudioClip sfx_spawn;

    protected override void OnEnable() {
        base.OnEnable();
        CursorMUD.OnGridPosition += UpdateCursor;
    }

    protected override void OnDisable() {
        base.OnDisable();
        CursorMUD.OnGridPosition -= UpdateCursor;
    }

    public override void ToggleWindow(bool toggle) {
        base.ToggleWindow(toggle);

        if(toggle) {

            SPCamera.SetFollow(null);
            SPCamera.SetFOVGlobal(10f);
            SPCamera.SetTarget(Vector3.forward * (BoundsComponent.Down + MapConfigComponent.Height * .5f) + Vector3.left * MapConfigComponent.Width);

            nameButton.UpdateField(NameComponent.LocalName);

            spawnZone.transform.position = Vector3.left * (BoundsComponent.Right+.5f) + Vector3.forward * -.5f; //+ Vector3.forward * (BoundsComponent.Down-.5f)
            spawnZone.transform.localScale = Vector3.up + Vector3.left * (MapConfigComponent.SpawnWidth-MapConfigComponent.Width) + Vector3.forward * MapConfigComponent.Height * (GameStateComponent.MILE_COUNT +1);
            
            cursor.transform.parent = CursorMUD.CursorTransform;
            cursor.transform.localPosition = Vector3.zero;
            cursor.transform.localRotation = Quaternion.identity;
            cursor.transform.localScale = Vector3.one;
        }

        // CameraControls.TogglePan(!toggle);
        
        scene.SetActive(toggle);
        cursor.SetActive(toggle);
        spawnZone.SetActive(toggle);


    }

    void Update() {

        if(!SPUIBase.IsPointerOverUIElement) {
            SPCursorTexture.UpdateCursor(SPCursorState.PointerSlot);
        }

        if(!spawning && Input.GetMouseButtonUp(0) && goodSpawn && !SPUIBase.IsPointerOverUIElement) {
            Spawn();
        }
    }

    void UpdateCursor(Vector3 newPos) {

        x = (int)newPos.x;
        y = (int)newPos.z;

        goodSpawn = true; 

        goodSpawn = y <= BoundsComponent.Up && y >= 0 && x < -MapConfigComponent.Width && x >= -MapConfigComponent.SpawnWidth;
        goodSpawn = goodSpawn && CursorMUD.Entity == null;

        ok.SetActive(goodSpawn);
        bad.SetActive(!goodSpawn);

        spawnButton.ToggleState(goodSpawn ? SPSelectableState.Default : SPSelectableState.Disabled);
    }

    async void Spawn() {

        SPUIBase.PlaySound(sfx_spawnChoose);
        scene.SetActive(false);
        cursor.SetActive(false);
        spawnZone.SetActive(false);

        spawning = true; 
        bool success = await TxManager.SendDirect<SpawnFunction>(System.Convert.ToInt32(x),System.Convert.ToInt32(y));
        spawning = false;

        if(success) {
            ToggleWindowClose();
        } else { 
            scene.SetActive(true);
            cursor.SetActive(true);
            spawnZone.SetActive(true);

        }

        while(SPPlayer.LocalPlayer == null) { await UniTask.Delay(100);}
        
        SPUIBase.PlaySound(sfx_spawn);
        WorldScroll.Instance.SetToPlayerMile();
        
    }

}
