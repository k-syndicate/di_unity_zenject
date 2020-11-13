using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Range(0, 1f)]
    [Tooltip("Flicker magnitude")]
    public float intensity = 0.5f;
    [Tooltip("Amount of change applied to light position per axis")]
    public Vector3 wobble = Vector3.one;
    [Range(0.1f, 100)]
    [Tooltip("Frequency of flicker in cycles per second")]
    public float speed = 60;
    [Range(1, 3)]
    [Tooltip("Quality of effect")]
    public int quality = 1;

    Light[] lights;
    LightSource[] lightSources;
    Vector3 v = new Vector3();

    struct LightSource
    {
        public Light source;
        public float intensity;
        public float targetIntensity;
        public Vector3 position;
        public Vector3 targetPosition;

        public LightSource(Light source)
        {
            this.source = source;
            this.intensity = source.intensity;
            this.targetIntensity = source.intensity;
            this.position = source.transform.position;
            this.targetPosition = source.transform.position;
        }
    }

    void OnEnable()
    {
        lights = GetComponentsInChildren<Light>();
        lightSources = new LightSource[lights.Length];

        for (int i = 0; i < lights.Length; i++)
        {
            lightSources[i] = new LightSource(lights[i]);
        }

        InvokeRepeating("Flicker", 0, 1 / speed);
    }

    void Flicker()
    {
        for (int i = 0; i < lightSources.Length; i++)
        {
            lightSources[i].targetIntensity = lightSources[i].intensity * (Random.Range(1 - intensity, 1));

            if (quality == 3)
            {
                v.x = (((1 - (Random.Range(1 - intensity, 1))) * 2) - intensity) * wobble.x;
                v.y = (((1 - (Random.Range(1 - intensity, 1))) * 2) - intensity) * wobble.y;
                v.z = (((1 - (Random.Range(1 - intensity, 1))) * 2) - intensity) * wobble.z;
                lightSources[i].targetPosition = lightSources[i].position + v;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < lightSources.Length; i++)
        {
            if (quality == 3)
            {
                lightSources[i].source.intensity = Mathf.Lerp(lightSources[i].source.intensity, lightSources[i].targetIntensity, Time.deltaTime * 10);
                lightSources[i].source.transform.position = Vector3.Lerp(lightSources[i].source.transform.position, lightSources[i].targetPosition, Time.deltaTime * 3);
                continue;
            }

            if (quality == 2)
            {
                lightSources[i].source.intensity = Mathf.Lerp(lightSources[i].source.intensity, lightSources[i].targetIntensity, Time.deltaTime * 10);
                continue;
            }

            lightSources[i].source.intensity = lightSources[i].targetIntensity;
        }
    }
}
