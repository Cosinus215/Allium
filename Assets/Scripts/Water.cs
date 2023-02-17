using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
    public float waterLevel;
    public float floatThreshold;
    public float waterDensity;
    public float downForce;

    private float forceFactor;
    private Vector3 floatForce;

    void Start() {
        forceFactor = 1.0f - ((waterLevel - transform.position.y) / floatThreshold);
    }

    void Update() {
        if (forceFactor > 0.0f) {
            floatForce = -Physics.gravity * (forceFactor - GetComponent<Rigidbody>().mass * waterDensity);
            floatForce += new Vector3(0.0f, -downForce, 0.0f);
            GetComponent<Rigidbody>().AddForceAtPosition(floatForce, transform.position);
        }
    }
}
