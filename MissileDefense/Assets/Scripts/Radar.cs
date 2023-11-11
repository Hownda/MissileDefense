using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public Interceptor interceptor;

    public float radius = 200f;

    private Dictionary<int, MissileInformation> trackedMissiles = new();

    private class MissileInformation
    {
        bool readyForInterception = false;
        Vector3 position;
        Vector3 forward;
        List<Vector3> lastForwards = new();

        public MissileInformation(Vector3 position)
        {
            this.position = position;
            forward = Vector3.zero;
        }

        public Vector3 UpdateInformation(Vector3 newPosition)
        {
            forward = newPosition - position;
            position = newPosition;

            if (lastForwards.Count != 0)
            {
                Vector3 sum = new();
                foreach (var pos in lastForwards)
                {
                    sum += pos;
                }
                forward = (forward + sum) / (lastForwards.Count + 1);
            }
            if (lastForwards.Count > 10)
            {
                lastForwards.RemoveAt(0);
                readyForInterception = true;
            }

            lastForwards.Add(forward);

            return forward;
        }

        public bool ReadyForInterception()
        {
            return readyForInterception;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void FixedUpdate()
    {
        var missiles = GameObject.FindGameObjectsWithTag("Missile");

        foreach (var m in missiles)
        {
            if (Vector3.Distance(transform.position, m.transform.position) <= radius)
            {
                AddOrUpdateMissile(m);
            }
        }
    }

    private void AddOrUpdateMissile(GameObject missile)
    {
        int id = missile.GetComponent<TestMissile>().missileID;
        Vector3 missilePosition = missile.transform.position;

        if (trackedMissiles.ContainsKey(id))
        {
            MissileInformation missileInformation = trackedMissiles[id];
            if (!missileInformation.ReadyForInterception())
            {
                Vector3 direction = missileInformation.UpdateInformation(missilePosition);

                if (missileInformation.ReadyForInterception())
                {
                    interceptor.OnTargetDetected(missile.transform.position += 15 * direction);
                }
            }
        }
        else
        {
            trackedMissiles.Add(id, new MissileInformation(missilePosition));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            interceptor.testMissile.Launch();
        }
    }
}
