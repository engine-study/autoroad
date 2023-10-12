using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public AudioClip [] shootFX;
    public AudioClip [] hitFX;

    public Transform target;
    public Vector3 pos;
    public Vector3 Target {get{return pos;}}

    void Start() {
        transform.rotation = Quaternion.LookRotation(Target - transform.position);
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, Target, Time.deltaTime * 25f);
        if(transform.position == Target) {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}
