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
    private Dictionary<Character, List<Square>> abilityAreaOfEffectPainted;

    private void Awake()
    {
        walkedSquaresPainted = new Dictionary<Character, List<Square>>();
        abilityAreaOfEffectPainted = new Dictionary<Character, List<Square>>();
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
        GetWalkingRange(character).ForEach(s => s.AddColor(walkingRange));
    }

    /// <summary>
    /// <para>Removes the colors from the squares that the character is able to move</para>
    /// </summary>
    /// <param name="character"></param>
    public void UnPaintWalkingRange(Character character)
    {
        GetWalkingRange(character).ForEach(s => s.RemoveColor(walkingRange));
    }

    /// <summary>
    /// Paints the range of the ability using the character as center with the abilityRange color.
    /// </summary>
    /// <param name="character"></param>
    /// <param name="range"></param>
    public void PaintAbilityRange(Character character, int range)
    {
        var rangeToColor = board.GetRange(board.GetCharacterSquare(character), range);
        rangeToColor.ForEach(square => square.AddColor(abilityRange));
    }

    /// <summary>
    /// Remove the abilityRange color from the range using the character as center.
    /// </summary>
    /// <param name="character"></param>
    /// <param name="range"></param>
    public void UnPaintAbilityRange(Character character, int range)
    {
        var rangeToColor = board.GetRange(board.GetCharacterSquare(character), range);
        rangeToColor.ForEach(square => square.RemoveColor(abilityRange));
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
        squares.ForEach(square => square.AddColor(abilityAreaOfEffect));
        abilityAreaOfEffectPainted.Add(character, squares);
    }

    /// <summary>
    /// <para>Removes the area of effect of an ability assigned to the character.</para>
    /// </summary>
    /// <param name="character"></param>
    public void UnPaintAbilityAreaOfEffect(Character character)
    {
        if (!abilityAreaOfEffectPainted.ContainsKey(character)) return;
        abilityAreaOfEffectPainted[character].ForEach(square => square.RemoveColor(abilityAreaOfEffect));
        abilityAreaOfEffectPainted.Remove(character);
    }

    /// <summary>
    /// <para>Returns the squares that the character is able to move</para>
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
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