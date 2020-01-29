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

    [Tooltip("Color used to represent the range of the abilities")] [SerializeField]
    private Color abilityRange = Color.yellow;

    [Tooltip("Color used to represent the area of effect of the abilities")] [SerializeField]
    private Color abilityAreaOfEffect = Color.red;

    [NonSerialized] public Board board;

    private Dictionary<Character, List<Square>> walkedSquaresPainted;
    private Dictionary<Character, List<Square>> walkingRangePainted;
    private Dictionary<Character, List<Square>> abilityAreaOfEffectPainted;
    private Dictionary<Character, List<Square>> abilityRangePainted;

    private void Awake()
    {
        walkedSquaresPainted = new Dictionary<Character, List<Square>>();
        walkingRangePainted = new Dictionary<Character, List<Square>>();
        abilityAreaOfEffectPainted = new Dictionary<Character, List<Square>>();
        abilityRangePainted = new Dictionary<Character, List<Square>>();
        board = GetComponent<Board>();
    }

    /// <summary>
    /// <para>Removes the walkingColor of the squares</para>
    /// </summary>
    /// <param name="character"></param>
    public void UnPaintWalkingSquares(Character character)
    {
        if (!walkedSquaresPainted.ContainsKey(character)) return;
        walkedSquaresPainted[character].ForEach(square => square.RemoveColor(walkingColor));
        walkedSquaresPainted.Remove(character);
    }

    /// <summary>
    /// <para>Paints the squares with the walkingColor</para>
    /// </summary>
    /// <param name="character"></param>
    /// <param name="path"></param>
    public void PaintWalkingSquares(Character character, List<Square> path)
    {
        path.ForEach(s => s.AddColor(walkingColor));
        walkedSquaresPainted.Add(character, path);
    }

    /// <summary>
    /// <para>Paint the squares that the character is able to move</para>
    /// </summary>
    /// <param name="character"></param>
    public void PaintWalkingRange(Character character)
    {
        if(walkingRangePainted.ContainsKey(character)) UnPaintWalkingRange(character);
        var range = board.GetRange(board.GetCharacterSquare(character), character.CharacterStats.Speed);
        walkingRangePainted.Add(character, range);

        foreach (var square in range)
        {
            square.AddColor(walkingRange);
        }
    }

    /// <summary>
    /// <para>Removes the walking character range color from the board</para>
    /// </summary>
    /// <param name="character"></param>
    public void UnPaintWalkingRange(Character character)
    {
        if (!walkingRangePainted.ContainsKey(character)) return;
        foreach (var square in walkingRangePainted[character])
        {
            square.RemoveColor(walkingRange);
        }

        walkingRangePainted.Remove(character);
    }

    /// <summary>
    /// <para> Paints the range of the ability using the character as center with the abilityRange color. 
    /// It assigns the character to the squares painted for more efficient painting removal.</para>
    /// </summary>
    /// <param name="character"></param>
    /// <param name="range"></param>
    public void PaintAbilityRange(Character character, int range)
    {
        var rangeToColor = board.GetRange(board.GetCharacterSquare(character), range);
        abilityRangePainted.Add(character, rangeToColor);
        foreach (var square in rangeToColor)
        {
            square.AddColor(abilityRange);
        }

    }

    /// <summary>
    /// <para>Remove the abilityRange color assigned to the character</para>
    /// </summary>
    /// <param name="character"></param>
    public void UnPaintAbilityRange(Character character)
    {
        if (!abilityRangePainted.ContainsKey(character)) return;
        var rangeToColor = abilityRangePainted[character];
        foreach (var square in rangeToColor)
        {
            square.RemoveColor(abilityRange);
        }

        abilityRangePainted.Remove(character);
    }

    /// <summary>
    /// <para>Paints the points of the area of effect using the destination as center.
    /// It assigns the character to the squares painted for more efficient painting removal.</para>
    /// </summary>
    /// <param name="character"></param>
    /// <param name="destination"></param>
    /// <param name="areaOfEffect"></param>
    public void PaintAbilityAreaOfEffect(Character character, Square destination, List<Vector2Int> areaOfEffect)
    {
        var squares = GetAbilityAreaOfEffect(destination, areaOfEffect);
        foreach (var square in squares)
        {
            square.AddColor(abilityAreaOfEffect);
        }
        abilityAreaOfEffectPainted.Add(character, squares);
    }

    /// <summary>
    /// <para>Removes the area of effect of an ability assigned to the character.</para>
    /// </summary>
    /// <param name="character"></param>
    public void UnPaintAbilityAreaOfEffect(Character character)
    {
        if (!abilityAreaOfEffectPainted.ContainsKey(character)) return;
        foreach (var square in abilityAreaOfEffectPainted[character])
        {
            square.RemoveColor(abilityAreaOfEffect);
        }
        abilityAreaOfEffectPainted.Remove(character);
    }

    private List<Square> GetAbilityAreaOfEffect(Square destination, List<Vector2Int> areaOfEffect)
    {
        var positions = new List<Square>(areaOfEffect.Count);
        foreach (var vector in areaOfEffect)
        {
           var square = board.GetSquare(vector.x + destination.Point.x, vector.y + destination.Point.y);
           if(square != null) positions.Add(square);
        }
        return positions;
    }
    
}