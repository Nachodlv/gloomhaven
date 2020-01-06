using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Board size")] [Tooltip("Quantity of squares in the z axis")]
    public int width = 10;

    [Tooltip("Quantity of squares in the x axis")]
    public int height = 10;

    [Header("Square details")] public Square squarePrefab;
    public Character initialCharacter; //TODO remove

    private Dictionary<Character, Square> characters;
    private List<List<Square>> squares;

    private void Awake()
    {
        characters = new Dictionary<Character, Square>();
        InstantiateSquares();
    }

    private void Start()
    {
        //TODO remove
        characters.Add(initialCharacter, squares[0][0]);
        MoveCharacter(initialCharacter, squares[5][5]);
    }

    /**
     * Instantiate the squares multidimensional list using the width and height variables.
     */
    private void InstantiateSquares()
    {
        var bounds = squarePrefab.GetComponent<SpriteRenderer>().sprite.bounds;
        var spriteHeight = bounds.size.x;
        var spriteWidth = bounds.size.y;

        squares = new List<List<Square>>();
        var position = transform.position;
        var x = position.x;
        var z = position.z;
        for (var i = 0; i < width; i++)
        {
            var column = new List<Square>();
            squares.Add(column);
            for (var j = 0; j < height; j++)
            {
                var newSquare = Instantiate(squarePrefab, new Vector3(x, 0, z), squarePrefab.transform.rotation,
                    transform);
                newSquare.x = i;
                newSquare.y = j;
                column.Add(newSquare.GetComponent<Square>());
                x += spriteHeight;
            }

            x = position.x;
            z += spriteWidth;
        }
    }

    /**
     * Returns the square where the character is positioned.
     * If the characters is not in the board it will return null.
     */
    public Square GetCharacterSquare(Character character)
    {
        return !characters.ContainsKey(character) ? null : characters[character];
    }

    /**
     * Moves the character to the destination through the squares of the board
     */
    public void MoveCharacter(Character character, Square destination)
    {
        var from = GetCharacterSquare(character);
        var path = GetPath(@from, destination);
        character.GetComponent<Movable>().MoveCharacter(path.Select(square => square.transform.position).ToList());
    }


    /**
     * Returns the path between the parameter from and the parameter to.
     * The path is represented as a list of squares
     */
    private List<Square> GetPath(Square from, Square to)
    {
        var path = new List<Square>();
        var i = from.x;
        for (; i < to.x; i++)
        {
            path.Add(squares[i][from.y]);
        }

        for (var j = from.y; j <= to.y; j++)
        {
            path.Add(squares[i][j]);
        }

        return path;
    }
}