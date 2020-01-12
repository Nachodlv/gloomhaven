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
    
    private Dictionary<Character, bool> characters;

    private void Awake()
    {
        turnManager = GetComponent<TurnManager>();
        characters =
            charactersPositions.ToDictionary(characterPosition => characterPosition.character, _ => true);
    }

    /**
     * Subscribes to the OnRoundEnd of the turn manager and starts the round with the list of characters.
     */
    private void Start()
    {
        PositionCharacters();
        turnManager.OnRoundEnd += OnRoundEnd;
        turnManager.StartRound(characters.Keys.ToList());
    }

    /**
     * Positions the characters on the board.
     */
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

    /**
     * Tells the characters active that the round has ended.
     * Starts a new round with them.
     */
    private void OnRoundEnd()
    {
        GetActiveCharacters().ForEach(c => c.OnRoundEnd());
        turnManager.StartRound(GetActiveCharacters());
    }

    /**
     * Returns the characters that are still alive.
     */
    private List<Character> GetActiveCharacters()
    {
        return characters.Where(character => character.Value).Select(r => r.Key).ToList();
    }
}