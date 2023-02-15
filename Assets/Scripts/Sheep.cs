using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sheep : MonoBehaviour {
    public Transform k;
    public NavMeshAgent agent;

    void Update() {
        agent.SetDestination(k.position);
    }
}
