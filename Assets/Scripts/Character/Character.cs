using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using UnityEngine;

[RequireComponent(typeof(Stats), typeof(Animator))]
public class Character : MonoBehaviour
{
    [NonSerialized]
    public Stats Stats;
    [NonSerialized]
    public Ability[] Abilities;
    
    [SerializeField]
    [Tooltip("Abilities prefabs that the character will have")]
    private Ability[] abilitiesPrefab;

    private Animator animator;
    private static readonly int DieAnimation = Animator.StringToHash("die");

    private void Awake()
    {
        Stats = GetComponent<Stats>();
        animator = GetComponent<Animator>();
        
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

    /// <summary>
    /// <para>
    /// Plays the dying animation of the character animator.
    /// Disables all the components from the GameObject.
    /// </para>.
    /// </summary>
    /// <remarks>This method is invoked when the character health is reduced to zero</remarks>
    public void Die()
    { 
        animator.SetBool(DieAnimation, true);
        var components = GetComponents<MonoBehaviour>();
        foreach (var component in components)
        {
            component.enabled = false;
        }
    }
    
    /// <summary>
    /// <para>Reduces the duration by one of the status effects. If the status effect has a duration of zero then it is
    /// removed.</para>
    /// </summary>
    private void ReduceDurationStatusEffects()
    {
        // TODO to much GC
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