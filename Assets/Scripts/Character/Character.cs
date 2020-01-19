using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Character : MonoBehaviour
{
    [NonSerialized]
    public Stats Stats;
    [NonSerialized]
    public Ability[] Abilities;
    
    [SerializeField]
    [Tooltip("Abilities prefabs that the character will have")]
    private Ability[] abilitiesPrefab;

    private void Awake()
    {
        Stats = GetComponent<Stats>();
        
        Abilities = new Ability[abilitiesPrefab.Length];
        for (var i = 0; i < abilitiesPrefab.Length; i++)
        {
            Abilities[i] = Instantiate(abilitiesPrefab[i], transform);
        }
    }

    /**
     * It is called when the round ends.
     * Reduce the duration of the status effects.
     */
    public void OnRoundEnd()
    {
        ReduceDurationStatusEffects();
        ReduceCooldownAbilities();
    }

    /**
     * Reduces the duration by one of the status effects. If the status effect has a duration of zero then it is
     * removed.
     */
    private void ReduceDurationStatusEffects()
    {
        var toBeRemoved = new List<StatusEffect>();
        Stats.StatusEffects.ForEach(se =>
        {
            se.Duration--;
            if (se.Duration <= 0) toBeRemoved.Add(se);
        });
        toBeRemoved.ForEach(se => Stats.RemoveStatusEffect(se));
    }

    private void ReduceCooldownAbilities()
    {
        foreach (var ability in Abilities)
        {
            if (ability.CurrentCooldown > 0) ability.CurrentCooldown--;
        }
    }
}