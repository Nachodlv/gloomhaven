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

    public void OnSquareHovered(Board board, Square square)
    {
        board.GetComponent<BoardPainter>()
            .PaintWalkingSquares(turnManager.GetCurrentCharacter(), GetPath(board, square));
    }

    public void OnSquareNotHovered(Board board, Square square)
    {
        board.GetComponent<BoardPainter>().UnPaintWalkingSquares(turnManager.GetCurrentCharacter());
    }

    private List<Square> GetPath(Board board, Square square)
    {
        return board.GetPath(board.GetCharacterSquare(turnManager.GetCurrentCharacter()), square);
    }
}