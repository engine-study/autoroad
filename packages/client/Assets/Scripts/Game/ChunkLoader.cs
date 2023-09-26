using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class ChunkLoader : MonoBehaviour
{
    public static ChunkLoader Instance;
    public static int Mile {get{return Instance.mile;}}
    public static ChunkComponent Chunk {get{return Instance.chunk;}}

    [Header("Chunks")]
    [SerializeField] int mile;
    [SerializeField] ChunkComponent chunk;

    void Awake() {
        Instance = this;
    }

    void OnDestroy() {
        Instance = null;
    }

    public static bool LoadMile(int newMile) {
        return Instance.LoadMileInternal(newMile);
    }

    public bool LoadMileInternal(int newMile) {

        string chunkEntity = MUDHelper.Keccak256("Chunk", (int)newMile);
        ChunkComponent newChunk = MUDWorld.FindComponent<ChunkComponent>(chunkEntity);

        if(newChunk == null) {
            return false; 
        }

        if(chunk) {
            chunk.gameObject.SetActive(false);
        }

        chunk = newChunk;
        newChunk.gameObject.SetActive(true);

        return true;
    }
}
