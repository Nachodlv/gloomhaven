using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Character : MonoBehaviour
{
    public Stats stats;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    /**
     * It is called when the round ends.
     * Reduce the duration of the status effects.
     */
    public void OnRoundEnd()
    {
        ReduceDurationStatusEffects();
    }

    /**
     * Reduces the duration by one of the status effects. If the status effect has a duration of zero then it is
     * removed.
     */
    private void ReduceDurationStatusEffects()
    {
        var toBeRemoved = new List<StatusEffect>();
        stats.StatusEffects.ForEach(se =>
        {
            se.Duration--;
            if (se.Duration <= 0) toBeRemoved.Add(se);
        });
        toBeRemoved.ForEach(se => stats.RemoveStatusEffect(se));
    }
}