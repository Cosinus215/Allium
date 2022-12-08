using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weather
{
    public string weatherName = "default weather";
    public LightPreset lightPreset;
    public ParticleSystem particleEffect;
    public bool isWatering = false;

    public void ToogleWeather(bool active)
    {
        if (particleEffect != null)
        {
            if (active)
            {
                particleEffect.Play();
            }
            else
            {
                particleEffect.Stop();
            }
        }
    }
}
