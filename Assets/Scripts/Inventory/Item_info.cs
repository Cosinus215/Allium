using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_info : MonoBehaviour {
    public string Name;
    public Sprite Icon;
    public Material Texture;

    private void Start() {
        Texture = GetComponent<MeshRenderer>().material;
    }
}
