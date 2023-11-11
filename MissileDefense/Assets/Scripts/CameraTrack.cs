using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    private bool track = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            track = true;
        }
        if (track)
        {
            GameObject missile = GameObject.FindGameObjectWithTag("Missile");
            if (missile != null)
            {
                transform.LookAt(missile.transform);
            }
        }
    }
}
