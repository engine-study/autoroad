using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MoveTeleport", menuName = "Engine/Animation/MoveTeleport", order = 1)]
public class MoverTeleportMUD : MoverMUD {
    public override Vector3 Move(Vector3 from, Vector3 to, float t) {
        if(t == 0f) {
            return from;
        } else {
            return to;
        }
    }
}
