using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            camera1.enabled = !camera1.enabled;
            camera2.enabled = !camera2.enabled;
        }
    }

    public void SwitchToCamera1()
    {
        camera1.enabled = true;
        camera2.enabled = false;
    }
}
