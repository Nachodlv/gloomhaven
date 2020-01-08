using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Board))]
public class BoardPainter : MonoBehaviour
{
    [Tooltip("Color used to represent the walking path of the character")] [SerializeField]
    private Color walkingColor = Color.green;

    [Tooltip("Color used to represent the walking range of the character")] [SerializeField]
    private Color walkingRange = Color.blue;
    
    [NonSerialized]
    public Board board;

    private Dictionary<Character, List<Square>> walkedSquaresPainted;

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
    public void PaintWalkingSquares(Character character, List<Square> path)
    {
        path.ForEach(s => s.AddColor(walkingColor));
        walkedSquaresPainted.Add(character, path);
    }

    /**
     * Returns the path that the character needs to make between his position and the square.
     */
    private List<Square> GetPath(Character character, Square square)
    {
        return board.GetPath(board.GetCharacterSquare(character), square);
    }
    
    /**
     * Paint the squares that the character is able to move
     */
    public void PaintWalkingRange(Character character)
    {
        GetRange(character).ForEach(s => s.AddColor(walkingRange));
    }

    /**
     * Removes the colors from the squares that the character is able to move
     */
    public void UnPaintWalkingRange(Character character)
    {
        GetRange(character).ForEach(s => s.RemoveColor(walkingRange));
    }

    /**
     * Returns the squares that the character is able to move
     */
    private List<Square> GetRange(Character character)
    {
        var characterSquare = board.GetCharacterSquare(character);
        return board.GetRange(characterSquare, (int) character.stats.Speed);
    } 
}