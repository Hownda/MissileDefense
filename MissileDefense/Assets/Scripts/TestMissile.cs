using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMissile : MonoBehaviour
{
    public int missileID;

    public float thrustForce = 150000f;
    public float gravityForce = 9.81f;
    public float dragCoefficient = 0.7f;
    public float areaFront = 0.21f;
    public float areaSide = 2.55f;
    public float airDensity = 1.3f;
    public float accelerationTime = 10f;
    public float correctionDelay = 0.2f;

    private float startThrust = 0;
    private float thrust = 0;
    public float fuelMass = 250;

    private Vector3 resultingForce;

    private bool launched = false;
    private bool turnHorizontal = false;
    private bool adjust = false;
    private bool guidance = false;
    private float launchTime;
    Transform missilePos;
    Rigidbody rb;

    void Start()
    {
        missileID = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1000000));
        missilePos = GetComponent<Transform>();
        Time.timeScale = 1f;
        startThrust = thrustForce / 3;
        thrust = startThrust;
        rb = GetComponent<Rigidbody>();
        rb.inertiaTensor = new Vector3(60f, 50.7f, 50.7f);
        missilePos = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (launched) 
        {
            if (launchTime + accelerationTime > Time.time)
            {
                ApplyThrust();
                BurnFuel();
            }
            else
            {
                resultingForce = Vector3.zero;
            }

            if (turnHorizontal)
            {
                Debug.Log("Flip");
                rb.AddForceAtPosition(100f * -transform.up * 2, transform.position + transform.forward * 3.125f);
            }
            if (adjust)
            {
                Debug.Log("Adjust");
                rb.AddForceAtPosition(103f * transform.up * 5, transform.position + transform.forward * 3.125f);
            }
            if (guidance)
            {
                rb.angularVelocity = Vector3.zero;
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.Euler(-5, rb.rotation.eulerAngles.y, rb.rotation.eulerAngles.z), 0.05f));
            }

            ApplyGravity();
            ApplyDrag();
        }
        //ApplyFriction();
        //Vector3 force = new(transform.forward.x * thrustForce, useGravity ? gravityForce : transform.forward.y * thrustForce, transform.forward.z * thrustForce);
        //rb.AddForce(force * Time.deltaTime);
        rb.AddForceAtPosition(resultingForce, transform.position);
    }

    public void Launch()
    {
        launched = true;
        launchTime = Time.time;
        StartCoroutine(CorrectionDelay());
    }

    private IEnumerator CorrectionDelay()
    {
        yield return new WaitForSeconds(correctionDelay);
        turnHorizontal = true;
        yield return new WaitForSeconds(0.5f);
        turnHorizontal = false;
        adjust = true;
        Debug.Log(transform.rotation.eulerAngles);
        yield return new WaitForSeconds(0.2f);
        adjust = false;
        thrust = thrustForce;
        guidance = true;

    }

    private void ApplyThrust()
    {
        resultingForce = transform.forward * thrust;
    }

    private void BurnFuel()
    {
        rb.mass -= fuelMass / accelerationTime * Time.fixedDeltaTime;
    }

    private void ApplyFriction()
    {
        throw new NotImplementedException();
    }

    private void ApplyDrag()
    {
        Vector3 airResistance = dragCoefficient * areaFront * airDensity * rb.velocity.sqrMagnitude * -rb.velocity.normalized / 2;
        //Debug.Log(resultingForce + " " + airResistance);
        resultingForce += airResistance;
        
    }
    private void ApplyGravity()
    {
        Vector3 gravity = new Vector3(0, -gravityForce * rb.mass, 0);
        resultingForce += gravity;
    }


}
