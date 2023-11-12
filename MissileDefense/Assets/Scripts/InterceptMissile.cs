using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using static UnityEngine.GraphicsBuffer;

public class InterceptMissile : MonoBehaviour
{
    public float thrustForce = 150000f;
    public float gravityForce = 9.81f;
    public float dragCoefficient = 0.7f;
    public float areaFront = 0.21f;
    public float areaSide = 2.55f;
    public float airDensity = 1.3f;
    public float accelerationTime = 20f;
    public float correctionDelay = 0.2f;

    private float startThrust = 0;
    private float thrust = 0;
    public float fuelMass = 250;

    private Vector3 resultingForce;

    private bool launched = false;
    private bool guidance = false;
    private float launchTime;
    Rigidbody rb;

    private GameObject target;

    void Start()
    {
        Time.timeScale = 1f;
        startThrust = thrustForce / 3;
        thrust = startThrust;
        rb = GetComponent<Rigidbody>();
        rb.inertiaTensor = new Vector3(60f, 50.7f, 50.7f);
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= 100)
        {
            Debug.Log("Close");
        }

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

            if (guidance)
            {
                rb.angularVelocity = Vector3.zero;

                // Rotate towards target
                Vector3 direction = target.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, 20 * Time.fixedDeltaTime));
                //rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.Euler(-5, rb.rotation.eulerAngles.y, rb.rotation.eulerAngles.z), 0.05f));
            }

            ApplyGravity();
            ApplyLift();
            ApplyDrag();
        }
        //ApplyFriction();
        //Vector3 force = new(transform.forward.x * thrustForce, useGravity ? gravityForce : transform.forward.y * thrustForce, transform.forward.z * thrustForce);
        //rb.AddForce(force * Time.deltaTime);
        rb.AddForceAtPosition(resultingForce, transform.position);
    }

    public void Launch(GameObject target)
    {
        launched = true;
        launchTime = Time.time;

        this.target = target;
    }

    private void ApplyThrust()
    {
        resultingForce = transform.forward * thrust;
    }

    private void BurnFuel()
    {
        rb.mass -= fuelMass / accelerationTime * Time.fixedDeltaTime;
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

    private void ApplyLift()
    {
        Vector3 lift = dragCoefficient * areaFront * airDensity * rb.velocity.sqrMagnitude * transform.up / 2;
        resultingForce += lift;
    }
}
