using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Abilities
{
    [Serializable]
    public struct Stats
    {
        [SerializeField] private int initiative;
        [SerializeField] private int defence;
        [SerializeField] private int health;
        [SerializeField] private int mana;
        [SerializeField] private int speed;

        public int Initiative => initiative;
        public int Defence => defence;
        public int Health => health;
        public int Mana => mana;
        public int Speed => speed;


        public Stats(int mana = 0, int speed = 0) : this()
        {
            this.mana = mana;
            this.speed = speed;
        }
        
        /// <summary>
        /// Sums two stats and returns a new stats that is the result of the sum.
        /// </summary>
        /// <param name="originalStats"></param>
        /// <param name="modifierStats"></param>
        /// <returns></returns>
        public static Stats SumStats(Stats originalStats, Stats modifierStats)
        {
            return new Stats()
            {
                initiative = Sum(originalStats.initiative, modifierStats.initiative),
                defence = Sum(originalStats.defence, modifierStats.defence),
                mana = Sum(originalStats.mana, modifierStats.mana),
                speed = Sum(originalStats.speed, modifierStats.speed),
                health = SumHealth(originalStats, modifierStats)
            };
        }

        private static int SumHealth(Stats originalStats, Stats modifierStats)
        {
            if (modifierStats.Health < 0 && modifierStats.Health < -originalStats.defence)
                return Sum(originalStats.health + originalStats.defence, modifierStats.Health);
            return modifierStats.Health > 0 ? Sum(originalStats.health, modifierStats.Health) : originalStats.health;
        }

       
        /// <summary>
        /// <para>
        /// Sum two values.
        /// If the results is positive, the return value is the sum of the two values.
        /// If the results is negative, the return value is zero.
        /// </para>
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        private static int Sum(int stat, long modifier)
        {
            var result = (int) (stat + modifier);
            return result > 0 ? result : 0;
        }
    }
}