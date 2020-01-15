using System;
using UnityEngine;

namespace Abilities
{
    [Serializable]
    public class StatusEffect
    {
        [Tooltip("Stats that the status effect modify")]
        public StatsModifier StatsModifier;
        public int Duration;
    }
}
