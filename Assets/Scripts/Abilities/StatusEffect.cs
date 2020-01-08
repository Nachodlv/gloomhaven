using UnityEngine;

namespace Abilities
{
    public class StatusEffect
    {
        [Tooltip("Stats that the status effect modify")]
        public StatsModifier StatsModifier;
        public int Duration;
    }
}
