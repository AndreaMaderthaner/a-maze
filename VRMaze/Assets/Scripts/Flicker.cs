using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//light flicker for fire
public class Flicker : MonoBehaviour
{
    public float minIntensity = 2.0f;
    public float maxIntensity = 3.0f;
    public float minRange = 2.0f;
    public float maxRange = 3.0f;
    Light light;
    // Start is called before the first frame update
    void Start()
    {
        light = this.GetComponentInChildren<Light>();
        InvokeRepeating("flicker", 0.05f, 0.05f);
    }

    private void flicker()
    {
        float intensity = Random.Range(minIntensity, maxIntensity);
        light.intensity = intensity;
        float range = Random.Range(minRange, maxRange);
        light.range = range;
    }
}
