using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class Board : MonoBehaviour
{
    [Header("Board size")] [Tooltip("Quantity of squares in the z axis")] [SerializeField]
    private int width = 10; // z

    [Tooltip("Quantity of squares in the x axis")] [SerializeField]
    public int height = 10; // x

    [Header("Square details")] [SerializeField]
    private Square squarePrefab;

    public Character initialCharacter; //TODO remove
    [SerializeField] private SelectionManager selectionManager;

    private Dictionary<Character, Square> characters;
    private List<List<Square>> squares;

    private void Awake()
    {
        characters = new Dictionary<Character, Square>();
        InstantiateSquares();
    }

    /**
     * Instantiate the squares multidimensional list using the width and height variables.
     */
    private void InstantiateSquares()
    {
        var boardPainter = GetComponent<BoardPainter>();

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
                AssignSelection(newSquare, boardPainter);
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
        characters[character] = path[path.Count - 1];
    }


    /**
     * Returns the path between the parameter from and the parameter to.
     * The path is represented as a list of squares
     */
    public List<Square> GetPath(Square from, Square to)
    {
        var path = BoardCalculator.CalculatePath(FromSquareToPoint(from), FromSquareToPoint(to));
        return path.Select(FromPointToSquare).ToList();
    }

    public List<Square> GetRange(Square init, int distance)
    {
        var range = BoardCalculator.CalculateRange(FromSquareToPoint(init), distance, new Point(0, 0),
            new Point(width -1, height -1));
        return range.Select(FromPointToSquare).ToList();
    }

    /**
     * Delegates the selection and the hover of a square to the SelectionManager.
     */
    private void AssignSelection(Square square, BoardPainter boardPainter)
    {
        square.GetComponent<Clickable>().onMouseDown = _ => selectionManager.OnSquareSelected(this, square);
        var hoverable = square.GetComponent<Hoverable>();
        hoverable.onMouseEnter = _ => selectionManager.OnSquareHovered(boardPainter, square);
        hoverable.onMouseExit = _ => selectionManager.OnSquareUnHovered(boardPainter);
    }

    /**
     * Add a character to the board.
     * Returns the square where the character was positioned.
     * If the position is out of bounds then it returns null.
     */
    public Square AddCharacter(Character character, int x, int z)
    {
        if (width < z || height < x || z < 0 || x < 0) return null;

        var square = squares[z][x];
        characters.Add(character, square);
        return square;
    }

    private Square FromPointToSquare(Point point)
    {
        return squares[point.X][point.Y];
    }

    private static Point FromSquareToPoint(Square square)
    {
        return new Point(square.x, square.y);
    }
}