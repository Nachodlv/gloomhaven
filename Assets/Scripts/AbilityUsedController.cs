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

    public void AbilityUsed(Ability ability, Square destination)
    {
        var squaresAffected = new List<Square>(ability.AreaOfEffect.Count);
        foreach (var square in ability.AreaOfEffect)
        {
            squaresAffected.Add(board.GetSquare(square.x + destination.Point.x, square.y + destination.Point.y));
        }

        foreach (var boardCharacter in board.Characters)
        {
            if (squaresAffected.Contains(boardCharacter.Value))
            {
                boardCharacter.Key.Stats.ModifyStats(ability.StatsModifier);
            }
        }
    }
}