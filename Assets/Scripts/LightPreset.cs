using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "World/Lighting Preset")]
public class LightPreset : ScriptableObject
{
    public Gradient ambientColor;
    public Gradient directionalColor;
    public Gradient fogColor;
}
