using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMissile : MonoBehaviour
{
    public bool useGravity = true;

    public float thrustForce = 300000f;
    public float gravityForce = 9.81f;
    public float dragCoefficient = 0.04f;
    public float area = 0.21f;
    public float airDensity = 1.3f;
    public float accelerationTime = 10;

    private Vector3 resultingForce;

    private bool launched = false;
    private float launchTime;
    Transform missilePos;
    Rigidbody rb;

    void Start()
    {
        missilePos = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (launched) 
        {
            if (launchTime + accelerationTime > Time.time)
            {
                ApplyThrust();
            }
            else
            {
                resultingForce = Vector3.zero;
            }
            ApplyGravity();
            ApplyDrag();
        }
        //ApplyFriction();
        //Vector3 force = new(transform.forward.x * thrustForce, useGravity ? gravityForce : transform.forward.y * thrustForce, transform.forward.z * thrustForce);
        //rb.AddForce(force * Time.deltaTime);
        rb.AddForce(resultingForce);
    }

    public void Launch()
    {
        launched = true;
        launchTime = Time.time;
    }

    private void ApplyFriction()
    {
        throw new NotImplementedException();
    }

    private void ApplyThrust()
    {
        resultingForce = transform.forward * thrustForce;
        rb.AddForce(resultingForce);
    }

    private void ApplyDrag()
    {
        Vector3 airResistance = dragCoefficient * area * airDensity * rb.velocity.sqrMagnitude * -rb.velocity.normalized / 2;
        Debug.Log(resultingForce + " " + airResistance);
        resultingForce += airResistance;
        
    }
    private void ApplyGravity()
    {
        Vector3 gravity = new Vector3(0, -gravityForce * rb.mass, 0);
        resultingForce += gravity;
    }


}
