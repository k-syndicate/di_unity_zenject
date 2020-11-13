using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeLightStreak : MonoBehaviour
{
    public Vector2 streakWidthRange = new Vector2(2, 2);

    void Awake()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        Light l = GetComponent<Light>();
        float r = Random.Range(streakWidthRange.x, streakWidthRange.y);

        if (lr != null)
        {
            lr.startWidth = r;
            lr.endWidth = r;
        }

        if (l != null && l.type == LightType.Spot)
        {
            l.spotAngle = r * 3;
        }
    }


}
