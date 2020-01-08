using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;

    public void OnSquareSelected(Board board, Square square)
    {
        board.MoveCharacter(turnManager.GetCurrentCharacter(), square);
    }

    public void OnSquareHovered(BoardPainter boardPainter, Square square)
    {
        boardPainter.PaintWalkingSquares(turnManager.GetCurrentCharacter(), square);
    }

    public void OnSquareUnHovered(BoardPainter boardPainter)
    {
        boardPainter.UnPaintWalkingSquares(turnManager.GetCurrentCharacter());
    }
}