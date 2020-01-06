using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Square : MonoBehaviour
{
    public int x;
    public int y;

    private SpriteRenderer spriteRenderer;
    private List<Color> colors;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colors = new List<Color>();
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
        colors.ForEach(color => finalColor += color);
        spriteRenderer.color = finalColor / colors.Count;
    }
}