using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MileColumn : MonoBehaviour
{

    public int Mile {get{return Mathf.RoundToInt(MileRaw);}}
    public float MileRaw {get{return totalRot/round;}}

    [Header("Rotate")]
    [SerializeField] Transform column;
    [SerializeField] float horizontalSpeed = 2.0f;
    [SerializeField] float verticalSpeed = 2.0f;
    [SerializeField] float drag = 1.0f;
    [SerializeField] float round = 180f;

    float scroll, lastScroll;
    float velocity;
    float totalRot;
    float lerpRot;
    float roundedRot;
    Vector3 startPos;
    float axis, startAxis = 0f;
    bool goodInput;
    public void SetMile(float newMile) {
        totalRot = Mathf.Clamp(newMile * round, 0f, (GameStateComponent.MILE_COUNT+1f) * round);
    }

    void Update() {

        lastScroll = scroll;
        axis = Input.GetAxis("Mouse X");

        if(Input.GetMouseButtonDown(0)) {
            goodInput = SPUIBase.IsPointerOverUIElement == false;
        }

        bool mouseInput = Input.GetMouseButton(0) && goodInput;

        if(mouseInput) {
            scroll = Mathf.Lerp(scroll, horizontalSpeed * -(axis - startAxis) * Time.deltaTime, .5f);
        } else {
            startAxis = axis;
            scroll = Mathf.MoveTowards(scroll, 0f, Time.deltaTime * drag);
        }

        // float newVel = (scroll - lastScroll) / Time.deltaTime;
        // velocity = Mathf.Lerp(velocity, Mathf.Clamp01(newVel - drag), .25f);

        totalRot = Mathf.Clamp(totalRot + scroll, 0f, (GameStateComponent.MILE_COUNT+1f) * round);
        lerpRot = Mathf.Lerp(lerpRot, totalRot, .1f);
        roundedRot = Mathf.Round(totalRot / round) * round;

        Quaternion rot = Quaternion.identity;

        if(true || mouseInput) {rot = Quaternion.Euler(Vector3.up * lerpRot);}
        else {rot = Quaternion.Lerp(column.localRotation, Quaternion.Euler(Vector3.up * roundedRot), .1f);}

        column.localRotation = rot;
        column.localPosition = Vector3.down * totalRot * verticalSpeed;

    }

    void LateUpdate() {
        transform.position = SPCamera.I.transform.position;
    }

}
