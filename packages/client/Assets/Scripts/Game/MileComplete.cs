using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class MileComplete : MonoBehaviour
{
    public static MileComplete Instance;

    [Header("Mile")]
    [SerializeField] GameObject mileUI;
    [SerializeField] SPHeading header;
    [SerializeField] List<GemComponent> gems;
    Coroutine coroutine;
    public static void AddGem(GemComponent gem) {
        Instance.gems.Add(gem);
    }

    void Awake() {
        Instance = this;
        mileUI.SetActive(false);
        GameStateComponent.OnMileCompleted += PlayMileRewardSequence;
    }

    void OnDestroy() {
        Instance = null;
        GameStateComponent.OnMileCompleted -= PlayMileRewardSequence;
    }

    public void PlayMileRewardSequence() {

        if(GameStateComponent.MILE_COUNT == 0) {
            return;
        }

        PlayMileRewardSequence((int)GameStateComponent.MILE_COUNT - 1);
    }

    void PlayMileRewardSequence(int mile) {
        if(coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(MileEndCoroutine());
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

                if(road == null || road.Gem == false) {
                    continue;
                }

                yield return StartCoroutine(PresentReward(road));
            }
        }

        yield return new WaitForSeconds(1f);

    }

    IEnumerator PresentReward(RoadComponent road) {

        if(road.FilledBy == null) {
            Debug.LogError("No player credited?", road);
            yield break;
        }

        SPCamera.SetFollow(road.transform);

        yield return new WaitForSeconds(2f);

        PlayerMUD player = road.FilledBy.PlayerScript;
        SPResourceJuicy gem = SPResourceJuicy.GiveResource("Prefabs/Gem", player.transform, road.transform.position + Vector3.up, Quaternion.identity);
        SPCamera.SetFollow(gem.transform);

        while (gem.gameObject.activeInHierarchy) { yield return null; }

        yield return new WaitForSeconds(2f);

    }
}
