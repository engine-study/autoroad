using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MileColumn : MonoBehaviour
{


    [Header("Rotate")]
    [SerializeField] Transform column;
    [SerializeField] float horizontalSpeed = 2.0f;
    [SerializeField] float verticalSpeed = 2.0f;
    [SerializeField] float drag = 1.0f;

    float scroll, lastScroll;
    float velocity;
    float lifeTimeRot;
    Vector3 startPos;

    void Update() {

        lastScroll = scroll;

        if(Input.GetMouseButton(0)) {
            scroll = Mathf.Lerp(scroll, horizontalSpeed * -Input.GetAxis("Mouse X") * Time.deltaTime, .5f);
        } else {
            scroll = Mathf.MoveTowards(scroll, 0f, Time.deltaTime * drag);
        }

        // float newVel = (scroll - lastScroll) / Time.deltaTime;
        // velocity = Mathf.Lerp(velocity, Mathf.Clamp01(newVel - drag), .25f);

        lifeTimeRot = Mathf.Clamp(lifeTimeRot + scroll, 0f, 1080f);

        column.localRotation = Quaternion.Euler(0, lifeTimeRot, 0f);
        column.localPosition = Vector3.down * lifeTimeRot * verticalSpeed;

    }

    void LateUpdate() {
        transform.position = SPCamera.I.transform.position;
    }

}
