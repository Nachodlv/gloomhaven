using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    public event UnityAction OnStatsChange;

    public int Initiative => SumStats().Initiative;
    public int Defence => SumStats().Defence;
    public int Health => SumStats().Health;
    public int Mana => SumStats().Mana;
    public int Speed => SumStats().Speed;
    public List<StatusEffect> StatusEffects { get; private set; }

    [SerializeField][Tooltip("Stats of the character")] private Stats stats;

    private void Awake()
    {
        StatusEffects = new List<StatusEffect>();
    }

    /// <summary>
    /// <para> Affects the stats depending on the stats passed as parameter.</para>
    /// <para>Invokes the OnStatsChange event.</para>
    /// </summary>
    /// <param name="modifierStats"></param>
    public void ModifyStats(Stats modifierStats)
    {
        stats = Stats.SumStats(stats, modifierStats);
        OnStatsChange?.Invoke();
    }

    /// <summary>
    /// <para>Adds a new status effect to the statusEffects list.</para>
    /// <para>If the status effect is permanent then the stats are modify.</para>
    /// <para>Invokes the OnStatsChange event if updateUi is true.</para>
    /// </summary>
    /// <param name="statusEffect">Status effect to be added</param>
    /// <param name="updateUi">If the OnStatsChange event should be invoke or not.</param>
    public void AddStatusEffect(StatusEffect statusEffect, bool updateUi = true)
    {
        StatusEffects.Add(StatusEffect.ResetDurationLeft(statusEffect));
        if (statusEffect.IsPermanent) ModifyStats(statusEffect.StatsModifier);
        if (updateUi) OnStatsChange?.Invoke();
    }

    /// <summary>
    /// <para>
    /// Add multiple StatusEffects to the statusEffects list. It uses the AddStatusEffect method.
    /// </para>
    /// <para>Invokes the OnStatsChange event</para>
    /// </summary>
    /// <param name="statusEffects">The status effects that will be added</param>
    public void AddStatusEffects(IEnumerable<StatusEffect> statusEffects)
    {
        var enumerator = statusEffects.GetEnumerator();
        while (enumerator.MoveNext())
        {
            AddStatusEffect(enumerator.Current, false);
        }

        enumerator.Dispose();
        OnStatsChange?.Invoke();
    }

    /// <summary>
    /// <para>
    /// Removes a StatusEffect from the statusEffects list at the index passed as parameter.
    /// </para>
    /// </summary>
    /// <param name="index"></param>
    public void RemoveStatusEffectAt(int index)
    {
        StatusEffects.RemoveAt(index);
    }


    private Stats SumStats()
    {
        var originalStats = stats;
        foreach (var statusEffect in StatusEffects)
        {
            if (!statusEffect.IsPermanent)
                originalStats = Stats.SumStats(originalStats, statusEffect.StatsModifier);
        }

        return originalStats;
    }
}