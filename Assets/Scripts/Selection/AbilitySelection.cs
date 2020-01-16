using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;

public class AbilitySelection
{
    public Ability Ability { get; private set; }

    /// <summary>
    /// Assigns the <paramref name="ability"/> that will be used
    /// </summary>
    /// <param name="ability">Assigned ability</param>
    public void OnAbilitySelected(Ability ability)
    {
        Ability = ability;
    }

    /// <summary>
    /// Casts the ability assigned from the position of the <paramref name="character"/> to the
    /// <paramref name="destination"/>.
    /// It reduces the mana of the <paramref name="character"/> by its cost.
    /// If the square selected is out of the range of the ability, then it is not casted.
    /// </summary>
    /// <param name="boardPainter">Used to remove the painting from the range and area of effect of the ability</param>
    /// <param name="destination">The destination of the ability</param>
    /// <param name="character">The character that is playing in this turn</param>
    /// <param name="onAbilityUsed">A function that will be called if the cast of the ability is successful</param>
    public void OnSquareSelected(BoardPainter boardPainter, Square destination, Character character,
        Action onAbilityUsed)
    {
        var characterSquare = boardPainter.board.GetCharacterSquare(character);
        if (GetDistance(characterSquare, destination) > Ability.range)
        {
            Debug.Log("Out of range!!");
            return;
        }

        RemovePainting(boardPainter, destination, character);
        Ability.CastAbility(characterSquare.transform.position,
            destination.transform.position);
        ReduceMana(character, Ability.cost);
        onAbilityUsed();
    }

    /// <summary>
    /// Paints the area of effect with its center being the <paramref name="destination"/>
    /// </summary>
    /// <param name="boardPainter">Used to paint the area of effect</param>
    /// <param name="destination">The center of the area of effect</param>
    public void OnSquareHovered(BoardPainter boardPainter, Square destination)
    {
        boardPainter.PaintAbilityAreaOfEffect(destination, Ability.AreaOfEffect);
    }

    /// <summary>
    /// Removes the painting of the area of effect of the ability selected
    /// </summary>
    /// <param name="boardPainter">Used to remove the paint of the area of effect.</param>
    /// <param name="destination">The center of the area of effect</param>
    public void OnSquareUnHovered(BoardPainter boardPainter, Square destination)
    {
        boardPainter.UnPaintAbilityAreaOfEffect(destination, Ability.AreaOfEffect);
    }
    
    private void RemovePainting(BoardPainter boardPainter, Square square, Character character)
    {
        boardPainter.UnPaintAbilityRange(character, Ability.range);
        boardPainter.UnPaintAbilityAreaOfEffect(square, Ability.AreaOfEffect);
    }

    private static void ReduceMana(Character character, int quantity)
    {
        character.Stats.AddStatusEffect(new StatusEffect
        {
            Duration = 1, StatsModifier = new StatsModifier {mana = -quantity}
        });
    }
    
    private int GetDistance(Square origin, Square destination)
    {
        var x = origin.x > destination.x ? origin.x - destination.x : destination.x - origin.x;
        var y = origin.y > destination.y ? origin.y - destination.y : destination.y - origin.y;
        return x + y;
    }
}