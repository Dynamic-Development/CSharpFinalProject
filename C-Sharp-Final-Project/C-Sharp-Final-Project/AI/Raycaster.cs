using System;
using System.Collections.Generic;
using System.Windows;

namespace C_Sharp_Final_Project
{
    class Raycaster
    {
        public static bool IsWallsBlockView(Vector fromPoint, Vector toPoint, List<Tile> walls)
        {
            foreach (Tile wall in walls)
            {
                foreach (Vector[] segment in wall.segments)
                {
                    if (Intersection(fromPoint, toPoint, segment[0], segment[1]) != null)
                        return true;
                }
            }
            return false;
        }

        public static double[] Cast(Vector fromPoint, Vector toPoint, List<Tile> walls) 
        {
            double[] closestIntersect = null;
            double[] currentIntersect;
            foreach (Tile wall in walls)
            {
                foreach(Vector[] segment in wall.segments)
                {
                    currentIntersect = Intersection(fromPoint, toPoint, segment[0], segment[1]);
                    if (currentIntersect == null)
                        continue;
                    else if(closestIntersect == null || currentIntersect[2] < closestIntersect[2])
                        closestIntersect = currentIntersect;
                }
            }
            return closestIntersect;
        }

        private static double[] Intersection(Vector lineRA, Vector lineRB, Vector lineSA, Vector lineSB)
        {
            Vector rayPoint = lineRA;
            Vector rayDist = lineRB - lineRA;
            Vector segPoint = lineSA;
            Vector segDist = lineSB - lineSA;

            if (rayDist.X * segDist.Y == rayDist.Y * segDist.X)
                return null;

            double t2 = ((segPoint.Y - rayPoint.Y) * rayDist.X + (rayPoint.X - segPoint.X) * rayDist.Y) / 
                        (segDist.X * rayDist.Y - segDist.Y * rayDist.X);

            if (t2 < 0 || t2 > 1)
                return null;

            double t1;
            if (rayDist.X == 0)
                t1 = (segDist.Y * t2 - rayPoint.Y + segPoint.Y) / rayDist.Y;
            else
                t1 = (segDist.X * t2 - rayPoint.X + segPoint.X) / rayDist.X;

            if (t1 < 0)
                return null;

            return new double[] { rayPoint.X + rayDist.X * t1, rayPoint.Y + rayDist.Y * t1, t1 };
        }
    }
}
