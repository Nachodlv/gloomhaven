using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class FireballProjectile : Pooleable
{
    [SerializeField] private ParticleSystem projectile;
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private ParticleSystem explosion;
    
    public override void Activate(ObjectPooler pooleable)
    {
        base.Activate(pooleable);
        projectile.Play();
        trail.Play();
    }

    public override void Deactivate()
    {
        projectile.Stop();
        trail.Stop();
        explosion.Play();
        CoroutineHelper.WaitForSeconds(this, explosion.main.startLifetime.constant, () => base.Deactivate());
    }

}
