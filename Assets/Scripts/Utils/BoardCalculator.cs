using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

namespace Utils
{
    public class BoardCalculator
    {
        /**
         * Represents a point that points to its successor in a path.
         * It is used for calculation the optimal path.
         */
        private class PointPath
        {
            public Vector2Int Point;
            public PointPath Successor;
            public int Cost;

            public PointPath NextPointPath(Vector2Int point)
            {
                return new PointPath {Point = point, Successor = this, Cost = Cost + 1};
            }
        }

        /**
         * It calculates the optimal path represented by a list of points given the origin, the destination and the
         * blocked points.
         * If the origin and the destination are the same then it returns an empty list.
         */
        public static List<Vector2Int> CalculatePath(Vector2Int origin, Vector2Int destination,
            List<Vector2Int> blockedPoints, List<List<Vector2Int>> squares, Vector2Int minPoint, Vector2Int maxPoint)
        {
            if (origin.Equals(destination)) return new List<Vector2Int>();

            var path = CalculatePath(new PointPath() {Cost = 0, Point = origin, Successor = null}, destination,
                blockedPoints, squares, minPoint, maxPoint);

            if (IsContained(destination, blockedPoints)) path.RemoveAt(path.Count - 1);
            return path;
        }


        /**
         * Returns a range represented by a list of points.
         * The range is a circle where its center and radius are the ones passed as parameters.
         * If the circle is bigger than some of the limits then it is cut to fit.
         */
        public static List<Vector2Int> CalculateRange(Vector2Int center, int radius, Vector2Int minPoint,
            Vector2Int maxPoint, List<List<Vector2Int>> squares = null)
        {
            var startX = center.x - radius;
            var halfX = center.x;
            var finalX = Math.Min(center.x + radius, maxPoint.x);
            var startY = center.y;
            var finalY = startY;
            var points = new List<Vector2Int>(radius * radius);
            var createSquares = squares == null;

            for (var x = startX; x <= finalX; x++)
            {
                if (x >= minPoint.x)
                {
                    for (var y = Math.Max(startY, minPoint.y); y <= Math.Min(finalY, maxPoint.y); y++)
                    {
                        points.Add(createSquares ? new Vector2Int(x, y) : squares[x][y]);
                    }
                }

                startY = halfX > x ? startY - 1 : startY + 1;
                finalY = halfX > x ? finalY + 1 : finalY - 1;
            }

            return points;
        }

        /**
         * Returns a range represented by a list of points.
         * The range is a circle where its center and radius are the ones passed as parameters.
         */
        public static List<Vector2Int> CalculateRange(Vector2Int center, int radius)
        {
            return CalculateRange(center, radius, new Vector2Int(center.x - radius, center.y - radius),
                new Vector2Int(center.x + radius, center.y + radius));
        }

        /**
         * Returns true if the point is contained in the list of points.
         * Returns false if the point is not contained in the list of points.
         */
        private static bool IsContained(Vector2Int point, List<Vector2Int> points)
        {
            foreach (var p in points)
            {
                if (point.Equals(p)) return true;
            }

            return false;
        }

        /**
         * Returns the optimal path given its origin, destination and blocked points.
         */
        private static List<Vector2Int> CalculatePath(PointPath origin, Vector2Int destination,
            List<Vector2Int> blockedPoints, List<List<Vector2Int>> squares, Vector2Int minPoint, Vector2Int maxPoint)
        {
            var openPaths = new List<PointPath>()
                {origin};
            var closePaths = new List<PointPath>();
            var newPaths = new List<PointPath>(4);
            var emptyList = new List<Vector2Int>(0);
            while (openPaths.Count > 0)
            {
                var nextPoint = openPaths[0];
                openPaths.RemoveAt(0);
                var point = nextPoint.Point;

                if (point.y >= minPoint.y && point.y <= maxPoint.y)
                {
                    if (point.x + 1 <= maxPoint.x) newPaths.Add(nextPoint.NextPointPath(squares[point.x + 1][point.y]));
                    if (point.x - 1 >= minPoint.x) newPaths.Add(nextPoint.NextPointPath(squares[point.x - 1][point.y]));
                }

                if (point.x >= minPoint.x && point.x <= maxPoint.x)
                {
                    if (point.y + 1 <= maxPoint.y) newPaths.Add(nextPoint.NextPointPath(squares[point.x][point.y + 1]));
                    if (point.y - 1 >= minPoint.y)newPaths.Add(nextPoint.NextPointPath(squares[point.x][point.y - 1]));
                    
                }

                foreach (var path in newPaths)
                {
                    if (path.Point.Equals(destination)) closePaths.Add(path);
                    if (IsContained(path.Point, blockedPoints) || IsLargerPath(closePaths, path)) continue;
                    var equals = false;
                    foreach (var openPath in openPaths)
                    {
                        if (path.Point.Equals(openPath.Point)) equals = true;
                    }

                    if (!equals) openPaths.Add(path);
                }

                newPaths.Clear();
            }

            closePaths.Sort((a, b) => a.Cost.CompareTo(b.Cost));
            return closePaths.Count > 0 ? FromPointPathToList(closePaths[0]) : emptyList;
        }

        /**
         * Returns true if the cost of the current path is bigger than any cost of the closed paths.
         * Returns false if the cost of the current path is smaller than all the costs of the closed paths.
         */
        private static bool IsLargerPath(List<PointPath> closedPaths, PointPath currentPath)
        {
            foreach (var path in closedPaths)
            {
                if (path.Cost <= currentPath.Cost) return true;
            }

            return false;
        }

        /**
         * Given a PointPath it returns a list of points.
         * It builds the list by iterating the successors of the PointPath
         */
        private static List<Vector2Int> FromPointPathToList(PointPath path)
        {
            var pointPath = path;
            var list = new List<Vector2Int>();
            while (pointPath.Successor != null)
            {
                list.Insert(0, pointPath.Point);
                pointPath = pointPath.Successor;
            }

            list.Insert(0, pointPath.Point);
            return list;
        }
    }
}