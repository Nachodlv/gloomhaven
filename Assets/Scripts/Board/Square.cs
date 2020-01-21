using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

[RequireComponent(typeof(SpriteRenderer))]
public class Square : MonoBehaviour
{
//    public int x;
//    public int y;
    [NonSerialized]
    public Vector2Int Point;

    private SpriteRenderer spriteRenderer;
    private List<Color> colors;
    private Color initialColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colors = new List<Color>(3);
        initialColor = Color.white;
        colors.Add(Color.white);
    }

    /**
     * Adds the color to the sprite renderer color
     */
    public void AddColor(Color newColor)
    {
        colors.Add(newColor);
        SetColor();
    }

    /**
     * Removes the color to the sprite renderer color
     */
    public void RemoveColor(Color color)
    {
        if (!colors.Remove(color)) return;
        SetColor();
    }

    /**
     * Combines all the colors of the square and it sets it to the sprite renderer
     */
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