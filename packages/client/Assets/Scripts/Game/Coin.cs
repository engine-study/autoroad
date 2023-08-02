using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Vector3 start;
    Transform target;
    [SerializeField] private AudioClip [] sfx_spawn, sfx_recieve;
    

    public void TipCoin(Transform newTarget) {
        target = newTarget;
        StartCoroutine(CoinAnimation());
    }

    IEnumerator CoinAnimation() {

        start = transform.position;
        
        SPAudioSource.Play(transform.position, sfx_spawn);

        float randomHeight = Random.Range(4f, 5f);

        float lerp = 0f;
        while(lerp < 1f) {
            
            transform.position = Vector3.Lerp(start, target.position, lerp) + Vector3.up * randomHeight * Mathf.Sin(lerp * Mathf.PI);
            transform.Rotate(Vector3.up * Time.deltaTime * 720f);
            transform.Rotate(Vector3.right * Time.deltaTime * 1080f);

            lerp += Time.deltaTime * .75f;

            yield return null;
        }

        SPAudioSource.Play(transform.position, sfx_recieve);

        Destroy(gameObject);
    } 
}
