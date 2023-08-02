using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmetic : MonoBehaviour
{
    public System.Action OnUpdated;
    public GameObject defaultBody;
    public Transform bodyParent;

    [Header("Debug")]
    public GameObject activeBody;

    MeshRenderer oldMR, mr;
    SkinnedMeshRenderer oldSMR, smr;
    
    void Awake() {
        SetNewBody(defaultBody);
    }
    public void SetNewBody(GameObject newBody) {
        
        if(activeBody) {
            activeBody.SetActive(false);
            oldMR = mr;
            oldSMR = smr;
        }

        activeBody = newBody;

        if(activeBody == null) {
            return;
        }

        mr = activeBody.GetComponent<MeshRenderer>();
        smr = activeBody.GetComponent<SkinnedMeshRenderer>();

        if(smr && oldSMR) {
            smr.bones = oldSMR.bones;
        }

        activeBody.SetActive(true);
        activeBody.transform.parent = bodyParent;
        activeBody.transform.localPosition = Vector3.zero;
        activeBody.transform.localRotation = Quaternion.identity;

        OnUpdated?.Invoke();
        
    }
}
