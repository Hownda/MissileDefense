using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMissile : MonoBehaviour
{
    public bool useGravity = true;

    public float thrustForce = 10f;
    public float gravityForce = 9.81f;
    public float dragCoefficient = 0.04f;
    public float diameter = 0.2f;
    public float airDensity = 1.3f;

    private Vector3 resultingForce;

    Transform missilePos;
    Rigidbody rb;

    void Start()
    {
        missilePos = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ApplyThrust();
        ApplyDrag();
        //ApplyFriction();
        //ApplyGravity();
        //Vector3 force = new(transform.forward.x * thrustForce, useGravity ? gravityForce : transform.forward.y * thrustForce, transform.forward.z * thrustForce);
        //rb.AddForce(force * Time.deltaTime);       
    }

    private void ApplyGravity()
    {
        throw new NotImplementedException();        
    }

    private void ApplyFriction()
    {
        throw new NotImplementedException();
    }

    private void ApplyThrust()
    {
        resultingForce = transform.forward * thrustForce;
    }

    private void ApplyDrag()
    {
        float area = Mathf.Pow(diameter / 2, 2) * Mathf.PI;
        Vector3 airResistance = dragCoefficient * area * airDensity * rb.velocity.sqrMagnitude * -rb.velocity.normalized / 2;
        resultingForce += airResistance;
        rb.AddForce(resultingForce);
    }


}
