using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionData : MonoBehaviour
{
    [Header("Actions")]
    [EnumNamedArray( typeof(ActionName) )]
    public ActionEffect [] effects;
}
