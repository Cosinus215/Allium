using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    private CinemachineVirtualCamera cvc;
    void Start()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
        if (cvc != null) StartCoroutine(IntroSequence());
    }
    IEnumerator IntroSequence()
    {
        CinemachineTransposer ct = cvc.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 endPos = ct.m_FollowOffset;
        Vector3 startPos = new Vector3(0,21, -125);
        float time = 2;
        for (float t = 0; t<=time; t+=Time.deltaTime)
        {
            ct.m_FollowOffset = Vector3.Lerp(startPos, endPos, t / time);
            yield return null;
        }
        ct.m_FollowOffset = endPos;
    }
}
