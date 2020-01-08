using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public uint Initiative => Sum(initiative, statusEffects.Sum(se => se.statsModifier.initiative));
    public uint Defence => Sum(defence, statusEffects.Sum(se => se.statsModifier.defence));
    public uint Health => Sum(health, statusEffects.Sum(se => se.statsModifier.health));
    public uint Sp => Sum(sp, statusEffects.Sum(se => se.statsModifier.sp));
    public uint Speed => Sum(speed, statusEffects.Sum(se => se.statsModifier.speed));
    public uint MaxHealth => maxHealth;

    [SerializeField] private uint initiative = 1;
    [SerializeField] private uint defence = 1;
    [SerializeField] private uint health = 10;
    [SerializeField] private uint sp = 5;
    [SerializeField] private uint speed = 5;

    private uint maxHealth;
    private List<StatusEffect> statusEffects;

    private void Awake()
    {
        maxHealth = health;
        statusEffects = new List<StatusEffect>();
    }

    /**
     * Affects the stats depending on the stats passed as parameter
     */
    public void ModifyStats(StatsModifier stats)
    {
        initiative = Sum(initiative, stats.initiative);
        defence = Sum(defence, stats.defence);
        sp = Sum(sp, stats.sp);
        speed = Sum(speed, stats.speed);
        
        if (stats.health < 0 && stats.health < -defence) 
            health = Sum(health + defence, stats.health);
        else if (stats.health > 0)
            health = Sum(health, stats.health);
    }

    /**
     * Adds a new status effect to the statusEffects list
     */
    public void AddStatusEffect(StatusEffect statusEffect)
    {
        statusEffects.Add(statusEffect);
    }
    
    /**
     * Sum two values.
     * If the results is positive, the return value is the sum of the two values.
     * If the results is negative, the return value is zero.
     */
    private static uint Sum(uint stat, long modifier)
    {
        return (uint) ((int) stat + modifier);
    }
}