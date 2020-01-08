using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Board))]
public class BoardPainter : MonoBehaviour
{
    [Tooltip("Color used to represent the walking path of the character")] [SerializeField]
    private Color walkingColor = Color.green;

    [Tooltip("Color used to represent the walking range of the character")] [SerializeField]
    private Color walkingRange = Color.blue;

    private Dictionary<Character, List<Square>> walkedSquaresPainted;
    private Board board;

    private void Awake()
    {
        walkedSquaresPainted = new Dictionary<Character, List<Square>>();
        board = GetComponent<Board>();
    }

    /**
     * Removes the walkingColor of the squares
     */
    public void UnPaintWalkingSquares(Character character)
    {
        if (!walkedSquaresPainted.ContainsKey(character)) return;
        walkedSquaresPainted[character].ForEach(square => square.RemoveColor(walkingColor));
        walkedSquaresPainted.Remove(character);
    }

    /**
    * Paints the squares with the walkingColor
    */
    public void PaintWalkingSquares(Character character, Square square)
    {
        var path = GetPath(character, square);
        path.ForEach(s => s.AddColor(walkingColor));
        walkedSquaresPainted.Add(character, path);
    }

    private List<Square> GetPath(Character character, Square square)
    {
        return board.GetPath(board.GetCharacterSquare(character), square);
    }
    
    public void PaintWalkingRange(Character character)
    {
        var characterSquare = board.GetCharacterSquare(character);
        var range = board.GetRange(characterSquare, (int)character.stats.Speed);
        range.ForEach(s => s.AddColor(walkingRange));
    }
}