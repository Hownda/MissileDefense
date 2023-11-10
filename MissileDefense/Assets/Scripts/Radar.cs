using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public float radius = 2000f;

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
