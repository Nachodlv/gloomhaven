using System;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public Action<GameObject> onMouseDown;

    private void OnMouseDown()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) onMouseDown(gameObject);
    }
}