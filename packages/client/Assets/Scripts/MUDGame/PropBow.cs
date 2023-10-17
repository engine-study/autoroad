using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBow : SPAnimationProp
{
    
    [SerializeField] Arrow arrow;
    [SerializeField] AnimationMUD mud;

    public override void SetAnimator(SPAnimator newAnimator) {
        base.SetAnimator(newAnimator);
        mud = Animator.GetComponentInParent<AnimationMUD>(true);

    }

    public override void Fire(string actionName) {

        if(actionName == "Arrow") {

            Arrow newArrow = Instantiate(arrow, transform.position, Quaternion.identity);

            if(mud && mud.ActionData.Target) {

                newArrow.inaccuracy = Random.onUnitSphere * .05f;

                SPAnimator targetAnimator = mud.ActionData.Target.Pos.Entity.GetRootComponent<AnimationMUD>()?.Animator;
    
                if(targetAnimator) { newArrow.target = targetAnimator.Head; } 
                else { newArrow.target = mud.ActionData.Target.Target; }

            } else {
                newArrow.pos = Animator.transform.position + Animator.transform.forward * 10f + Vector3.up;
                newArrow.pos.y = transform.position.y;
            }


        } else {
            base.Fire(actionName);
        }

    }
}
