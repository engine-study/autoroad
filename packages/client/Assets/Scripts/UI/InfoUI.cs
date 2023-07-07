using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mud.Client;

public class InfoUI : SPWindow
{
    [Header("Info")]
    public SPHeading header;
    public SPHeading playerName, coordinate;
    public SPRawText text;

    public void UpdateInfo(SPBase newBase, int x, int y)
    {

        SPBaseActor actor = newBase as SPBaseActor;
        if(actor) {

            header.UpdateField(actor.gameObject.name);
            coordinate.UpdateField("[" + x + " , " + y + "]");
            // text.UpdateField(actor.stats.description);
        }
    }
}
