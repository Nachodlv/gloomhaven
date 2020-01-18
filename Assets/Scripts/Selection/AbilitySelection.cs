using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;

public class AbilitySelection
{
    public event Action OnAbilityUsed;
    private Action onAbilityUsed;
    public Ability Ability { get; private set; }

    /// <summary>
    /// Assigns the ability that will be used and paints it range.
    /// </summary>
    /// <param name="ability">Assigned ability</param>
    /// <param name="boardPainter">Used to paint the range of the ability</param>
    /// <param name="character">Used to get the center of the range</param>
    /// <param name="onAbilitySuccessfullyUsed">Function that will be called when the ability is casted</param>
    public void OnAbilitySelected(Ability ability, BoardPainter boardPainter, Character character,
        Action onAbilitySuccessfullyUsed)
    {
        Ability = ability;
        onAbilityUsed = onAbilitySuccessfullyUsed;
        boardPainter.PaintAbilityRange(character, ability.range);
    }

    /// <summary>
    /// Remove the paint from the ability range and the area of effect of the ability.
    /// </summary>
    /// <param name="boardPainter">Used to remove the paint</param>
    /// <param name="character">Character that was previously assigned to this painted squares</param>
    public void UnSelectAbility(BoardPainter boardPainter, Character character)
    {
        RemovePainting(boardPainter, character);
    }


    /// <summary>
    /// Casts the ability assigned from the position of the character to the destination.
    /// It reduces the mana of the character by the cost of the ability.
    /// If the square selected is out of the range of the ability, then the ability is not casted.
    /// </summary>
    /// <param name="boardPainter">Used to remove the painting from the range and area of effect of the ability</param>
    /// <param name="destination">The destination of the ability</param>
    /// <param name="character">The character that is playing in this turn</param>
    /// <param name="onFinishCasting">A function that will be called if the cast of the ability is successful</param>
    public void OnSquareSelected(BoardPainter boardPainter, Square destination, Character character,
        Action onFinishCasting)
    {
        var characterSquare = boardPainter.board.GetCharacterSquare(character);
        if (GetDistance(characterSquare, destination) > Ability.range)
        {
            Debug.Log("Out of range!!");
            return;
        }

        RemovePainting(boardPainter, character);
        Ability.CastAbility(characterSquare.transform.position,
            destination.transform.position);
        ReduceMana(character, Ability.cost);
        onAbilityUsed.Invoke();
        onFinishCasting();
    }

    /// <summary>
    /// Paints the area of effect with its center being the destination
    /// </summary>
    /// <param name="boardPainter">Used to paint the area of effect</param>
    /// <param name="destination">The center of the area of effect</param>
    /// <param name="character">The character that will be assigned to the painted squares</param>
    public void OnSquareHovered(BoardPainter boardPainter, Square destination, Character character)
    {
        boardPainter.PaintAbilityAreaOfEffect(character, destination, Ability.AreaOfEffect);
    }

    /// <summary>
    /// Removes the painting of the area of effect of the ability selected
    /// </summary>
    /// <param name="boardPainter">Used to remove the paint of the area of effect.</param>
    /// <param name="character">The character that was used previously to paint the area of effect of this ability</param>
    public void OnSquareUnHovered(BoardPainter boardPainter, Character character)
    {
        boardPainter.UnPaintAbilityAreaOfEffect(character);
    }

    private void RemovePainting(BoardPainter boardPainter, Character character)
    {
        boardPainter.UnPaintAbilityRange(character, Ability.range);
        boardPainter.UnPaintAbilityAreaOfEffect(character);
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