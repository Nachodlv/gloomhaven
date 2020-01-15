using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooleable : MonoBehaviour
{
    private ObjectPooler objectPooler;

    protected void Start()
    {
//        gameObject.SetActive(false);
    }

    public virtual void Activate(ObjectPooler pooleable)
    {
        gameObject.SetActive(true);
        objectPooler = pooleable;
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        objectPooler?.PooleableDeactivated(this);
    }
}
