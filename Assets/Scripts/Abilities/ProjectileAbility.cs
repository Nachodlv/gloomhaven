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

    private void Awake()
    {
        AreaOfEffect = BoardCalculator.CalculateRange(new Point(0, 0), rangeOfAreaOfEffect)
            .Select(p => new Vector2Int(p.X, p.Y)).ToList();
    }

    public override void CastAbility(Vector3 origin, Vector3 destination)
    {
        var projectile = Instantiate(prefab, origin, Quaternion.identity);
        projectile.Activate(null);
        var middlePoint = (origin + destination) / 2;
        middlePoint.y += 5;
        StartCoroutine(MoveToDestination(projectile, middlePoint,
            () => StartCoroutine(MoveToDestination(projectile, destination, () => ArriveAtDestination(projectile)))));
    }

    private void ArriveAtDestination(Pooleable projectile)
    {
        projectile.Deactivate();
        Destroy(projectile);
    }

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