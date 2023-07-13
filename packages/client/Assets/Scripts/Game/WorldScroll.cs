using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScroll : MonoBehaviour
{

    [Header("World Scroll")]
    public SPHeading mileHeading;
    public float maxMile = 0f;
    public float currentMile = -1f;

    float mileScroll, lastScroll = -100f;

    public float MileTotal { get { return currentMile * GameStateComponent.MILE_DISTANCE; } }
    public float MileTotalScroll { get { return mileScroll * GameStateComponent.MILE_DISTANCE; } }

    void Start()
    {
        SetMile(-1f);
    }

    void Update()
    {
        if (SPUIBase.CanInput && SPUIBase.IsMouseOnScreen && Input.GetKey(KeyCode.LeftAlt)) {
            mileScroll += Input.mouseScrollDelta.y * .1f;
            // scrollLock = Mathf.Round(mileScroll / 90) * 90;
        }

        //magnetism, lerp back to current mile
        mileScroll = Mathf.MoveTowards(mileScroll, currentMile, 1f * Time.deltaTime);

        //if we're more than halfway to the next mile, magnet over to it
        if (Mathf.Abs((mileScroll * GameStateComponent.MILE_DISTANCE) - MileTotal) > GameStateComponent.MILE_DISTANCE * .5f)
        {
            SetMile(Mathf.Round(mileScroll));
        }

        if (mileScroll != lastScroll)
            SPCamera.SetTarget(Vector3.forward * MileTotalScroll);

        lastScroll = mileScroll;
    }
    public void LoadInto()
    {
        SetMile(0f);
    }

    public void SetMaxMile(float newMaxMile) {
        maxMile = newMaxMile;
    }
    public void SetMile(float newMile)
    {

        // mileHeading.UpdateField("Mile " + newMile);

        currentMile = Mathf.Clamp(newMile,0f, maxMile);
        SPCamera.SetTarget(Vector3.forward * MileTotal);

    }


}
