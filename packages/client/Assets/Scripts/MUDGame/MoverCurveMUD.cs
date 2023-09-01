using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveCurve", menuName = "Engine/Animation/MoveCurve", order = 1)]
public class MoverCurveMUD : MoverMUD {
    public override Vector3 Move(Vector3 from, Vector3 to, float t) {
       return Vector3.LerpUnclamped(from, to, lerp.Evaluate(t));
    }
}

