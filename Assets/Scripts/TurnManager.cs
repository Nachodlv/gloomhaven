using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public delegate void RoundEnd();

    public delegate void CharacterDie(Character character);
    public event RoundEnd OnRoundEnd;
    public event CharacterDie OnCharacterDie;
    
    public BoardPainter boardPainter;

    [SerializeField][Tooltip("Used to build the character UI")]
    private CurrentCharacterUI currentCharacterUi;
    private List<Character> charactersOrdered;
    private int currentTurn;
    private AbilityUsedController abilityUsedController;

    public AbilityUsedController AbilityUsedController => abilityUsedController;

    private void Awake()
    {
        abilityUsedController = new AbilityUsedController(boardPainter.board);
    }

    /// <summary>
    /// <para>Sorts the characters by its initiative and starts the first turn</para>
    /// </summary>
    /// <param name="characters"></param>
    public void StartRound(List<Character> characters)
    {
        characters.Sort((a, b) => a.Stats.Initiative - b.Stats.Initiative);
        charactersOrdered = characters;
        currentTurn = 0;
        NextTurn();
    }

    /// <summary>
    /// Paint the walking range of the character who is playing.
    /// Set the current character in the UI.
    /// </summary>
    private void NextTurn()
    {
        var character = GetCurrentCharacter();
        boardPainter.PaintWalkingRange(character);
        currentCharacterUi.SetCurrentCharacter(character);
    }

    /// <summary>
    /// <para>Remove the colors of the board of the previous character.
    /// Set the turn to the next character.
    /// If there are no more characters then it invokes the OnRoundEnd event.</para>
    /// </summary>
    public void EndTurn()
    {
        boardPainter.UnPaintWalkingRange(GetCurrentCharacter());
        boardPainter.UnPaintWalkingSquares(GetCurrentCharacter());
        if (IsAnotherTurn())
        {
            NextTurn();
            return;
        }
        OnRoundEnd?.Invoke();
    }

    /// <summary>
    /// <para>Returns the characters that is playing at the moment</para>
    /// </summary>
    /// <returns></returns>
    public Character GetCurrentCharacter()
    {
        return charactersOrdered[currentTurn];
    }
    
    /// <summary>
    /// <para>Invokes the OnCharacterDie event.</para>
    /// <para>If the character who died is the current character then it ends its turn</para>
    /// </summary>
    /// <param name="character">The character who died</param>
    public void OnCharacterDead(Character character)
    {
        OnCharacterDie?.Invoke(character);
        if(character == GetCurrentCharacter()) EndTurn();
    }

    private bool IsAnotherTurn()
    {
        while (true)
        {
            currentTurn++;
            if (currentTurn >= charactersOrdered.Count) return false;
            if (GetCurrentCharacter().Stats.Health > 0) return true;
        }
    }
}