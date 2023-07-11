using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScroll : MonoBehaviour
{

    [Header("World Scroll")]
    public SPHeading mileHeading;

    float mileScroll, lastScroll = -100f;
    float mileCount = -1f;

    public float MileTotal { get { return mileCount * MILE_DISTANCE; } }
    public float MileTotalScroll { get { return mileScroll * MILE_DISTANCE; } }
    public static float MILE_DISTANCE = 20f;

    void Start()
    {
        SetMile(-1f);
    }

    void Update()
    {
        if (SPUIBase.IsMouseOnScreen)
        {

            mileScroll += Input.mouseScrollDelta.y * .1f;
            // scrollLock = Mathf.Round(mileScroll / 90) * 90;

        }

        //magnetism, lerp back to current mile
        mileScroll = Mathf.MoveTowards(mileScroll, mileCount, 1f * Time.deltaTime);

        //if we're more than halfway to the next mile, magnet over to it
        if (Mathf.Abs((mileScroll * MILE_DISTANCE) - MileTotal) > MILE_DISTANCE * .5f)
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

    public void SetMile(float newMile)
    {

        // mileHeading.UpdateField("Mile " + newMile);

        mileCount = newMile;
        SPCamera.SetTarget(Vector3.forward * MileTotal);

    }


}
