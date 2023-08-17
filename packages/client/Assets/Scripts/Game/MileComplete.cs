using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MileComplete : MonoBehaviour
{
    public static MileComplete Instance;

    [Header("Mile")]
    [SerializeField] GameObject mileUI;
    [SerializeField] SPHeading header;
    [SerializeField] List<GemComponent> gems;

    public static void AddGem(GemComponent gem) {
        Instance.gems.Add(gem);
    }

    void Awake() {
        Instance = this;
        mileUI.SetActive(false);
        GameStateComponent.OnMileCompleted += MileCompletion;
    }

    void OnDestroy() {
        Instance = null;
        GameStateComponent.OnMileCompleted -= MileCompletion;
    }

    void MileCompletion() {

        Debug.Log("MILE " + GameStateComponent.MILE_COUNT + " COMPLETED", this);
        StartCoroutine(MileEndCoroutine());
    }


    IEnumerator MileEndCoroutine() {

        int completedMile = (int)GameStateComponent.MILE_COUNT - 1;

        mileUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        mileUI.SetActive(false);


        ChunkComponent chunk = ChunkComponent.Chunks[completedMile];

        for (int y = 0; y < chunk.Rows.Length; y++) {
            for (int x = 0; x < chunk.Rows[y].Roads.Length; x++) {
                RoadComponent road = chunk.Rows[y].Roads[x];

                if(road.Gem == false) {
                    continue;
                } 

                

                yield return new WaitForSeconds(1f);
            }
        }
            for (int i = gems.Count - 1; i > -1; i--)
            {
                // gems[i]
                yield return new WaitForSeconds(1f);
            }

        yield return new WaitForSeconds(1f);


    }
}
