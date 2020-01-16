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

    private Color abilityRange = Color.yellow;
    private Color abilityAreaOfEffect = Color.red;

    [NonSerialized] public Board board;

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
     * Paint the squares that the character is able to move
     */
    public void PaintWalkingRange(Character character)
    {
        GetWalkingRange(character).ForEach(s => s.AddColor(walkingRange));
    }

    /**
     * Removes the colors from the squares that the character is able to move
     */
    public void UnPaintWalkingRange(Character character)
    {
        GetWalkingRange(character).ForEach(s => s.RemoveColor(walkingRange));
    }

    public void PaintAbilityRange(Character character, int range)
    {
        var rangeToColor = board.GetRange(board.GetCharacterSquare(character), range);
        rangeToColor.ForEach(square => square.AddColor(abilityRange));
    }

    public void UnPaintAbilityRange(Character character, int range)
    {
        var rangeToColor = board.GetRange(board.GetCharacterSquare(character), range);
        rangeToColor.ForEach(square => square.RemoveColor(abilityRange));
    }

    public void PaintAbilityAreaOfEffect(Square destination, List<Vector2Int> areaOfEffect)
    {
        GetAbilityAreaOfEffect(destination, areaOfEffect).ForEach(square => square.AddColor(abilityAreaOfEffect));
    }

    public void UnPaintAbilityAreaOfEffect(Square destination, List<Vector2Int> areaOfEffect)
    {
        GetAbilityAreaOfEffect(destination, areaOfEffect).ForEach(square => square.RemoveColor(abilityAreaOfEffect));
    }

    /**
     * Returns the squares that the character is able to move
     */
    private List<Square> GetWalkingRange(Character character)
    {
        var characterSquare = board.GetCharacterSquare(character);
        return board.GetRange(characterSquare, (int) character.Stats.Speed);
    }

    private List<Square> GetAbilityAreaOfEffect(Square destination, List<Vector2Int> areaOfEffect)
    {
        var positions = areaOfEffect.Select(position =>
            new Vector2Int(position.x + destination.x, position.y + destination.y)).ToList();
        return board.GetSquares(positions);
    }
}