using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    public GameObject explosionPrefab;

    [Header("Turret Transforms")]
    public Transform body;
    public Transform guns;

    [Header("Turret Attributes")]
    public float gunRange = 50f;
    public float fireRate = 0.2f;

    float fireTimer;

    List<GameObject> targets = new();

    public virtual void AddTarget(GameObject target)
    {
        targets.Add(target);
    }

    public virtual void OnTargetDetected(GameObject target)
    {
        Vector3 targetPosition = target.transform.position;
        Vector3 missileVector = targetPosition - transform.position;

        float bodyOffset = -Vector2.SignedAngle(new(body.forward.x, body.forward.z), new(missileVector.x, missileVector.z));
        body.rotation *= Quaternion.Euler(0, bodyOffset, 0);

        float distance = Vector3.Distance(transform.position, targetPosition);
        float gunsAngle = (360 - guns.eulerAngles.x) % 360;
        float missileAngle = Mathf.Asin((targetPosition.y - transform.position.y) / distance) * (180 / Mathf.PI);
        float gunsOffset = (float)Math.Round(gunsAngle, 3) - (float)Math.Round(missileAngle, 3);
        guns.rotation *= Quaternion.Euler(gunsOffset, 0, 0);

        OnTurretTrigger(target);
    }

    public virtual void OnTargetHit(GameObject target)
    {
        var explosion = Instantiate(explosionPrefab, target.transform.position, Quaternion.identity);
        explosion.GetComponentInChildren<ParticleSystem>().Play();

        Destroy(target);
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer > fireRate)
        {
            if (targets.Count > 0)
            {
                fireTimer = 0f;
                OnTargetDetected(targets[0]);
                targets.RemoveAt(0);
            }
        }
    }

    public abstract void OnTurretTrigger(GameObject target);
}
