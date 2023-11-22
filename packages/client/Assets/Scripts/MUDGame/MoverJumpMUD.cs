using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveJump", menuName = "Engine/Animation/MoveJump", order = 1)]
public class MoverJumpMUD : MoverMUD {
    public float height = 1.4f;
    public override Vector3 Move(Vector3 from, Vector3 to, float t) {
        Vector3 vertical = Vector3.up * height * Mathf.Sin(Mathf.PI * t);
        if(t == 1) {SPCamera.AddShake(.1f,to);}
        return Vector3.LerpUnclamped(from + vertical, to + vertical, lerp.Evaluate(t));
    }

}
