using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CharacterPosition
{
    public Character character;
    [Header("Position")] public int x;
    public int z;
}

[RequireComponent(typeof(TurnManager))]
public class GameController : MonoBehaviour
{
    [Tooltip("Characters that will be on the board")] [SerializeField]
    private List<CharacterPosition> charactersPositions;

    [SerializeField] private Board board;
    private TurnManager turnManager;
    [SerializeField] private GameOverUI gameOverUi;

    private Dictionary<Character, bool> characters;

    private void Awake()
    {
        turnManager = GetComponent<TurnManager>();
        characters =
            charactersPositions.ToDictionary(characterPosition => characterPosition.character, _ => true);
    }

    /// <summary>
    /// Subscribes to the OnRoundEnd of the turn manager and starts the round with the list of characters.
    /// </summary>
    private void Start()
    {
        PositionCharacters();
        turnManager.OnRoundEnd += OnRoundEnd;
        turnManager.OnCharacterDie += OnCharacterDie;
        turnManager.StartRound(characters.Keys.ToList());
    }

    /// <summary>
    /// <para>Positions the characters on the board</para>
    /// </summary>
    private void PositionCharacters()
    {
        charactersPositions.ForEach(characterPosition =>
        {
            var square = board.AddCharacter(characterPosition.character, characterPosition.x, characterPosition.z);
            var squarePosition = square.transform.position;
            var characterTransform = characterPosition.character.transform;
            characterTransform.position =
                new Vector3(squarePosition.x, characterTransform.position.y, squarePosition.z);
        });
    }

    /// <summary>
    /// <para>Tells the characters active that the round has ended.
    /// Starts a new round with them.</para>
    /// </summary>
    private void OnRoundEnd()
    {
        GetActiveCharacters().ForEach(c => c.OnRoundEnd());
        turnManager.StartRound(GetActiveCharacters());
    }

    /// <summary>
    /// <para>Returns the characters that are still alive.</para>
    /// </summary>
    /// <returns></returns>
    private List<Character> GetActiveCharacters()
    {
        var activeCharacters = new List<Character>();
        foreach (var character in this.characters)
        {
            if (character.Value) activeCharacters.Add(character.Key);
        }

        return activeCharacters;
    }

    private void OnCharacterDie(Character character)
    {
        characters[character] = false;
        if (GetActiveCharacters().Count > 1) return;

        GameOver();
    }

    private void GameOver()
    {
        gameOverUi.ShowGameOver();
    }
}