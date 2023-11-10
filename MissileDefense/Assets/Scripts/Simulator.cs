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

    private GameObject missile;
    private TestMissile missileControl;
    public GameObject missilePrefab;

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
        missile = Instantiate(missilePrefab, staticMissileFired.transform.position, staticMissileFired.transform.rotation);
        missileControl = missile.GetComponent<TestMissile>();
        missileControl.Launch();
        Destroy(staticMissileFired);
    }
}
