using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mud.Client;

public class ActorUI : SPWindowBase
{
    [Header("Stats")]
    public StatUI healthStat;
    public StatUI attackStat;
    public TextMeshProUGUI nameText;

    public override void UpdateObject(SPBase newObject)
    {
        base.UpdateObject(newObject);

        // MUDEntity mEntity = newEntity as MUDEntity;
        // if(mEntity) {
        //     healthStat.SetValue(mEntity.stats.health.ToString());
        //     attackStat.SetValue(mEntity.stats.attack.ToString());
        //     nameText.text = mEntity.stats.objectName;

        //     if(mEntity is Structure) {
        //         Structure structure = mEntity as Structure;

        //     } else if(mEntity is Resource) {
        //         Resource resource = mEntity as Resource;

        //     } else if (mEntity is Unit) {
        //         Unit unit = mEntity as Unit;

        //     } else {
        //         ToggleWindowClose();
        //     }
        // } else {
        //     ToggleWindowClose();
        // }

        
    }
}
