using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Portal : MonoBehaviour
{
    Player p;
    [SerializeField] private Sprite curSkin;
    [SerializeField] private Sprite[] skins;
    float nextChange = 0;
    public UnityEvent e;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(p == null) {p=other.GetComponent<Player>();}
            if (p && Time.time >= nextChange)
            {
                int a = Random.Range(0, skins.Length);
                p.ChangeAppearance(skins[a]);

                Sprite bufor = skins[a];
                skins[a] = curSkin;
                curSkin = bufor;
                nextChange = Time.time + 2;
                e.Invoke();
            }
        }
    }
}
