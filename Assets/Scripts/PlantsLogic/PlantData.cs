using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plants/Plant data", fileName = "NewPlantData")]
public class PlantData : ScriptableObject
{
    [SerializeField] private string plantName = "default plant";
    [SerializeField] private Sprite plantIcon;
    [SerializeField] private Texture[] plantTextures;
    [SerializeField] private int plantGrowthTime = 20;
    public string GetPlantName()
    {
        return plantName;
    }
    public Sprite GetPlantIcon()
    {
        return plantIcon;
    }
    public Texture GetPlantTexture(int id)
    {
        id = Mathf.Clamp(id, 0, plantTextures.Length - 1);
        return plantTextures[id];//current texture id
    }
    public int GrowthStages()
    {
        return plantTextures.Length;
    }
    public int GetPlantGrowthTime()
    {
        return plantGrowthTime;
    }

}
