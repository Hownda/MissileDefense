using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMissile : MonoBehaviour
{
    public bool useGravity = true;

    public float thrustForce = 50f;
    public float gravityForce = 9.81f;

    Transform missilePos;
    Rigidbody rb;

    void Start()
    {
        missilePos = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 force = new(transform.forward.x * thrustForce, useGravity ? gravityForce : transform.forward.y * thrustForce, transform.forward.z * thrustForce);
        rb.AddForce(force * Time.deltaTime);
    }
}
