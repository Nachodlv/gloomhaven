using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour
{
    private Quaternion rotation;
    private void Awake()
    {
        rotation = transform.rotation;
    }
    
    /// <summary>
    /// Fix the rotation of the GameObject.
    /// </summary>
    private void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
