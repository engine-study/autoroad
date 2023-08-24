using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mud.Client;

public class ActorUI : SPWindowBase {
    [Header("Stats")]
    public MUDEntity entity;
    public StatUI healthStat;
    public StatUI attackStat;
    public SPRawText nameRawText;
    public TextMeshProUGUI nameText;
    public MoveTypeUI move;

    public void UpdateInfo(Entity newEntity) {

        if (entity != newEntity) {
            if (entity != null)
                entity.OnUpdated -= Refresh;

            MUDEntity m = newEntity as MUDEntity;
            if (m != null) {
                m.OnUpdated += Refresh;
            }
        }

        entity = (MUDEntity)newEntity;
        move.UpdateInfo(entity);

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

    void Refresh() {
        UpdateInfo(entity);
    }

}
