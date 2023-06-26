using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mud.Client;

public class InfoUI : WindowEntity
{
    [Header("Info")]
    public SPHeading header;
    public SPHeading playerName, coordinate;
    public SPRawText text;

    public override void UpdateEntity(Entity newEntity)
    {
        base.UpdateEntity(newEntity);

        // Entity mEntity = newEntity as MUDEntity;
        // if(mEntity) {

        //     header.UpdateField(mEntity.stats.objectName);
        //     coordinate.UpdateField("[" + mEntity.gridPos.x + " , " + mEntity.gridPos.z + "]");
        //     text.UpdateField(mEntity.stats.description);

        //     if(mEntity is Ground) {

        //     } else if(mEntity is Structure) {

        //     }
        // }
    }
}
