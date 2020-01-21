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

    [SerializeField] private SelectionManager selectionManager;

    [SerializeField] private MoveCamera moveCamera;
    
    private Dictionary<Character, Square> characters;
    private List<List<Square>> squares;
    private List<List<Vector2Int>> squarePoints;

    public Dictionary<Character, Square> Characters => characters;

    private Vector2Int minPoint;
    private Vector2Int maxPoint;

    private void Awake()
    {
        characters = new Dictionary<Character, Square>();
        minPoint = new Vector2Int(0, 0);
        maxPoint = new Vector2Int(width - 1, height - 1);

        InstantiateSquares();
        moveCamera.MinPoint = squares[0][0].transform.position;
        moveCamera.MaxPoint = squares[width - 1][height - 1].transform.position;
        squarePoints = FromSquaresToPoints();
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
                newSquare.Point = new Vector2Int(i, j);
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
        var path = BoardCalculator.CalculatePath(from.Point, to.Point,
            characters.Values.Select(v => v.Point).ToList(), squarePoints, minPoint, maxPoint);
        return path.Select(FromVectorToSquare).ToList();
    }

    /**
     * Returns the squares that are in the distance passed as parameter to the center square
     */
    public List<Square> GetRange(Square center, int distance)
    {
        var range = BoardCalculator.CalculateRange(center.Point, distance, minPoint, maxPoint, squarePoints);
        var rangeSquares = new List<Square>(range.Count);
        foreach (var point in range)
        {
            rangeSquares.Add(FromVectorToSquare(point));
        }

        return rangeSquares;
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
        var squaresToReturn = new List<Square>(positions.Count);
        foreach (var position in positions)
        {
            if (position.x < height && position.x >= 0 && position.y < width && position.y >= 0)
                squaresToReturn.Add(squares[position.x][position.y]);
        }

        return squaresToReturn;
    }

    public Square GetSquare(int x, int y)
    {
        if (x < height && x >= 0 && y < width && y >= 0)
            return squares[x][y];
        return null;
    }

    private Square FromVectorToSquare(Vector2Int vector)
    {
        return squares[vector.x][vector.y];
    }

    private List<List<Vector2Int>> FromSquaresToPoints()
    {
        return squares.Select(col => col.Select(square => square.Point).ToList()).ToList();
    }
}