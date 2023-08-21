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
    static Coroutine coroutine;
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

    public static void PlayMileRewardSequence() {
        PlayMileRewardSequence((int)WorldScroll.Mile);
    }

    public static void PlayMileRewardSequence(int mile) {
        if(coroutine != null) Instance.StopCoroutine(coroutine);
        coroutine = Instance.StartCoroutine(Instance.MileEndCoroutine(mile));
    }

    void SetCameraToMile(int mile) {
        SPCamera.SetFollow(null);
        SPCamera.SetTarget(Vector3.forward * mile * GameStateComponent.MILE_DISTANCE * .5f);
        SPCamera.SetFOVGlobal(10f);
    }
    IEnumerator MileEndCoroutine(int mile) {

        SetCameraToMile(mile);

        mileUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        mileUI.SetActive(false);


        ChunkComponent chunk = ChunkComponent.Chunks[mile];

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

        SetCameraToMile(mile);

    }

    IEnumerator PresentReward(RoadComponent road) {

        if(road.FilledBy == null) {
            Debug.LogError("No player credited?", road);
            yield break;
        }

        SPCamera.SetFollow(road.transform);

        while (SPCamera.I.transform.position != road.transform.position) { yield return null; }
        yield return new WaitForSeconds(.25f);

        PlayerMUD player = road.FilledBy.PlayerScript;
        SPResourceJuicy gem = SPResourceJuicy.GiveResource("Prefabs/Gem", player.transform, road.transform.position + Vector3.up, Quaternion.identity);
        SPCamera.SetFollow(gem.transform);

        while (gem != null) { yield return null; }

        SPCamera.SetFollow(player.headCosmetic.bodyParent);

        yield return new WaitForSeconds(.5f);

    }
}
