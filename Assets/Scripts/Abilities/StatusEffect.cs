using System;
using UnityEngine;

namespace Abilities
{
    [Serializable]
    public struct StatusEffect
    {
        [SerializeField] [Tooltip("Stats that the status effect modify")]
        private Stats statsModifier;

        [SerializeField] private int duration;
        [SerializeField] private Color color;
        [SerializeField] private bool isPermanent;
        private int durationLeft;

        public Stats StatsModifier => statsModifier;
        public int Duration => duration;
        public Color Color => color;
        public bool IsPermanent => isPermanent;
        
        public int DurationLeft => durationLeft;

        public StatusEffect(Stats statsModifier, int duration, bool isPermanent) : this()
        {
            this.statsModifier = statsModifier;
            this.duration = duration;
            this.isPermanent = isPermanent;
            durationLeft = duration;
            color = Color.clear;
        }

        public static StatusEffect ReduceDurationStatusEffect(StatusEffect statusEffect)
        {
            statusEffect.durationLeft -= 1;
            return statusEffect;
        }

        public static StatusEffect ResetDurationLeft(StatusEffect statusEffect)
        {
            statusEffect.durationLeft = statusEffect.duration;
            return statusEffect;
        }
    }
}