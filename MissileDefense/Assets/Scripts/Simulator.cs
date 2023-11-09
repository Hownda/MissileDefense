using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public GameObject missileLauncher;
    public List<GameObject> staticMissiles = new();

    public GameObject interceptor;
    public List<GameObject> staticInterceptors = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LaunchMissile();
        }
    }

    private void LaunchMissile()
    {
        // To do: Implement physics

        GameObject staticMissileFired = staticMissiles[0];
        staticMissiles.Remove(staticMissileFired);
        Destroy(staticMissileFired);
    }
}
