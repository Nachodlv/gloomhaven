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
    
    private List<Character> characters;

    private void Awake()
    {
        turnManager = GetComponent<TurnManager>();
        characters = charactersPositions.Select(characterPosition => characterPosition.character).ToList();
    }

    private void Start()
    {
        PositionCharacters();
        turnManager.StartRound(characters);
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
    
    
    // Update is called once per frame
    void Update()
    {
    }
}