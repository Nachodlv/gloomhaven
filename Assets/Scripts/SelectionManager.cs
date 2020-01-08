using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    private bool moving;

    /**
     * Moves the player to the selected square. Remove the colors from the walking range and the walking trail
     */
    public void OnSquareSelected(BoardPainter boardPainter, Square square)
    {
        if (moving) return;

        var character = turnManager.GetCurrentCharacter();
        if (character.stats.Speed == 0) return;
        moving = true;

        RemovesPainting(boardPainter, character);
        var distance = boardPainter.board.MoveCharacter(character, GetPath(boardPainter.board, character, square), () =>
        {
            moving = false;
            if(character.stats.Speed > 0) boardPainter.PaintWalkingRange(character);
        });
        ReducesSpeed(character, distance);
    }

    /**
     * Add color to the squares representing the walking path that the character will make if the square is selected.
     */
    public void OnSquareHovered(BoardPainter boardPainter, Square square)
    {
        if(moving) return;
        var character = turnManager.GetCurrentCharacter();
        boardPainter.PaintWalkingSquares(character, GetPath(boardPainter.board, character, square));
    }

    /**
     * Removes the color to the squares that was added when it was hovered.
     */
    public void OnSquareUnHovered(BoardPainter boardPainter)
    {
        boardPainter.UnPaintWalkingSquares(turnManager.GetCurrentCharacter());
    }

    /**
     * Reduces the speed of the character in the quantity provided by one turn
     */
    private static void ReducesSpeed(Character character, int quantity)
    {
        character.stats.AddStatusEffect(new StatusEffect
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

    /**
     * Gets the path that the character need to do to get to its destination.
     * The path is limited by the speed of the character
     */
    private List<Square> GetPath(Board board, Character character, Square destination)
    {
        var speed = (int) character.stats.Speed;
        if (speed == 0) return new List<Square>();
        var from = board.GetCharacterSquare(character);
        var path = board.GetPath(from, destination);
        if (path.Count > speed) path.RemoveRange(speed + 1, path.Count - 1 - speed);
        return path;
    }
}