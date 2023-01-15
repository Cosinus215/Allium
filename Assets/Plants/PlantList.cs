using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Plants/Plant list", fileName = "NewPlantList")]

public class PlantList : ScriptableObject
{
    public List<PlantData> Plants = new List<PlantData>();
    [ContextMenu("Set Plants ID")]
    public void SetPlantsID()
    {
        for(int i=0;i<Plants.Count;i++)
        {
            Plants[i].SetPlantID(i);
        }
    }
}
