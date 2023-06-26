using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RegionGenerator : Generator
{
    [Header("Region")]
    public RegionData regionData;

    [Header("Debug")]
    public List<GameObject> spawnedObjects;
    public override void Generate()
    {
        base.Generate();

    }

    public override void Render()
    {
        base.Render();

        if (spawnedObjects != null)
        {
            for (int i = spawnedObjects.Count - 1; i > -1; i--)
            {
                DestroyImmediate(spawnedObjects[i]);
            }
        }

        spawnedObjects = new List<GameObject>();


        Vector3 basePos = transform.position;

        foreach (RegionSpawner s in regionData.spawners)
        {

            float startRad = Random.Range(0f, Mathf.PI * 2f);

            float index = 0f;
            float rotateLerp = 0f;

            foreach (RegionPatch p in s.spawns)
            {

                float spawnRad = startRad + rotateLerp * Mathf.PI * 2f;
                Vector3 spawnPos = new Vector3(Mathf.Sin(spawnRad), 0f, Mathf.Cos(spawnRad)) * s.radius;

                spawnPos += basePos;
                spawnPos = baseGenerator.WorldToGridLocal(spawnPos);
                // Debug.Log("Spawning: " + spawnPos.ToString());

                SpawnPatch(p, spawnPos);

                index++;
                rotateLerp = index / (float)s.spawns.Length;

            }


        }
    }

    void OnDrawGizmosSelected()
    {
        if (!regionData)
        {
            return;
        }

        Gizmos.DrawWireCube(transform.position, Vector3.up * .01f + (Vector3.right + Vector3.forward) * regionData.size);
    }

    public void SpawnPatch(RegionPatch p, Vector3 position)
    {
        GameObject prefab = p.prefabs[Random.Range(0, p.prefabs.Length)];
        GameObject go = MapGenerator.SpawnObject(prefab, position, Quaternion.identity,transform);
       
        mud.Client.MUDEntity e = go.GetComponent<mud.Client.MUDEntity>();
        
        baseGenerator.AddEntity(MapGenerator.PositionRound(position), e);

        // e.gridPos = position;
        // go.name = prefab.name;
        // spawnedObjects.Add(go);

    }

}
