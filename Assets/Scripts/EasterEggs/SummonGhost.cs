using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonGhost : MonoBehaviour
{
    public Candle[] candles;
    public Ghost[] ghosts;

    private void Start()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ShowGhost(false);
        }
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
        int a = Random.Range(0, ghosts.Length);
        ghosts[a].ShowGhost(true);
        yield return new WaitForSeconds(10);
        ghosts[a].ShowGhost(false);
        yield return new WaitForSeconds(2);
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

    public void ShowGhost(bool status)
    {
        appearance.SetActive(status);
    }
}
