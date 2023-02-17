using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/New tool", fileName = "NewTool")]
public class Items : ScriptableObject {
    public string Name;
    public Sprite Icon;
    public Texture Texture;
    public int EffectNumber = -1;
    public enum Type {Axe, Hoe, Shovel, Can, Bundle};
    public Type itemType;
}

