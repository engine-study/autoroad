using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CoinMinigameUI : MonoBehaviour
{
    bool loading = false;
    [SerializeField] Volume volume;
    [SerializeField] SPClickableGameobject clickable;
    [SerializeField] SPMouseRotate rotate;
    [SerializeField] SPAudioSource sfx;
    [SerializeField] float cameraWander = 1f;
    [SerializeField] float cameraMagnitude = 1f;
    [SerializeField] float fov = 100f;
    [SerializeField] float volumeMultiplier = .1f, pitchMultiplier = .1f;
    [SerializeField] Vector2 pitch;
    [SerializeField] SPAudioSource sfx_hover;
    [SerializeField] SPAudioSource sfx_launched;
    [SerializeField] GameObject [] coins;
    [SerializeField] GameObject coinParent;
    [SerializeField] GameObject coinHover;
    [SerializeField] Canvas canvas;
    
    Vector3 cameraPos;
    int coinIndex = 0;
    public void Awake() {
        canvas.gameObject.SetActive(false);
        Hover();
    }

    public void Toggle(bool toggle) {
        if(toggle) {
            // SPCamera.SetFollow(transform);
            // SPCamera.SetFOVGlobal(2f);
        }
    }

    void Update() {
        sfx.Source.volume = Mathf.Clamp(rotate.Delta.magnitude * volumeMultiplier, 0f, 1f);
        sfx.Source.pitch = Mathf.Clamp(rotate.Momentum.magnitude * pitchMultiplier, pitch.x, pitch.y);

        cameraPos += Random.insideUnitSphere;
        cameraPos = Vector3.ClampMagnitude(cameraPos, cameraMagnitude);
    
        SPCamera.SetFOVGlobal(cameraPos.magnitude + fov);
        Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, cameraPos, .025f);
    }

    float lastHover;
    public void Hover() {

        coinIndex = Random.Range(0,coins.Length-1);
        for(int i = 0; i < coins.Length; i++) { coins[i].SetActive(i == coinIndex);}

        coinParent.SetActive(!clickable.Hovering);
        coinHover.SetActive(clickable.Hovering);

        if(clickable.Hovering && Time.time - lastHover > .1f) {
            sfx_hover.Play();
        }

        lastHover = Time.time;
    }

    public void LoadGame() {
        if(loading) {return;}
        loading = true;

        rotate.gameObject.SetActive(false);
        canvas.gameObject.SetActive(true);
        sfx_launched.Play();

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
