using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;
using IWorld.ContractDefinition;
public class ValueComponent : MUDComponent {

    public List<InventorySlot> Items {get{return slots;}}
    public float Value {get{return value;}}
    public string String {get{return valueString;}}
    public StatType StatType {get{return statType;}}
    
    [Header("Value")]
    [SerializeField] GaulItem item;
    [SerializeField] protected GameObject visualPrefab;
    // [SerializeField] GaulItem [] items;
    protected Vector3 visualOffset;

    [Header("Debug")]
    [SerializeField] float value;
    [SerializeField] string valueString;
    [SerializeField] StatType statType;
    List<InventorySlot> slots;
    PositionSync pos;

    protected virtual float SetValue(MUDTable update) {return (int)MUDTable.GetRecord(Entity.Key, MUDTableType)?.RawValue["value"];}
    protected virtual StatType SetStat(MUDTable update) {return StatType;}
    protected virtual string SetString(MUDTable update) {return Value.ToString("00");}


    protected override void Init(SpawnInfo newSpawnInfo) {
        base.Init(newSpawnInfo);

        if(slots  == null) {slots = new List<InventorySlot>();}
        slots.Add(new InventorySlot(){item = item});
        
        visualOffset = Vector3.up * 1.25f;
        // for(int i = 0; i < items.Length; i++) {
        //     slots.Add(new InventorySlot(){item = items[i]});
        // }
    }


    protected override void PostInit() {
        base.PostInit();
        if(visualPrefab && visualPrefab.transform.IsChildOf(transform)) {
            visualPrefab.SetActive(false);
        }
    }

    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {

        value = SetValue(update);
        valueString = SetString(update);
        statType = SetStat(update);

        //simple inventory
        slots[0].amount = (int)value;

        if(Loaded || SpawnInfo.Source == SpawnSource.InGame) {
            //do a little spawn animation
            if(item && visualPrefab && Entity.gameObject.activeInHierarchy) {
                pos = Entity.GetRootComponent<PositionSync>();
                if (pos == null) { Debug.LogError("No position sync", this); return; }
                Entity.StartCoroutine(SpawnProp());
            }
        }
        
    }

    

    IEnumerator SpawnProp() {

        // Debug.Log($"{gameObject.name} + Spawning", this);
        
        yield return new WaitForSeconds(.5f);

        string prefab = null;

        if(item.itemType == ItemType.GameplayStashable) {
            prefab = "Prefabs/BuyEffectStash";
        } else if(item.itemType == ItemType.GameplayEquipment) {
            prefab = "Prefabs/BuyEffectEquipment";
        } else {
            yield break;
        }

        SPResourceJuicy propEffect = SPResourceJuicy.SpawnResource(prefab, pos.Target, pos.Target.position, Quaternion.Euler(-45f, Random.Range(0f,360f), 0f));
        propEffect.offset = visualOffset;

        GameObject propForEffect = Instantiate(visualPrefab, propEffect.transform.position, propEffect.transform.rotation, propEffect.transform);
        
        propForEffect.SetActive(true);
        SPHelper.CenterGameObject(propForEffect);

        SPRotate rotate = propEffect.gameObject.AddComponent<SPRotate>();
        rotate.space = Space.World;
        rotate.rotateSpeed = Vector3.up * -180f;

        propEffect.SendResource();
        
    }




}
