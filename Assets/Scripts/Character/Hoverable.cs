using System;
using UnityEngine;

public class Hoverable : MonoBehaviour
{
    public Action<GameObject> onMouseEnter;
    public Action<GameObject> onMouseExit;

    private void OnMouseEnter()
    {
        onMouseEnter(gameObject);
    }

    private void OnMouseExit()
    {
        onMouseExit(gameObject);
    }
}