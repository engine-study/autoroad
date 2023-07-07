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

        if(newObject == null) {
            ToggleWindowClose();
            return;
        }

        SPBase actor = newObject as SPBase;
        if(actor) {

            nameText.text = actor.gameObject.name;
            // coordinate.UpdateField("[" + x + " , " + y + "]");
            // text.UpdateField(actor.stats.description);
        }

        
    }
}
