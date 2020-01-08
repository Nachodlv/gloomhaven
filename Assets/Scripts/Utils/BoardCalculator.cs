using System;
using System.Collections.Generic;

namespace Utils
{
    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public class BoardCalculator
    {
        public static List<Point> CalculatePath(Point from, Point to)
        {
            var path = new List<Point>();
            var i = from.X;
            path.Add(from);
            while (i != to.X)
            {
                if (i <= to.X) i++;
                else i--;
                path.Add(new Point(i, from.Y));
            }

            var j = from.Y;
            while (j != to.Y)
            {
                if (j <= to.Y) j++;
                else j--;
                path.Add(new Point(i, j));
            } 

            return path;
        }

        public static List<Point> CalculateRange(Point from, int distance, Point minPoint, Point maxPoint)
        {
            var points = new List<Point>();
//            var startX = Math.Max(from.X - distance, minPoint.X);
            var startX = from.X - distance;
            var halfX = from.X;
            var finalX = Math.Min(from.X + distance, maxPoint.X);
            var startY = from.Y;
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

                startY = halfX > x? startY - 1 : startY + 1;
                finalY = halfX > x ? finalY + 1 : finalY - 1;
            }

            return points;
        }
    }
}