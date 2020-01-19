using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class FireballProjectile : Pooleable
{
    [SerializeField] private ParticleSystem projectile;
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private ParticleSystem explosion;

    private bool needsToDeactivate;
    private float timeToDeactivate;
    
    private void Update()
    {
        if (!needsToDeactivate) return;
        timeToDeactivate -= Time.deltaTime;
        if (timeToDeactivate <= 0)
        {
            base.Deactivate();
            needsToDeactivate = false;
        }
    }

    /// <summary>
    /// Invokes the base Activate method and plays the projectile and trail ParticleSystem
    /// </summary>
    /// <param name="pooleable"></param>
    public override void Activate(ObjectPooler pooleable)
    {
        base.Activate(pooleable);
        projectile.Play();
        trail.Play();
    }

    /// <summary>
    /// <para>
    /// Plays the explosion ParticleSystem.
    /// Stops the projectile and trail ParticleSystem.
    /// After the lifetime of the explosion the base Deactivate method will be Invoke.
    /// </para>
    /// </summary>
    public override void Deactivate()
    {
        projectile.Stop();
        trail.Stop();
        explosion.Play();
        needsToDeactivate = true;
        timeToDeactivate = explosion.main.startLifetime.constant;
    }

}
