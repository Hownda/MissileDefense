using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : Turret
{
    public GameObject interceptorPrefab;

    public List<GameObject> interceptionMissiles = new();

    public bool TargetMode = false;

    public float smoothness = 0.1f;

    [Header("Laser")]
    public LineRenderer laserLine;
    public Transform laserOrigin;
    public float laserDuration = 0.05f;

    public override void OnTurretTrigger(GameObject target)
    {
        LaunchLaser(target);
    }

    private void LaunchLaser(GameObject target)
    {
        Vector3 targetPosition = target.transform.position;

        laserLine.SetPosition(0, laserOrigin.position);
        Vector3 rayOrigin = laserOrigin.position;
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, targetPosition - guns.position, out hit, gunRange))
        {
            laserLine.SetPosition(1, hit.point);
            OnTargetHit(target);
        }
        else
        {
            laserLine.SetPosition(1, rayOrigin + ((targetPosition - guns.position) * gunRange));
        }
        StartCoroutine(ShootLaser());
    }

    IEnumerator ShootLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }

    public void LaunchInterceptor(GameObject target)
    {
        GameObject staticInterceptorFired = interceptionMissiles[0];
        interceptionMissiles.Remove(staticInterceptorFired);
        GameObject missile = Instantiate(interceptorPrefab, staticInterceptorFired.transform.position, staticInterceptorFired.transform.rotation);
        InterceptMissile missileControl = missile.GetComponent<InterceptMissile>();
        missileControl.Launch(target);
        Destroy(staticInterceptorFired);
        Debug.Break();
    }
}
