using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer octiAppearance;
    [SerializeField] private ParticleSystem particles;
    public void ChangeAppearance(Sprite newSkin)
    {
        octiAppearance.sprite = newSkin;
        particles.Play();
    }
}
