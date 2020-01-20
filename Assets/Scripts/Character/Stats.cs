using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviour
{
    public event UnityAction OnStatsChange;

    public uint Initiative => Sum(initiative, StatusEffects.Sum(se => se.StatsModifier.initiative));
    public uint Defence => Sum(defence, StatusEffects.Sum(se => se.StatsModifier.defence));
    public uint Health => Sum(health, StatusEffects.Sum(se => se.StatsModifier.health));
    public uint Mana => Sum(mana, StatusEffects.Sum(se => se.StatsModifier.mana));
    public uint Speed => Sum(speed, StatusEffects.Sum(se => se.StatsModifier.speed));
    public List<StatusEffect> StatusEffects { get; private set; }

    [SerializeField] private uint initiative = 1;
    [SerializeField] private uint defence = 1;
    [SerializeField] private uint health = 10;
    [SerializeField] private uint mana = 5;
    [SerializeField] private uint speed = 5;


    private void Awake()
    {
        StatusEffects = new List<StatusEffect>();
    }

    /// <summary>
    /// <para> Affects the stats depending on the stats passed as parameter.</para>
    /// <para>Invokes the OnStatsChange event.</para>
    /// </summary>
    /// <param name="stats"></param>
    public void ModifyStats(StatsModifier stats)
    {
        initiative = Sum(initiative, stats.initiative);
        defence = Sum(defence, stats.defence);
        mana = Sum(mana, stats.mana);
        speed = Sum(speed, stats.speed);

        if (stats.health < 0 && stats.health < -defence)
            health = Sum(health + defence, stats.health);
        else if (stats.health > 0)
            health = Sum(health, stats.health);
        
        OnStatsChange?.Invoke();
    }

    /// <summary>
    /// <para>Adds a new status effect to the statusEffects list</para>
    /// <para>Invokes the OnStatsChange event</para>
    /// </summary>
    /// <param name="statusEffect">Status effect to be added</param>
    public void AddStatusEffect(StatusEffect statusEffect)
    {
        StatusEffects.Add(statusEffect);
        OnStatsChange?.Invoke();
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        StatusEffects.Remove(statusEffect);
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