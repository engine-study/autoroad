using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public Vector3 Target {get{return target ? target.position : pos;}}

    [Header("Projectile")]
    public SPEnableDisable effects;
    public AudioClip [] shootFX;
    public AudioClip [] hitFX;

    [Header("Debug")]
    public Transform target;
    public SPAnimator targetAnimator;
    public Vector3 pos;
    public Vector3 inaccuracy = Vector3.zero;
    public Rigidbody rb;

    void Awake() {
        if(rb == null) rb = GetComponentInChildren<Rigidbody>();
    }

    void Start() {
        transform.rotation = Quaternion.LookRotation(Target - transform.position);
        effects?.Spawn(true);
    }

    void Update() {

        transform.position = Vector3.MoveTowards(transform.position, Target + inaccuracy, Time.deltaTime * 25f);
        if(!target && transform.position == Target) { End();}

    }

    void End() {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {

        if(target == null) return;
        if(other.transform.IsChildOf(target) == false) {return;}

        SPCamera.AddShake(.05f,transform.position);
        if(rb) Destroy(rb);


        transform.parent = other.transform;
        if(targetAnimator) { targetAnimator.SetToAnimatorLayer(gameObject);}
        
        effects?.Spawn(false);

        target = null;
        enabled = false;

    }


}
