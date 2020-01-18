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
                AssignSelection(newSquare);
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
    public int MoveCharacter(Character character, List<Square> path, Action onFinishMoving)
    {
        if (path.Count == 0)
        {
            onFinishMoving();
            return 0;
        }
        character.GetComponent<Movable>()
            .MoveCharacter(path.Select(square => square.transform.position).ToList(), onFinishMoving);
        characters[character] = path[path.Count - 1];
        return path.Count - 1; // The first position is shouldn't me counted
    }


    /**
     * Returns the path between the parameter from and the parameter to.
     * The path is represented as a list of squares
     */
    public List<Square> GetPath(Square from, Square to)
    {
        var path = BoardCalculator.CalculatePath(FromSquareToPoint(from), FromSquareToPoint(to),
            characters.Values.Select(FromSquareToPoint).ToList());
        return path.Select(FromPointToSquare).ToList();
    }

    /**
     * Returns the squares that are in the distance passed as parameter to the center square
     */
    public List<Square> GetRange(Square center, int distance)
    {
        var range = BoardCalculator.CalculateRange(FromSquareToPoint(center), distance, new Point(0, 0),
            new Point(width - 1, height - 1));
        return range.Select(FromPointToSquare).ToList();
    }

    /**
     * Delegates the selection and the hover of a square to the SelectionManager.
     */
    private void AssignSelection(Square square)
    {
        square.GetComponent<Clickable>().onMouseDown = _ => selectionManager.OnSquareSelected(square);
        var hoverable = square.GetComponent<Hoverable>();
        hoverable.onMouseEnter = _ => selectionManager.OnSquareHovered(square);
        hoverable.onMouseExit = _ => selectionManager.OnSquareUnHovered(square);
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

    /// <summary>
    /// Returns the squares that represent the <paramref name="positions"/> given.
    /// If any of the points given is put of range the it will be ignored.
    /// </summary>
    /// <param name="positions">A list representing the squares</param>
    /// <returns>A list of the squares in the given <paramref name="positions"/></returns>
    public List<Square> GetSquares(List<Vector2Int> positions)
    {
        var squaresToReturn = new List<Square>();
        positions.ForEach(position =>
        {
            if (position.x < height && position.x >= 0 && position.y < width && position.y >= 0) 
                squaresToReturn.Add(squares[position.x][position.y]);
        });
        return squaresToReturn;
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