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
    public TextMeshProUGUI nameText;
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

        if (entity == null) {
            nameText.text = "";
            position.SetFollow(null);   
        } else {
            nameText.text = entity.Name;
            position.SetFollow(entity.transform);   
        }

        gameObject.SetActive(entity != null);
    }

    void Refresh() {
        UpdateInfo(entity);
    }

}
