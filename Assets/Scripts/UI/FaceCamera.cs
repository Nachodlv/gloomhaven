using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera cameraToFace;

    private void Update()
    {
        transform.LookAt(cameraToFace.transform);
    }
}
