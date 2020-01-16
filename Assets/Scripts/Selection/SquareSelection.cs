using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;

public class SquareSelection
{
    /// <summary>
    /// Moves the player to the selected square. Uses the <paramref name="boardPainter"/> to remove the colors from the
    /// walking range and the walking trail.
    /// </summary>
    /// <param name="boardPainter">Used for removing the walking range and the walking trail</param>
    /// <param name="square">The selected square.</param>
    /// <param name="character">The character that is playing in this turn</param>
    /// <param name="onStopMoving">Function that will be executed when the character stop moving</param>
    public void OnSquareSelected(BoardPainter boardPainter, Square square, Character character, Action onStopMoving)
    {
        RemovesPainting(boardPainter, character);
        var distance =
            boardPainter.board.MoveCharacter(character, GetPath(boardPainter.board, character, square), onStopMoving);
        ReduceSpeed(character, distance);
    }
    
    /// <summary>
    /// Adds color to the squares with the <paramref name="boardPainter"/> representing the walking path that the
    /// character will make if the square is selected.
    /// </summary>
    /// <param name="boardPainter">Used to paint the walking path</param>
    /// <param name="square">Square that is being hovered with the mouse</param>
    /// <param name="character">The character that is playing in this turn</param>
    public void OnSquareHovered(BoardPainter boardPainter, Square square, Character character)
    {
        boardPainter.PaintWalkingSquares(character, GetPath(boardPainter.board, character, square));
    }
    
    /// <summary>
    /// Removes the color to the squares that was added when it was hovered.
    /// </summary>
    /// <param name="boardPainter">Used to remove the color of the squares</param>
    /// <param name="character">The character that is playing in this turn</param>
    public void OnSquareUnHovered(BoardPainter boardPainter, Character character)
    {
        boardPainter.UnPaintWalkingSquares(character);
    }
    
    /// <summary>
    /// Reduces the speed of the <paramref name="character"/> in the <paramref name="quantity"/> provided by one turn
    /// </summary>
    /// <param name="character">The character that will loose speed</param>
    /// <param name="quantity">The quantity in which the speed will be reduce</param>
    private static void ReduceSpeed(Character character, int quantity)
    {
        character.Stats.AddStatusEffect(new StatusEffect
            {Duration = 1, StatsModifier = new StatsModifier {speed = -quantity}});
    }

    /**
     * Uses the board painter to remove the colors from the walking range and from the walking square if it is any.
     */
    private static void RemovesPainting(BoardPainter boardPainter, Character character)
    {
        boardPainter.UnPaintWalkingSquares(character);
        boardPainter.UnPaintWalkingRange(character);

    }
    
    /// <summary>
    /// Gets the path that the <paramref name="character"/> need to do to get to its <paramref name="destination"/>.
    /// The path is limited by the speed of the character
    /// </summary>
    /// <param name="board">The board is used to calculate the path</param>
    /// <param name="character">The character is used to get the speed and the initial square of the path</param>
    /// <param name="destination">The final square of the path</param>
    /// <returns>The path represented as a list of squares</returns>
    private List<Square> GetPath(Board board, Character character, Square destination)
    {
        var speed = (int) character.Stats.Speed;
        if (speed == 0) return new List<Square>();
        var from = board.GetCharacterSquare(character);
        var path = board.GetPath(from, destination);
        if (path.Count > speed) path.RemoveRange(speed + 1, path.Count - 1 - speed);
        return path;
    }
}