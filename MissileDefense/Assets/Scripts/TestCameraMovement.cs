using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraMovement : MonoBehaviour
{
    public Vector3 cameraOffset;
    public Transform missile;

    void Update()
    {
            transform.position = cameraOffset + missile.position;
    }
}
