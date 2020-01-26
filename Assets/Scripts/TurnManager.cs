using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public delegate void RoundEnd();

    public event RoundEnd OnRoundEnd;
    public event Action OnCharacterDie;
    
    public BoardPainter boardPainter;

    [SerializeField][Tooltip("Used to build the character UI")]
    private CurrentCharacterUI currentCharacterUi;

    [SerializeField] [Tooltip("Camera of the game")]
    private MoveCamera moveCamera;
    
    private List<Character> charactersOrdered;
    private int currentTurn;
    private AbilityUsedController abilityUsedController;

    public AbilityUsedController AbilityUsedController => abilityUsedController;

    private void Awake()
    {
        abilityUsedController = new AbilityUsedController(boardPainter.board);
        OnRoundEnd += abilityUsedController.OnRoundEnd;
    }

    /// <summary>
    /// <para>Sorts the characters by its initiative and starts the first turn</para>
    /// </summary>
    /// <param name="characters"></param>
    public void StartRound(List<Character> characters)
    {
        characters.Sort((a, b) => a.CharacterStats.Initiative - b.CharacterStats.Initiative);
        charactersOrdered = characters;
        currentTurn = 0;
        NextTurn();
    }

    /// <summary>
    /// <para>
    /// Paint the walking range of the character who is playing.
    /// Set the current character in the UI.
    /// Tells the current character that his turn is beginning.
    /// </para>
    /// </summary>
    private void NextTurn()
    {
        var character = GetCurrentCharacter();
        character.OnTurnStart(boardPainter.board.GetCharacterSquare(character));
        moveCamera.MoveCameraToLocation(boardPainter.board.GetCharacterSquare(character).transform.position);
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
    /// <para>If the character who died is the current character then it invokes the OnCharacterDie event</para>
    /// </summary>
    /// <param name="character">The character who died</param>
    public void OnCharacterDead(Character character)
    {
        if(character == GetCurrentCharacter()) OnCharacterDie?.Invoke();
    }

    private bool IsAnotherTurn()
    {
        while (true)
        {
            currentTurn++;
            if (currentTurn >= charactersOrdered.Count) return false;
            if (GetCurrentCharacter().CharacterStats.Health > 0) return true;
        }
    }
}