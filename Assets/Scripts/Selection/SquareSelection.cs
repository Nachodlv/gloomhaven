using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;
using Utils;

public class SquareSelection
{
    /// <summary>
    /// Paints the walking range of the character.
    /// </summary>
    /// <param name="boardPainter">Used to paint the walking range</param>
    /// <param name="character">The character used to get the walking range</param>
    public static void StartWalking(BoardPainter boardPainter, Character character)
    {
        boardPainter.PaintWalkingRange(character);
    }

    /// <summary>
    /// Removes the paint of the walking range and the walking squares if there was any.
    /// </summary>
    /// <param name="boardPainter">Used to remove the paint.</param>
    /// <param name="character">The character used to get the walking range and the walking squares</param>
    public static void StopWalking(BoardPainter boardPainter, Character character)
    {
        RemovesPainting(boardPainter, character);
    }

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
        var path = GetPath(boardPainter.board, character, square);
        var distance =
            boardPainter.board.MoveCharacter(character, path, onStopMoving);
        ReduceSpeed(character, distance);
    }

    /// <summary>
    /// Adds color to the squares with the board painter representing the walking path that the
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
    /// <para>
    /// Reduces the speed of the character in the quantity provided by one turn
    /// </para>
    /// </summary>
    /// <param name="character">The character that will loose speed</param>
    /// <param name="quantity">The quantity in which the speed will be reduce</param>
    private static void ReduceSpeed(Character character, int quantity)
    {
        character.CharacterStats.AddStatusEffect(new StatusEffect
            (new Stats (0, -quantity), 1, false));
    }

    /// <summary>
    /// Uses the board painter to remove the colors from the walking range and from the walking square if it is any.
    /// </summary>
    /// <param name="boardPainter"></param>
    /// <param name="character"></param>
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
    private static List<Square> GetPath(Board board, Character character, Square destination)
    {
        var speed = character.CharacterStats.Speed;
        if (speed == 0) return new List<Square>();
        var from = board.GetCharacterSquare(character);
        var path = board.GetPath(from, destination);
        if (path.Count > speed) path.RemoveRange(speed + 1, path.Count - 1 - speed);
        return path;
    }
}