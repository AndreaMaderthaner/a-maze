using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//light flicker for fire
public class Flicker : MonoBehaviour
{
    public float minIntensity = 2.0f;
    public float maxIntensity = 3.0f;
    public float time = 0.1f;
    Light light;
    // Start is called before the first frame update
    void Start()
    {
        light = this.GetComponentInChildren<Light>();
        InvokeRepeating("flicker", time, time);
    }

   private void flicker(){
    float intensity = Random.Range(minIntensity,maxIntensity);
    light.intensity=intensity;
   }
}