using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TerrainSimple : MonoBehaviour
{
    TerrainCollider tc;
    void Awake() {
        tc = GetComponent<TerrainCollider>();
        tc.enabled = false;
    }
}
