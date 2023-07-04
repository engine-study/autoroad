using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PositionLoader : MonoBehaviour
{
    void Start() {
        MUDChunkManager.OnEntityRequest += LoadEntityFromPosTable;
    }   

    void OnDestroy() {
        MUDChunkManager.OnEntityRequest -= LoadEntityFromPosTable;
    }

    void LoadEntityFromPosTable(string key) {

    }
}
