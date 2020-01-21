using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class MeleeAbility : Ability
{
    private Animator animator;
    private static readonly int Attack = Animator.StringToHash("attack");
    private bool castingAbility;
    private Action finishCasting;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInParent<Animator>();
        AreaOfEffect = new List<Vector2Int>{Vector2Int.zero};
    }

    protected override void GetProjectile()
    {
        // no projectiles for melee abilities
    }

    private void Update()
    {
        if (!castingAbility) return;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0)) 
            return;
        finishCasting?.Invoke();
        castingAbility = false;
    }

    public override void CastAbility(Vector3 origin, Vector3 destination, Action onFinishCasting)
    {
        base.CastAbility(origin, destination, onFinishCasting);
        animator.SetTrigger(Attack);
        castingAbility = true;
        finishCasting = onFinishCasting;

        var targetPosition = new Vector3( destination.x, 
            origin.y, 
            destination.z ) ;
        transform.parent.LookAt( targetPosition ) ;
    }

}
