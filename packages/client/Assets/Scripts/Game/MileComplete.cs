using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class MileComplete : MonoBehaviour
{
    public static MileComplete Instance;

    [Header("UI")]
    [SerializeField] GameObject mileUI;
    [SerializeField] SPHeading header;
    [SerializeField] SPButton gemUI;
    [SerializeField] AudioClip sfx_mileComplete;
    [SerializeField] AudioClip sfx_gemSpawn;
    [SerializeField] AudioClip sfx_gemSend;

    [Header("Mile")]
    [SerializeField] List<PlayerMUD> players;
    [SerializeField] List<RoadComponent> roads;
    [SerializeField] List<SPResourceJuicy> gems;

    static Coroutine coroutine;
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
        PlayMileRewardSequence((int)WorldScroll.Mile, false);
    }

    public static void PlayMileRewardSequence(int mile) { PlayMileRewardSequence(mile, true); }

    public static void PlayMileRewardSequence(int mile, bool showComplete) {
        if(coroutine != null) Instance.StopCoroutine(coroutine);
        coroutine = Instance.StartCoroutine(Instance.MileEndCoroutine(mile, showComplete));
    }

    void SetCameraToMile(int mile) {
        SPCamera.SetFollow(null);
        SPCamera.SetTarget(Vector3.forward * ((mile * GameStateComponent.MILE_DISTANCE) + (GameStateComponent.MILE_DISTANCE * .5f)));
        SPCamera.SetFOVGlobal(10f);
    }
    IEnumerator MileEndCoroutine(int mile, bool showComplete) {

        SPUIBase.PlaySound(sfx_mileComplete);

        mileUI.SetActive(true);
        gemUI.ToggleWindowClose();

        if (showComplete) {
            header.ToggleWindowOpen();
            yield return new WaitForSeconds(2.5f);
        } 

        header.ToggleWindowClose();
        SetCameraToMile(mile);

        yield return new WaitForSeconds(2.5f);

        gemUI.ToggleWindowOpen();
        gemUI.UpdateField("0");

        ChunkComponent chunk = ChunkLoader.Chunks[mile];
        players = new List<PlayerMUD>();
        roads = new List<RoadComponent>();
        gems = new List<SPResourceJuicy>();

        for (int y = 0; y < chunk.Rows.Length; y++) {
            for (int x = 0; x < chunk.Rows[y].Roads.Length; x++) {

                RoadComponent road = chunk.Rows[y].Roads[x];
                if(road == null || road.Gem == false) { continue; }
                if(road.FilledBy == null) { Debug.LogError("No player credited?", road); continue; }
                PlayerMUD player = road.FilledBy.PlayerScript;
                SPResourceJuicy gem = SPResourceJuicy.SpawnResource("Prefabs/Gem", player.headCosmetic.bodyParent, road.transform.position + Vector3.up, Quaternion.identity);

                players.Add(player);
                roads.Add(road);
                gems.Add(gem);

                SPUIBase.PlaySound(sfx_gemSpawn, 1f, .8f + (gems.Count * .05f));
                gemUI.UpdateField(gems.Count.ToString());

                yield return new WaitForSeconds(.5f);

            }
        }


        yield return new WaitForSeconds(1f);

        gemUI.ToggleWindowClose();
        SPUIBase.PlaySound(sfx_gemSend);

        for (int i = 0; i < gems.Count; i++) {
            gems[i].SendResource();
            // yield return StartCoroutine(PresentReward(roads[i], gems[i], players[i]));
        }

        mileUI.SetActive(false);
        yield return new WaitForSeconds(1f);

    }

    IEnumerator PresentReward(RoadComponent road, SPResourceJuicy gem, PlayerMUD player) {

        SPCamera.SetFollow(gem.transform);

        while (SPCamera.I.transform.position != gem.transform.position) { yield return null; }

        yield return new WaitForSeconds(1f);
        gem.SendResource();
        while (gem != null) { yield return null; }

        SPCamera.SetFollow(player.headCosmetic.bodyParent);
        yield return new WaitForSeconds(.5f);

    }
}
