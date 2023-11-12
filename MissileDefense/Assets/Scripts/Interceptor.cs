using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Interceptor : MonoBehaviour
{
    public GameObject interceptorPrefab;

    public List<GameObject> interceptionMissiles = new();

    public Transform body;
    public Transform guns;

    public bool TargetMode = false;

    public float smoothness = 0.1f;

    public void OnTargetDetected(Vector3 targetPosition)
    {
        Vector3 missileVector = targetPosition - transform.position;

        float bodyOffset = -Vector2.SignedAngle(new(body.forward.x, body.forward.z), new(missileVector.x, missileVector.z));
        body.rotation *= Quaternion.Euler(0, bodyOffset, 0);

        float distance = Vector3.Distance(transform.position, targetPosition);
        float gunsAngle = (360 - guns.eulerAngles.x) % 360;
        float missileAngle = Mathf.Asin((targetPosition.y - transform.position.y) / distance) * (180 / Mathf.PI);
        float gunsOffset = (float)Math.Round(gunsAngle, 3) - (float)Math.Round(missileAngle, 3);
        guns.rotation *= Quaternion.Euler(gunsOffset, 0, 0);

        LaunchInterceptor();
    }

    public void LaunchInterceptor()
    {
        GameObject staticInterceptorFired = interceptionMissiles[0];
        interceptionMissiles.Remove(staticInterceptorFired);
        GameObject missile = Instantiate(interceptorPrefab, staticInterceptorFired.transform.position, staticInterceptorFired.transform.rotation);
        TestMissile missileControl = missile.GetComponent<TestMissile>();
        missileControl.Launch();
        Destroy(staticInterceptorFired);
    }
}
