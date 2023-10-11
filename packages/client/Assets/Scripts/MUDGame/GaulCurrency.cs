using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Value", menuName = "Gaul/Value", order = 1)]
public class GaulValue : ScriptableObject {
    [SerializeField] public Currency Value;
}