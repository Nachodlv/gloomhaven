using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class AbilityUsedController
{
    private Board board;

    public AbilityUsedController(Board board)
    {
        this.board = board;
    }

    /// <summary>
    /// <para>If the ability's area of effect hits a character then it modifies the stats of this character by the
    /// StatsModifier of the ability. If the character health is reduced to zero then it invokes the OnCharacterDead
    /// method from the TurnManager</para>
    /// </summary>
    /// <param name="turnManager"></param>
    /// <param name="ability">The ability casted</param>
    /// <param name="destination">The destination where the ability was casted</param>
    public void AbilityUsed(TurnManager turnManager, Ability ability, Square destination)
    {
        var squaresAffected = new List<Square>(ability.AreaOfEffect.Count);
        foreach (var square in ability.AreaOfEffect)
        {
            squaresAffected.Add(board.GetSquare(square.x + destination.Point.x, square.y + destination.Point.y));
        }

        foreach (var boardCharacter in board.Characters)
        {
            if (!squaresAffected.Contains(boardCharacter.Value)) continue;
            boardCharacter.Key.Stats.ModifyStats(ability.StatsModifier);

            if (boardCharacter.Key.Stats.Health > 0) continue;
            boardCharacter.Key.Die();
            turnManager.OnCharacterDead(boardCharacter.Key);
        }
    }
}