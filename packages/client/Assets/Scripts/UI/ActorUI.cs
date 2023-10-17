using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mud;

public class ActorUI : EntityUI {

    [Header("Stats")]
    public StatsList stats;
    public LevelUI level;
    public TextMeshProUGUI nameText;
    public SPRawText nameRawText;

    protected override void UpdateEntity() {
        base.UpdateEntity();

        // move.SetEntity(Entity);
        level.SetEntity(Entity);
        stats.SetEntity(Entity);

        if (Entity) {
            nameRawText.UpdateField(Entity.Name);
            nameText.text = Entity.Name;
            PositionComponent pos = Entity.GetMUDComponent<PositionComponent>();
            position.SetFollow(pos?.transform);   
        } else {
            nameText.text = "";
            position.SetFollow(null);   
        }

        gameObject.SetActive(Entity != null);

        if(gameObject.activeInHierarchy)
            LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
            
        // Canvas.ForceUpdateCanvases();
    }

}
