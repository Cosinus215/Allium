using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour ,IInteractable
{
    [SerializeField] [Multiline] [NonReorderable] private string[] line;
    [SerializeField] private TextMeshPro tmp;
    bool a = false;
    void Start()
    {
        tmp.SetText("");
    }
    public bool Interact(Items item = null)
    {
        if (!a)
        {
            StartCoroutine(DialogSequence());
            return true;
        }
        return false;
    }
    IEnumerator DialogSequence()
    {
        a=true;
        tmp.SetText(line[Random.Range(0, line.Length)]);
        yield return new WaitForSeconds(4);
        tmp.SetText("");
        a = false;
    }
}
