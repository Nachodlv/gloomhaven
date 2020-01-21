using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] [Tooltip("Camera to move")]
    private Camera myCamera;

    [Header("Camera movement")] [SerializeField] [Tooltip("Camera movement speed")]
    private int speed = 5;
    [NonSerialized]public Vector3 MinPoint;
    [NonSerialized] public Vector3 MaxPoint;
    
    [SerializeField]
    [Tooltip(
        "The camera will move when there are less pixels between the mouse and the border of the screen than edgeSize")]
    private float edgeSize = 20;

    [Header("Camera zoom")] [SerializeField]
    private float maxFieldOfView = 90;

    [SerializeField] private float minFieldOfView = 20;

    [SerializeField] [Tooltip("Speed at which the camera will zoom in and zoom out")]
    private float scrollingSpeed = 20;

    [Header("Keys to move the camera")] [SerializeField] [Tooltip("Key to move up")]
    private KeyCode moveUp = KeyCode.W;

    [SerializeField] [Tooltip("Key to move down")]
    private KeyCode moveDown = KeyCode.S;

    [SerializeField] [Tooltip("Key to move left")]
    private KeyCode moveLeft = KeyCode.A;

    [SerializeField] [Tooltip("Key to move right")]
    private KeyCode moveRight = KeyCode.D;

    private GameObject cameraFollow;
    private Vector3 target;
    private bool moveToTarget;

    private void Awake()
    {
        cameraFollow = new GameObject();
        target = Vector3.zero;
        
        var cameraTransform = myCamera.transform;
        cameraFollow.transform.position = cameraTransform.position;
        cameraTransform.parent = cameraFollow.transform;
        
        var cameraPosition = Vector3.zero;
        cameraPosition.x = 9f;
        cameraTransform.localPosition = cameraPosition;
    }


    private void Update()
    {
        if (moveToTarget)
        {
            var position = Vector3.MoveTowards(cameraFollow.transform.position, target, speed * Time.deltaTime);
            if (Vector3.Distance(position, target) < 0.01f) moveToTarget = false;
            cameraFollow.transform.position = position;
            return;
        }

        TranslateCamera();
        ZoomInOrOut();
    }

    public void MoveCameraToLocation(Vector3 destination)
    {
        moveToTarget = true;
        target.x = destination.x;
        target.y = cameraFollow.transform.position.y;
        target.z = destination.z;
    }

    /// <summary>
    /// <para>
    /// Moves the camera up if the moveUp key is pressed or the mouse is at the top of the screen.
    /// Moves the camera left if the moveLeft key is pressed or the mouse is at the left border of the screen.
    /// Moves the camera down if the moveDown key is pressed or the mouse is at the bottom of the screen.
    /// Moves the camera right if the moveRight key is pressed or the mouse is at the right border of the screen.
    /// </para>
    /// </summary>
    private void TranslateCamera()
    {
        var position = cameraFollow.transform.position;
        var movement = speed * Time.deltaTime;

        if (Input.GetKey(moveUp)) position.x -= movement;
        if (Input.GetKey(moveDown)) position.x += movement;
        if (Input.GetKey(moveRight)) position.z += movement;
        if (Input.GetKey(moveLeft)) position.z -= movement;

        if (Input.mousePosition.y > Screen.height - edgeSize) position.x -= movement;
        if (Input.mousePosition.y < edgeSize) position.x += movement;
        if (Input.mousePosition.x > Screen.width - edgeSize) position.z += movement;
        if (Input.mousePosition.x < edgeSize) position.z -= movement;

        position.x = Math.Min(Math.Max(position.x, MinPoint.x), MaxPoint.x);
        position.z = Math.Min(Math.Max(position.z, MinPoint.z), MaxPoint.z);
        cameraFollow.transform.position = position;
    }

    /// <summary>
    /// <para>
    /// If it detects a mouse scroll the it makes bigger or smaller the field of view.
    /// </para>
    /// </summary>
    private void ZoomInOrOut()
    {
        if (Input.mouseScrollDelta.y > 0 && myCamera.fieldOfView < maxFieldOfView)
            myCamera.fieldOfView -= Time.deltaTime * scrollingSpeed;
        if (Input.mouseScrollDelta.y < 0 && myCamera.fieldOfView > minFieldOfView)
            myCamera.fieldOfView += Time.deltaTime * scrollingSpeed;
    }
}