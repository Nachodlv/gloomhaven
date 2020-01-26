using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Abilities;
using UnityEngine;

public class AbilityUsedController
{
    private readonly Board board;
    private readonly List<Square> squaresWithStatusEffects;

    public AbilityUsedController(Board board)
    {
        this.board = board;
        squaresWithStatusEffects = new List<Square>();
    }

    /// <summary>
    /// <para>If the ability's area of effect hits a character then it modifies the stats of this character by the
    /// StatsModifier of the ability. It also adds the status effects of the ability if it has any</para>
    /// <para>If the ability has status effects then it adds them to the squares hit by the ability</para>
    /// </summary>
    /// <param name="ability">The ability casted</param>
    /// <param name="destination">The destination where the ability was casted</param>
    public void AbilityUsed(Ability ability, Square destination)
    {
        foreach (var square in ability.AreaOfEffect)
        {
            var squareAffected = board.GetSquare(square.x + destination.Point.x, square.y + destination.Point.y);
            foreach (var boardCharacter in board.Characters)
            {
                if (boardCharacter.Value != squareAffected) continue;

                boardCharacter.Key.CharacterStats.ModifyStats(ability.StatsModifier);

                boardCharacter.Key.CharacterStats.AddStatusEffects(ability.statusEffects);
            }

            if (ability.statusEffects.Length <= 0) continue;

            if (squareAffected.StatusEffects.Count == 0)
            {
                squaresWithStatusEffects.Add(squareAffected);
            }
            squareAffected.AddStatusEffects(ability.statusEffects);
        }
    }

    /// <summary>
    /// Reduces the duration by one of the status effects on the squares. If any of the status effects reach duration
    /// zero then it is removed.
    /// </summary>
    public void OnRoundEnd()
    {
        for (var i = 0; i < squaresWithStatusEffects.Count; i++)
        {
            var square = squaresWithStatusEffects[i];
            for (var j = 0; j < square.StatusEffects.Count; j++)
            {
                var statusEffect = square.StatusEffects[j];
                if (statusEffect.DurationLeft > 1)
                {
                    square.StatusEffects[j] = StatusEffect.ReduceDurationStatusEffect(statusEffect);
                }
                else
                {
                    square.RemoveStatusEffectAt(j);
                    j--;
                }
            }

            if (square.StatusEffects.Count != 0) continue;
            squaresWithStatusEffects.RemoveAt(i);
            i--;
        }
    }

   
}