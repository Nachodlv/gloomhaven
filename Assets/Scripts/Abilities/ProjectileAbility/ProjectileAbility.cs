using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

[Serializable]
public class ProjectileAbility : Ability
{
    [SerializeField] [Tooltip("Speed of the projectile")]
    private float speed;

    [SerializeField] [Tooltip("Squares that the ability will affect around the destination")]
    private int rangeOfAreaOfEffect;

    private List<Vector3> targets;
    private Pooleable projectile;
    private bool hasTargets;
    private Action finishCasting;

    protected override void Awake()
    {
        base.Awake();
        AreaOfEffect = BoardCalculator.CalculateRange(new Vector2Int(0, 0), rangeOfAreaOfEffect)
            .Select(p => new Vector2Int(p.x, p.y)).ToList();
        targets = new List<Vector3>();
    }

    private void Update()
    {
        if (!hasTargets || targets.Count == 0) return;
        if (Vector3.Distance(projectile.transform.position, targets[0]) < 0.01f)
        {
            targets.RemoveAt(0);
            if (targets.Count == 0)
            {
                FinishMoving();
                return;
            }
        }

        projectile.transform.position =
            Vector3.MoveTowards(projectile.transform.position, targets[0], speed * Time.deltaTime);
    }

    /// <summary>
    ///     <para> Cast the ability in the given origin directed to its given destination </para>
    /// </summary>
    /// <param name="origin">The origin of the ability</param>
    /// <param name="destination">The destination of the ability</param>
    /// <param name="onFinishCasting">The method that will be invoked when the ability reaches its destination</param>
    public override void CastAbility(Vector3 origin, Vector3 destination, Action onFinishCasting)
    {
        base.CastAbility(origin, destination, onFinishCasting);
        projectile = ObjectPooler.GetNextObject();
        projectile.transform.position = origin;
        var middlePoint = (origin + destination) / 2;
        middlePoint.y += 5;

        hasTargets = true;
        targets.Add(middlePoint);
        targets.Add(destination);
        finishCasting = onFinishCasting;
    }
    
    private void FinishMoving()
    {
        hasTargets = false;
        projectile.Deactivate();
        finishCasting();
    }
}