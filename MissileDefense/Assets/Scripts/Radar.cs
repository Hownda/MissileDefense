using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public float radius = 200f;

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
                Debug.Log($"Missile detected! Current Position {m.transform.position}");
            }
        }
    }
}
