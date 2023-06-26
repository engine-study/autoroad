using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    [Header("Placement")]
    public Entity place;
    public GameObject valid, invalid;
    public bool canPlace;

    void Awake() {
        CursorUI.CursorUpdate += UpdateCursor;
    }

    void OnDestroy() {
        CursorUI.CursorUpdate -= UpdateCursor;
    }

    public void UpdateCursor() {
        canPlace = ValidSpot();
        
        valid.SetActive(canPlace);
        invalid.SetActive(!canPlace);

        transform.position = SPCursor.GridPos;

    }
    public bool ValidSpot() {

        Ground groundAt = CursorUI.CursorGround;
        if(groundAt == null || groundAt.material == GroundMaterial.Dust) {
            // Debug.Log("No ground");
            return false;
        }

        Entity entity = CursorUI.CursorEntity;
        if(entity != null) {
            // Debug.Log("Entity");
            return false;
        }

        return true;
    }

    void Update() {
        UpdatePlacement();
        UpdateInput();
    }

    void UpdatePlacement() {


        if(place is Structure) {

        } else if (place is Ground) {

        } else if(place is Unit) {

        }
    }

    void UpdateInput() {
        if(Input.GetMouseButtonDown(0) && canPlace) {
            // BuildingManager.Instance.Build(new Vector2(SPCursor.GridPos.x, SPCursor.GridPos.z));
        }
    }
}
