using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sheep : MonoBehaviour {
    public Transform Destination;
    private NavMeshAgent agent;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        Destination.localPosition = new Vector3(Random.Range(-55f, 72f), 1, Random.Range(20f, -90f));

        StartCoroutine("NewPlace");
        StartCoroutine("Stop");
        StartCoroutine("Starting");
    }

    void Update() {
        agent.SetDestination(Destination.localPosition);
    }

    private IEnumerator NewPlace() {
        while (true) {
            yield return new WaitForSeconds(30);
            agent.speed = 1.4f;
            Destination.localPosition = new Vector3(Random.Range(-55f, 72f), 1, Random.Range(20f, -90f));
        }
    }

    private IEnumerator Stop() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(10f, 50f));
            agent.speed = 0;
        }
    }

    private IEnumerator Starting() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(10f, 50f));
            agent.speed = 1.4f;
        }
    }
}
