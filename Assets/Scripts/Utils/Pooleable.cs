using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooleable : MonoBehaviour
{
    private ObjectPooler objectPooler;

    protected void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates the game object.
    /// </summary>
    /// <param name="pooleable">ObjectPooler that will be used when this GameObject is deactivated</param>
    public virtual void Activate(ObjectPooler pooleable)
    {
        gameObject.SetActive(true);
        objectPooler = pooleable;
    }

    /// <summary>
    /// Deactivated the game object. Tells the object pooler that this game object is deactivated.
    /// </summary>
    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        objectPooler?.PooleableDeactivated(this);
    }
}
