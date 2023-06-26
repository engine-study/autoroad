using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : Generator
{

    public enum DrawMode { None, NoiseMap, ColourMap, Falloff, Blend, Blocks, Biome };
    public static MapGenerator Instance;
    public static Transform BlockParent { get { return Instance.blockParent; } }


    public static Vector3 CursorToGrid(Vector3 cursor)
    {
        int x = (int)Mathf.Round(cursor.x);
        int z = (int)Mathf.Round(cursor.z);
        Vector2 key = new Vector2(x, z);
        if (Instance.mapSave.noiseMap.ContainsKey(key))
        {
            return new Vector3(x, Instance.mapSave.noiseMap[key], z);
        }
        else
        {
            return new Vector3(x, Mathf.Round(cursor.y), z);
        }
    }

    public static Vector3 WorldToGrid(Vector3 world)
    {
        return Instance.WorldToGridLocal(world);
    }

    public Vector3 WorldToGridLocal(Vector3 world)
    {
        int x = (int)Mathf.Round(world.x - .5f);
        int z = (int)Mathf.Round(world.z - .5f);
        Vector2 key = new Vector2(x, z);

        if (mapSave.noiseMap.ContainsKey(key))
        {
            return new Vector3(x, mapSave.noiseMap[key], z);
        }
        else
        {
            return new Vector3(x, Mathf.Round(world.y), z);
        }
    }

    public static Vector2 PositionRound(Vector3 position)
    {
        Vector2 result = new Vector2(Mathf.Round(position.x), Mathf.Round(position.z));
        return result;
    }
    public static Entity GetEntityAtPosition(Vector3 position)
    {
        return GetEntityAtPosition(PositionRound(position));
    }
    public static Entity GetEntityAtPosition(Vector2 key)
    {
        // Debug.Log(Instance.gameObject.name);

        if (Instance.mapSave.entities.ContainsKey(key))
        {
            return Instance.mapSave.entities[key];
        }
        else
        {
            return null;
        }

    }


    public static Ground GetTerrainAtPosition(Vector3 position)
    {
        return GetTerrainAtPosition(PositionRound(position));
    }
    public static Ground GetTerrainAtPosition(Vector2 key)
    {
        // Debug.Log(Instance.gameObject.name);

        if (Instance.mapSave.blocks.ContainsKey(key))
        {
            return Instance.mapSave.blocks[key];
        }
        else
        {
            return null;
        }

    }

    [Header("Generator")]
    public bool mainMap;
    public DrawMode drawMode;
    public MapData mapData;
    public MapData biomeData;
    public MapBiomes mapRegions;
    public MapSave mapSave;
    public bool fog = false;
    public bool autoUpdate;
    public bool randomSeed;


    [Header("Debug")]
    public Transform blockParent;
    public MapDisplay display;
    public Generator[] maps;
    public Generator[] regions;

    void Awake()
    {
        if (mainMap)
            Create();
    }

    void OnDestroy()
    {
        Instance = null;
    }

    public override void Generate()
    {
        base.Generate();

        if (mainMap)
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Changing map");
            }

            Instance = this;
        }

        Debug.Log("Generate", gameObject);

        mapSave = new MapSave();
        mapSave.blocks = new Dictionary<Vector2, Ground>();
        mapSave.entities = new Dictionary<Vector2, Entity>();

        if (randomSeed)
        {
            mapData.seed = Random.Range(0, 100000000);
        }

        if (mapData.genType == MapData.MapGen.Noise)
        {
            mapSave.noiseMap = Noise.GenerateNoiseMap(mapData);
            mapSave.biomeMap = Noise.GenerateNoiseMap(biomeData);
        }
        else
        {
            mapSave.noiseMap = Noise.GenerateFillMap(mapData);
            mapSave.biomeMap = Noise.GenerateFillMap(mapData);
        }

        mapSave.blendMap = new Dictionary<Vector2, float>();

        FalloffGenerator falloffGenerator = GetComponent<FalloffGenerator>();
        falloffGenerator = null;

        if (falloffGenerator)
        {
            mapSave.falloffMap = falloffGenerator.GenerateFalloffMap(transform.position, mapData.mapWidth, mapData.mapHeight);
        }

        for (int y = (int)(mapData.mapHeight * -.5f); y < mapData.mapHeight * .5f; y++)
        {
            for (int x = (int)(mapData.mapWidth * -.5f); x < mapData.mapWidth * .5f; x++)
            {
                Vector2 v2 = new Vector2(x, y);
                float currentHeight = mapSave.noiseMap[v2];
                float currentBiome = mapSave.biomeMap[v2];
                if (falloffGenerator)
                {
                    currentHeight = Mathf.Clamp01(currentHeight - (1f - mapSave.falloffMap[v2]) * mapData.falloffStrength);
                    currentBiome = Mathf.Clamp01(currentBiome - (1f - mapSave.falloffMap[v2]) * mapData.falloffStrength);
                }
                mapSave.noiseMap[v2] = currentHeight * 2f;
                mapSave.biomeMap[v2] = currentBiome;
            }
        }

        maps = GetComponentsInChildren<MapGenerator>();
        foreach (MapGenerator g in maps)
        {

            if (g == this)
                continue;

            g.baseGenerator = mainMap ? this : baseGenerator;
            g.Generate();
            Merge(this, g, new Vector2(g.transform.position.x, g.transform.position.z));
        }

        regions = GetComponentsInChildren<RegionGenerator>();
        foreach (RegionGenerator g in regions)
        {

            if (g == this)
                continue;

            g.baseGenerator = mainMap ? this : baseGenerator;
            g.Generate();
        }

    }

    public override void Render()
    {
        base.Render();

        maps = GetComponentsInChildren<MapGenerator>();
        foreach (MapGenerator g in maps)
        {

            if (g == this)
                continue;

            g.Render();
        }

        regions = GetComponentsInChildren<RegionGenerator>();
        foreach (RegionGenerator g in regions)
        {

            if (g == this)
                continue;

            g.Render();
        }

        GameObject[] gos = Instance.gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < gos.Length; i++)
        {
            if (gos[i].name == "Blocks")
            {
                blockParent = gos[i].transform;
                break;
            }
        }

        if (blockParent != null)
        {
            DestroyImmediate(blockParent.gameObject);
        }

        if (blockParent == null)
        {
            blockParent = new GameObject("Blocks").transform;

            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(blockParent.gameObject, Instance.gameObject.scene);
            blockParent.localPosition = Vector3.zero;
            blockParent.localRotation = Quaternion.identity;
            blockParent.transform.parent = null;
        }

        if (mainMap)
        {
            Spawn();
        }


        if (display == null)
        {
            display = Instantiate(Resources.Load("MapData/MapDisplay") as GameObject, transform.position, transform.rotation, transform).GetComponent<MapDisplay>();
            display.name = "MapDisplay";
        }

        blockParent.gameObject.SetActive(drawMode == DrawMode.Blocks);
        display.gameObject.SetActive(drawMode != DrawMode.Blocks && drawMode != DrawMode.None);

        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapSave.noiseMap, mapData.mapWidth, mapData.mapHeight));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapSave.colourMap, mapData.mapWidth, mapData.mapHeight));
        }
        else if (drawMode == DrawMode.Falloff)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapSave.falloffMap, mapData.mapWidth, mapData.mapHeight));
        }
        else if (drawMode == DrawMode.Blend)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapSave.blendMap, mapData.mapWidth, mapData.mapHeight));
        }
        else if (drawMode == DrawMode.Biome)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapSave.biomeMap, mapData.mapWidth, mapData.mapHeight));
        }
        else if (drawMode == DrawMode.Blocks)
        {

        }

    }

    void Spawn()
    {

        mapSave.colourMap = new Color[mapData.mapWidth + mapData.mapHeight];

        for (int y = (int)(mapData.mapHeight * -.5f); y < mapData.mapHeight * .5f; y++)
        {
            for (int x = (int)(mapData.mapWidth * -.5f); x < mapData.mapWidth * .5f; x++)
            {

                float currentHeight = mapSave.noiseMap[new Vector2(x, y)];
                float currentBiome = mapSave.biomeMap[new Vector2(x, y)];

                // height biomes
                // for (int i = 0; i < mapRegions.regions.Length; i++) {
                // 	if (currentHeight <= mapRegions.regions [i].height) {
                // 		// colourMap [y * mapData.mapWidth + x] = mapRegions.regions [i].colour;

                // 		Vector3 position = new Vector3(x - mapData.mapWidth * .5f, mapRegions.regions [i].height, y - mapData.mapHeight * .5f);
                // 		// Vector3 position = new Vector3(x - mapData.mapWidth * .5f, mapRegions.regions [i].height * .75f + currentHeight * .5f, y - mapData.mapHeight * .5f);
                // 		// Vector3 position = new Vector3(.5f + x - mapData.mapWidth * .5f, currentHeight, .5f + y - mapData.mapHeight * .5f);
                // 		GameObject block = Instantiate(blockPrefab, position, Quaternion.identity, blockParent);


                // 		break;
                // 	}
                // }


                //noise biomes
                for (int i = 0; i < mapRegions.regions.Length; i++)
                {
                    if (currentBiome <= mapRegions.regions[i].height)
                    {
                        // colourMap [y * mapData.mapWidth + x] = mapRegions.regions [i].colour;

                        // Vector3 position = GridToWorld(new Vector3(x, currentHeight, y));
                        Vector3 position = new Vector3(x, currentHeight, y);

                        // Vector3 position = new Vector3(.5f + x - mapData.mapWidth * .5f, currentHeight, .5f + y - mapData.mapHeight * .5f);
                        // Vector3 position = new Vector3(x - mapData.mapWidth * .5f, mapRegions.regions [i].height * .75f + currentBiome * .5f, y - mapData.mapHeight * .5f);
                        // Vector3 position = new Vector3(.5f + x - mapData.mapWidth * .5f, currentBiome, .5f + y - mapData.mapHeight * .5f);
                        Ground newGround = SpawnObject(mapRegions.regions[i].block.gameObject, position, Quaternion.identity, blockParent).GetComponent<Ground>();
                        if (Application.isPlaying)
                        {
                            newGround = Instantiate(mapRegions.regions[i].block, position, Quaternion.identity, blockParent).GetComponent<Ground>();
                        }
                        else
                        {
#if UNITY_EDITOR
                            newGround = UnityEditor.PrefabUtility.InstantiatePrefab(mapRegions.regions[i].block) as Ground;
                            newGround.transform.position = position;
                            newGround.transform.rotation = Quaternion.identity;
                            newGround.transform.parent = blockParent;
#else
							block = Instantiate(mapRegions.regions[i].block, position, Quaternion.identity, blockParent).GetComponent<Block>();
#endif
                        }

                        // newGround.gridPos = new Vector3(x, currentHeight, y);
                        AddGround(new Vector2(x, y), newGround);
                        // Block block = Instantiate(mapRegions.regions[i].block, position, Quaternion.identity, blockParent).GetComponent<Block>();
                        // block.SetMaterial(mapRegions.regions[i].blockType);

                        break;
                    }
                }
            }
        }

    }

    public void AddEntity(Vector2 position, Entity entity)
    {
        // Debug.Log("Entity: " + entity.gameObject.name);
        Instance.mapSave.entities[position] = entity;
        // baseGenerator.mapSave.entities.TryAdd(MapGenerator.PositionRound(position), e);
    }

    public void AddGround(Vector2 position, Ground ground)
    {
        // Debug.Log("Ground: " + ground.gameObject.name);
        Instance.mapSave.blocks[position] = ground;
    }

    public static GameObject SpawnObject(GameObject spawnGO, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject go = null;

        if (Application.isPlaying)
        {
            go = Instantiate(spawnGO, position, rotation, parent);
        }
        else
        {
#if UNITY_EDITOR
            go = UnityEditor.PrefabUtility.InstantiatePrefab(spawnGO) as GameObject;
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.transform.parent = parent;
#else
            go = Instantiate(spawnGO, position, rotation, parent);
#endif
        }


        return go;

    }

    public int Distance(Vector3 grid1, Vector3 grid2)
    {
        return (int)(Vector2.Distance(grid1, grid2));
    }

    public Ground[,] GetBlocksAtPoint(Ground[,] blocks, Vector3 centerPoint, int radius)
    {
        int startX = (int)centerPoint.x - radius;
        int startY = (int)centerPoint.z - radius;
        int endX = (int)centerPoint.x + radius;
        int endY = (int)centerPoint.z + radius;

        int sizeX = endX - startX + 1;
        int sizeY = endY - startY + 1;

        Ground[,] result = new Ground[sizeX, sizeY];

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                result[x - startX, y - startY] = blocks[x, y];
            }
        }

        return result;
    }

    public List<Vector2> GetBlocksInCircularRadius(Dictionary<Vector2, Ground> blocks, Vector3 centerPoint, int radius)
    {
        int startX = (int)centerPoint.x - radius;
        int startY = (int)centerPoint.z - radius;
        int endX = (int)centerPoint.x + radius;
        int endY = (int)centerPoint.z + radius;

        int sizeX = endX - startX + 1;
        int sizeY = endY - startY + 1;

        List<Vector2> result = new List<Vector2>();

        int centerX = sizeX / 2;
        int centerY = sizeY / 2;

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                int xOffset = x - centerX;
                int yOffset = y - centerY;

                if (xOffset * xOffset + yOffset * yOffset < radius * radius)
                {
                    int originalX = startX + x;
                    int originalY = startY + y;

                    result.Add(new Vector2(originalX, originalY));

                }
            }
        }

        return result;
    }


    public void ToggleFog() { ToggleFog(!fog); }
    public void ToggleFog(bool toggle)
    {
        fog = toggle;
        for (int y = 0; y < mapData.mapHeight; y++)
        {
            for (int x = 0; x < mapData.mapWidth; x++)
            {
                mapSave.blocks[new Vector2(x, y)].gameObject.SetActive(false);
            }
        }

        List<Vector2> fogBlocks = GetBlocksInCircularRadius(mapSave.blocks, WorldToGrid(Vector3.zero), 4);

        for (int i = 0; i < fogBlocks.Count; i++)
        {
            mapSave.blocks[fogBlocks[i]]?.gameObject.SetActive(true);
        }
    }

    public static void Merge(MapGenerator gen, MapGenerator modGen, Vector2 origin)
    {

        return;

        int startX = (int)(origin.x - modGen.mapData.mapWidth * .5f);
        int startY = (int)(origin.y - modGen.mapData.mapHeight * .5f);

        int iMod = 0;
        for (int i = startX; i < (int)(startX + modGen.mapData.mapWidth - 1f); i++)
        {

            int jMod = 0;
            for (int j = startY; j < (int)(startY + modGen.mapData.mapHeight - 1f); j++)
            {

                Vector2 v2 = new Vector2(i, j);
                Vector2 v2Local = new Vector2(iMod, jMod);

                if (gen.mapSave.noiseMap.ContainsKey(v2))
                {

                    float currentHeight = Mathf.Lerp(gen.mapSave.noiseMap[v2], modGen.mapSave.noiseMap[v2Local], modGen.mapSave.falloffMap[v2Local]);
                    // float currentHeight = modGen.noiseMap[iMod,jMod];
                    gen.mapSave.blendMap[v2] = modGen.mapSave.falloffMap[v2Local];
                    gen.mapSave.noiseMap[v2] = currentHeight;
                }

                jMod++;
            }

            iMod++;
        }

    }



    void OnValidate()
    {

        // Debug.Log("Editor validate");

        if (mapData.mapWidth < 1)
        {
            Debug.LogError("No map width");
            mapData.mapWidth = 1;
        }
        if (mapData.mapHeight < 1)
        {
            Debug.LogError("No map height");
            mapData.mapHeight = 1;
        }

        if (mapData.mapWidth % 2 == 0)
        {
            Debug.LogError("Must have odd numbered width");
            mapData.mapWidth += 1;
        }
        if (mapData.mapHeight % 2 == 0)
        {
            Debug.LogError("Must have odd numbered height");
            mapData.mapHeight += 1;
        }

        if (mapData.lacunarity < 1)
        {
            Debug.LogError("No map lacunarity");
            mapData.lacunarity = 1;
        }
        if (mapData.octaves < 0)
        {
            Debug.LogError("No octaves");
            mapData.octaves = 0;
        }

    }

    void OnDrawGizmos()
    {
        // Draws the Light bulb icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawWireCube(transform.position, Vector3.up * .01f + Vector3.right * mapData.mapWidth + Vector3.forward * mapData.mapHeight);
        Gizmos.DrawIcon(transform.position, "Map.png", true);
    }
}

[System.Serializable]
public struct TerrainType
{
    public GroundMaterial blockType;
    public float height;
    public Ground block;

}