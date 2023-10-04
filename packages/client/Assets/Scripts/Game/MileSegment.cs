using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MileSegment : MonoBehaviour
{
    [Header("Segment")]
    [SerializeField] GameObject wip;
    [SerializeField] GameObject finished;
    public void ToggleFinished(bool toggle) {
        wip.SetActive(!toggle);
        finished.SetActive(toggle);
    }
}
