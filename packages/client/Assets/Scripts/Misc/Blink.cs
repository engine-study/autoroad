using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
   
    public GameObject open, close;

    void OnEnable() {
        StartCoroutine(BlinkCoroutine());
    }
    IEnumerator BlinkCoroutine() {

        while(true) {
            open.SetActive(true);
            close.SetActive(false);
            yield return new WaitForSeconds(Random.Range(1f,5f));
            open.SetActive(false);
            close.SetActive(true);
            yield return new WaitForSeconds(.25f);
        }


    }
}
