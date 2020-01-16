using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public delegate void RoundEnd();
    public event RoundEnd OnRoundEnd;

    
    public BoardPainter boardPainter;

    private List<Character> charactersOrdered;
    private int currentTurn;

    /**
     * Sorts the characters by its initiative and starts the first turn
     */
    public void StartRound(List<Character> characters)
    {
        characters.Sort((a, b) => (int) a.Stats.Initiative - (int) b.Stats.Initiative);
        charactersOrdered = characters;
        currentTurn = 0;
        NextTurn();
    }

    /**
     * Paint the walking range of the character who is playing
     */
    private void NextTurn()
    {
        FindObjectOfType<SelectionManager>().StartTurn(); //TODO remove
    }

    /**
     * Remove the colors of the board of the previous character.
     * Set the turn to the next character.
     * If there are no more characters then it invokes the OnRoundEnd event.
     */
    public void EndTurn()
    {
        boardPainter.UnPaintWalkingRange(GetCurrentCharacter());
        boardPainter.UnPaintWalkingSquares(GetCurrentCharacter());
        currentTurn++;
        if (currentTurn < charactersOrdered.Count)
        {
            NextTurn();
            return;
        }
        OnRoundEnd?.Invoke();
    }

    /**
     * Returns the characters that is playing at the moment
     */
    public Character GetCurrentCharacter()
    {
        return charactersOrdered[currentTurn];
    }
}