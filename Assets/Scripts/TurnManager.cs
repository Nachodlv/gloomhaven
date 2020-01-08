using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityScript.Lang;

public class TurnManager : MonoBehaviour
{
    public BoardPainter boardPainter;

    private List<Character> charactersOrdered;
    private int currentTurn;

    public void StartRound(List<Character> characters)
    {
        characters.Sort((a, b) => (int) a.stats.Initiative - (int) b.stats.Initiative);
        charactersOrdered = characters;
        currentTurn = 0;
        StartTurn();
    }

    private void StartTurn()
    {
        boardPainter.PaintWalkingRange(GetCurrentCharacter());
    }
    
    /**
     * Returns the characters that is playing at the moment
     */
    public Character GetCurrentCharacter()
    {
        return charactersOrdered[currentTurn];
    }
}