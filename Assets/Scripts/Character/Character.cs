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
    public List<Ability> Abilities;
    
    [SerializeField]
    [Tooltip("Abilities prefabs that the character will have")]
    private List<Ability> abilitiesPrefab;

    private void Awake()
    {
        Stats = GetComponent<Stats>();
        
        // Instantiates the abilities prefabs
        Abilities = abilitiesPrefab.Select(a => Instantiate(a, transform)).ToList();
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
        Stats.StatusEffects.ForEach(se =>
        {
            se.Duration--;
            if (se.Duration <= 0) toBeRemoved.Add(se);
        });
        toBeRemoved.ForEach(se => Stats.RemoveStatusEffect(se));
    }
}