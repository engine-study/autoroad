using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mud;

public class ActorUI : SPWindowEntity {

    [Header("Stats")]
    public LevelUI level;
    public MoveTypeUI move;
    public TextMeshProUGUI nameText;
    public SPRawText nameRawText;
    public StatUI healthStat;
    public StatUI attackStat;

    public override void UpdateEntity() {
        base.UpdateEntity();

        move.SetEntity(entity);
        level.SetEntity(entity);

        if (entity) {
            nameRawText.UpdateField(entity.Name);
            nameText.text = entity.Name;
            PositionComponent pos = entity.GetMUDComponent<PositionComponent>();
            position.SetFollow(pos?.transform);   
        } else {
            nameText.text = "";
            position.SetFollow(null);   
        }

        gameObject.SetActive(entity != null);

        if(gameObject.activeInHierarchy)
            LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
            
        // Canvas.ForceUpdateCanvases();
    }

}
