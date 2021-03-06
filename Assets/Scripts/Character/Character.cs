﻿using System;
using Abilities;
using UnityEngine;

[RequireComponent(typeof(CharacterStats), typeof(Animator))]
public class Character : MonoBehaviour
{
    public event Action<Character> OnDie;
    
    [NonSerialized]
    public CharacterStats CharacterStats;
    [NonSerialized]
    public Ability[] Abilities;
    
    [SerializeField]
    [Tooltip("Abilities prefabs that the character will have")]
    private Ability[] abilitiesPrefab;

    private Animator animator;
    private static readonly int DieAnimation = Animator.StringToHash("die");

    private void Awake()
    {
        CharacterStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
        CharacterStats.OnStatsChange += OnStatsChange;
        
        Abilities = new Ability[abilitiesPrefab.Length];
        for (var i = 0; i < abilitiesPrefab.Length; i++)
        {
            Abilities[i] = Instantiate(abilitiesPrefab[i], transform);
        }
    }

    /// <summary>
    /// <para>Calls the method StepOnSquare with the currentSquare.</para>
    /// <para>Applies the status effects that currently has the character.</para>
    /// <para>Reduces the duration of the status effects.</para>
    /// <para>Reduces the cooldown of the character abilities.</para>
    /// </summary>
    /// <remarks>This method is called when the turn of the character starts</remarks>
    /// <param name="currentSquare"></param>
    public void OnTurnStart(Square currentSquare)
    {
        StepOnSquare(currentSquare);
        foreach (var statusEffect in CharacterStats.StatusEffects)
        {
            ApplyStatusEffect(statusEffect);
        }
        CharacterStats.ReduceDurationStatusEffects();
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
        OnDie?.Invoke(this);
        animator.SetBool(DieAnimation, true);
        var components = GetComponents<MonoBehaviour>(); //TODO should be changed
        foreach (var component in components)
        {
            component.enabled = false;
        }
    }

    /// <summary>
    /// <para>If the square has any status effect then it is applied to the character</para>
    /// </summary>
    /// <remarks>This method is called when the character is moving and it reaches a square</remarks>
    /// <param name="square">The square reached while the character is moving</param>
    public void StepOnSquare(Square square)
    {
        foreach (var squareStatusEffect in square.StatusEffects)
        {
            CharacterStats.AddStatusEffect(squareStatusEffect);
        }
        
    }

    private void ReduceCooldownAbilities()
    {
        foreach (var ability in Abilities)
        {
            if (ability.CurrentCooldown > 0) ability.CurrentCooldown--;
        }
    }

    private void ApplyStatusEffect(StatusEffect statusEffect)
    {
        if(statusEffect.IsPermanent) CharacterStats.ModifyStats(statusEffect.StatsModifier);
    }

    private void OnStatsChange()
    {
        if (CharacterStats.Health > 0) return;
        Die();
    }
}