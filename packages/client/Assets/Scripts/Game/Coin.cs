using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Vector3 start;
    Transform target;
    [SerializeField] private AudioClip [] coinSounds;

    IEnumerator CoinAnimation() {

        start = transform.position;
        
        float lerp = 0f;
        while(lerp < 1f) {
            
            
            lerp += Time.deltaTime;
            yield return null;
        }
        

    } 
}
