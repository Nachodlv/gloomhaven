using System;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public Action<GameObject> onMouseDown;

    private void OnMouseDown()
    {
        onMouseDown(gameObject);
    }
}