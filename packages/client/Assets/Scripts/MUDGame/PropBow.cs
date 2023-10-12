using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBow : SPAnimationProp
{
    
    [SerializeField] Arrow arrow;

    public override void Fire(string actionName) {

        if(actionName == "Arrow") {
            Arrow newArrow = Instantiate(arrow, transform.position, Quaternion.identity);
            newArrow.pos = animator.transform.position + animator.transform.forward * 10f + Vector3.up;
            newArrow.pos.y = transform.position.y;
        } else {
            base.Fire(actionName);
        }

    }
}
