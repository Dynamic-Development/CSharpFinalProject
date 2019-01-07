using System;
using System.Collections.Generic;
using System.Windows;

namespace C_Sharp_Final_Project
{
    class Raycaster
    {
        public static bool AreWallsBlockView(Vector fromPoint, Vector toPoint, List<Tile> walls)
        {
            foreach (Tile wall in walls)
            {
                foreach (Vector[] segment in wall.segments)
                {
                    if (isIntersect(fromPoint, toPoint, segment[0], segment[1]))
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
                    currentIntersect = findIntersection(fromPoint, toPoint, segment[0], segment[1]);
                    if (currentIntersect == null)
                        continue;
                    else if(closestIntersect == null || currentIntersect[2] < closestIntersect[2])
                        closestIntersect = currentIntersect;
                }
            }
            return closestIntersect;
        }

        private static double[] findIntersection(Vector lineRA, Vector lineRB, Vector lineSA, Vector lineSB)
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

        private static bool onSegment(Vector q, Vector r, Vector p)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }

        private static int orientation(Vector p, Vector q, Vector r) 
        {
            int val = (int) ((q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y));

            if (val == 0) return 0;  // colinear 

            return (val > 0) ? 1 : 2; // clock or counterclock wise 
        }

        private static bool isIntersect(Vector p1, Vector q1, Vector p2, Vector q2)
        {
            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);

            // General case 
            if (o1 != o2 && o3 != o4)
                return true;
            
            if (o1 == 0 && onSegment(p1, p2, q1)) return true;
            
            if (o2 == 0 && onSegment(p1, q2, q1)) return true;
            
            if (o3 == 0 && onSegment(p2, p1, q2)) return true;
            
            if (o4 == 0 && onSegment(p2, q1, q2)) return true;

            return false;
        }
    }
}
