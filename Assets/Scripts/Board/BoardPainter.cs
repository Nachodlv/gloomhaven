using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPainter : MonoBehaviour
{

    [Tooltip("Color used to represent the walking path of the character")]
    [SerializeField] private Color walkingColor = Color.green;

    private Dictionary<Character, List<Square>> walkedSquaresPainted;

    private void Awake()
    {
        walkedSquaresPainted = new Dictionary<Character, List<Square>>();
    }

    /**
     * Paints the squares with the walkingColor
     */
    public void PaintWalkingSquares(Character character, List<Square> squares)
    {
        squares.ForEach(square => square.AddColor(walkingColor));
        walkedSquaresPainted.Add(character, squares);
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
}
