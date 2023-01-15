using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plants/Plant data", fileName = "NewPlantData")]
public class PlantData : ScriptableObject
{
    [SerializeField] public string plantName = "default plant";
    [SerializeField] public Sprite seedIcon;
    [SerializeField] public Texture[] plantTextures;
    [SerializeField] public int plantGrowthTime = 20;
    [SerializeField] private int plantGameID = 0;
    public int Price;
    public string GetPlantName()
    {
        return plantName;
    }
    public Sprite GetPlantIcon()
    {
        return seedIcon;
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
    public void SetPlantID(int id)
    {
        plantGameID = id;
    }
    public int GetPlantID()
    {
        return plantGameID;
    }
}
