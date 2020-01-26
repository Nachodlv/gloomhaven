using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using UnityEngine;
using Utils;

[RequireComponent(typeof(SpriteRenderer))]
public class Square : MonoBehaviour
{
//    public int x;
//    public int y;
    [NonSerialized]
    public Vector2Int Point;

    public List<StatusEffect> StatusEffects => statusEffects;
    
    private SpriteRenderer spriteRenderer;
    private List<Color> colors;
    private Color initialColor;
    private List<StatusEffect> statusEffects;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        statusEffects = new List<StatusEffect>();
        colors = new List<Color>(3);
        initialColor = Color.white;
        colors.Add(Color.white);
    }

    /// <summary>
    /// <para>Adds the color to the sprite renderer color</para>
    /// </summary>
    /// <param name="newColor"></param>
    public void AddColor(Color newColor)
    {
        colors.Add(newColor);
        SetColor();
    }

    /// <summary>
    /// <para>Removes the color to the sprite renderer color</para>
    /// </summary>
    /// <param name="color"></param>
    public void RemoveColor(Color color)
    {
        if (!colors.Remove(color)) return;
        SetColor();
    }

    /// <summary>
    /// <para>Adds an array of StatusEffect to the square.</para>
    /// <para>Adds the color to the square from the new StatusEffect</para>
    /// </summary>
    /// <param name="newStatusEffects"></param>
    public void AddStatusEffects(StatusEffect[] newStatusEffects)
    {
        foreach (var statusEffect in newStatusEffects)
        {
            statusEffects.Add(StatusEffect.ResetDurationLeft(statusEffect));
            AddColor(statusEffect.Color);
        }
    }

    /// <summary>
    /// <para>Removes the StatusEffect at the index passed as parameter.</para>
    /// <para>Removes the color from the StatusEffect removed.</para>
    /// </summary>
    /// <param name="index"></param>
    public void RemoveStatusEffectAt(int index)
    {
        RemoveColor(statusEffects[index].Color);
        StatusEffects.RemoveAt(index);
    }
    
    /// <summary>
    /// <para>Combines all the colors of the square and it sets it to the sprite renderer</para>
    /// </summary>
    private void SetColor()
    {
        var finalColor = new Color(0, 0, 0, 0);
        foreach (var color in colors)
        {
            finalColor += color;
        }
        spriteRenderer.color = finalColor / colors.Count;
    }
}