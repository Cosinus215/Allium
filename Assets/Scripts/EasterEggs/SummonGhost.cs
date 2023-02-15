using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class SummonGhost : MonoBehaviour
{
    [SerializeField] private Candle[] candles;
    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private TextMeshPro tmp;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private ParticleSystem particlesOut;
    private void Start()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ShowGhost(false);
        }
        tmp.text = "";
    }
    public void Check()
    {
        for (int i=0; i<candles.Length; i++)
        { 
            if (candles[i].IsLit)return;
        }
        StartCoroutine(GhostArrival());
    }

    IEnumerator GhostArrival()
    {
        particles.Play();
        int a = Random.Range(0, ghosts.Length);
        ghosts[a].ShowGhost(true);
        tmp.text = ghosts[a].GetRandomQuote();
        yield return new WaitForSeconds(5);
        particlesOut.Play();
        yield return new WaitForSeconds(1.25f);
        tmp.text = "";
        ghosts[a].ShowGhost(false);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < candles.Length; i++)
        {
            candles[i].LitFire();
        }
    }
}
[System.Serializable]
public class Ghost
{
    public string name;
    public GameObject appearance;
    public string[] quotes;

    public void ShowGhost(bool status)
    {
        appearance.SetActive(status);
    }
    public string GetRandomQuote()
    {
        return quotes[Random.Range(0,quotes.Length)];
    }
}
