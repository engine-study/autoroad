using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoverMUD : ScriptableObject {

    public SPLerpCurve lerp;

    public abstract Vector3 Move(Vector3 from, Vector3 to, float t);

}
