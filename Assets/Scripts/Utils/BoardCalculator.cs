using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    /**
     * Represents a point in the board. It is used for path and range calculation
     */
    public class Point : IEquatable<Point>
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }


        public bool Equals(Point other)
        {
            if (other == null) return false;
            return X == other.X && Y == other.Y;
        }
    }

    public class BoardCalculator
    {
        /**
         * Represents a point that points to its successor in a path.
         * It is used for calculation the optimal path.
         */
        private class PointPath
        {
            public Point Point;
            public PointPath Successor;
            public int Cost;

            public PointPath NextPointPath(Point point)
            {
                return new PointPath {Point = point, Successor = this, Cost = Cost + 1};
            }
        }

        /**
         * It calculates the optimal path represented by a list of points given the origin, the destination and the
         * blocked points.
         * If the origin and the destination are the same then it returns an empty list.
         */
        public static List<Point> CalculatePath(Point origin, Point destination, List<Point> blockedPoints)
        {
            if(origin.Equals(destination)) return new List<Point>();
            
            var path = CalculatePath(new PointPath() {Cost = 0, Point = origin, Successor = null}, destination, blockedPoints);
            
            if (IsContained(destination, blockedPoints)) path.RemoveAt(path.Count - 1);
            return path;
        }
        

        /**
         * Returns a range represented by a list of points.
         * The range is a circle where its center and radius are the ones passed as parameters.
         * If the circle is bigger than some of the limits then it is cut to fit.
         */
        public static List<Point> CalculateRange(Point center, int radius, Point minPoint, Point maxPoint)
        {
            var points = new List<Point>();
            var startX = center.X - radius;
            var halfX = center.X;
            var finalX = Math.Min(center.X + radius, maxPoint.X);
            var startY = center.Y;
            var finalY = startY;


            for (var x = startX; x <= finalX; x++)
            {
                if (x >= minPoint.X)
                {
                    for (var y = Math.Max(startY, minPoint.Y); y <= Math.Min(finalY, maxPoint.Y); y++)
                    {
                        points.Add(new Point(x, y));
                    }
                }

                startY = halfX > x ? startY - 1 : startY + 1;
                finalY = halfX > x ? finalY + 1 : finalY - 1;
            }

            return points;
        }

        /**
         * Returns true if the point is contained in the list of points.
         * Returns false if the point is not contained in the list of points.
         */
        private static bool IsContained(Point point, List<Point> points)
        {
            return points.Any(p => p.Equals(point));
        }
        
        /**
         * Returns the optimal path given its origin, destination and blocked points.
         */
        private static List<Point> CalculatePath(PointPath origin, Point destination, List<Point> blockedPoints)
        {
            var openPaths = new List<PointPath>()
                {origin};
            var closePaths = new List<PointPath>();
            while (openPaths.Count > 0)
            {
                var nextPoint = openPaths[0];
                openPaths.RemoveAt(0);
                var point = nextPoint.Point;
                var newPaths = new List<PointPath>()
                {
                    nextPoint.NextPointPath(new Point(point.X + 1, point.Y)),
                    nextPoint.NextPointPath(new Point(point.X - 1, point.Y)),
                    nextPoint.NextPointPath(new Point(point.X, point.Y + 1)),
                    nextPoint.NextPointPath(new Point(point.X, point.Y - 1)),
                };

                closePaths.AddRange(newPaths.FindAll(p => p.Point.Equals(destination)));

                openPaths.AddRange(newPaths.Where(p =>
                    !IsContained(p.Point, blockedPoints) && !IsLargerPath(closePaths, p) &&
                    !openPaths.Any(op => op.Point.Equals(p.Point))));
            }

            closePaths.Sort((a, b) => a.Cost.CompareTo(b.Cost));
            return closePaths.Count > 0 ? FromPointPathToList(closePaths[0]) : new List<Point>();
        }

        /**
         * Returns true if the cost of the current path is bigger than any cost of the closed paths.
         * Returns false if the cost of the current path is smaller than all the costs of the closed paths.
         */
        private static bool IsLargerPath(List<PointPath> closedPaths, PointPath currentPath)
        {
            return closedPaths.Any(c => c.Cost <= currentPath.Cost);
        }

        /**
         * Given a PointPath it returns a list of points.
         * It builds the list by iterating the successors of the PointPath
         */
        private static List<Point> FromPointPathToList(PointPath path)
        {
            var pointPath = path;
            var list = new List<Point>();
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