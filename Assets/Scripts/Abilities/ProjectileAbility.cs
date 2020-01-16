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

    protected override void Awake()
    {
        base.Awake();
        AreaOfEffect = BoardCalculator.CalculateRange(new Point(0, 0), rangeOfAreaOfEffect)
            .Select(p => new Vector2Int(p.X, p.Y)).ToList();
    }

    /// <summary>
    ///     <para> Cast the ability in the given origin directed to its given destination </para>
    /// </summary>
    /// <param name="origin">The origin of the ability</param>
    /// <param name="destination">The destination of the ability</param>
    public override void CastAbility(Vector3 origin, Vector3 destination)
    {
        var projectile = objectPooler.GetNextObject();
        projectile.Activate(objectPooler);
        projectile.transform.position = origin;
        var middlePoint = (origin + destination) / 2;
        middlePoint.y += 5;
        StartCoroutine(MoveToDestination(projectile, middlePoint,
            () => StartCoroutine(MoveToDestination(projectile, destination, () => ArriveAtDestination(projectile)))));
    }

    /// <summary>
    ///     <para> Deactivates the projectile</para>
    /// </summary>
    /// <remarks>This method is executed when the <paramref name="projectile"/> reaches his final destination</remarks>
    /// <param name="projectile">The projectile that will be deactivated</param>
    private void ArriveAtDestination(Pooleable projectile)
    {
        projectile.Deactivate();
    }

    /// <summary>
    ///     <para>Coroutine used to move the projectile to its destination</para>
    /// </summary>
    /// <param name="projectile">Pooleable that the method will move</param>
    /// <param name="destination">The target that will be used as destination</param>
    /// <param name="onFinish">The Action that will be executed when the projectile reaches his destination</param>
    /// <returns></returns>
    private IEnumerator MoveToDestination(Pooleable projectile, Vector3 destination, Action onFinish)
    {
        while (Vector3.Distance(projectile.transform.position, destination) > 0.01f)
        {
            projectile.transform.position =
                Vector3.MoveTowards(projectile.transform.position, destination, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        onFinish();
        yield return null;
    }
}