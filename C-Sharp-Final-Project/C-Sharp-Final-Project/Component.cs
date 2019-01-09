using System;
using System.Windows;

namespace C_Sharp_Final_Project
{
    class Component
    {
        //cool down timer: return false while decrement current time until 0, then reset it to cool down max.
        public static bool CoolDown(ref int coolDownCurrentTime, int coolDownMax)
        {
            if (coolDownCurrentTime > 0)
            {
                coolDownCurrentTime--;
            } else
            {
                coolDownCurrentTime = coolDownMax;
                return true;
            }
            return false;
        }

        public static bool DistanceOfPointsLessThan(Vector a, Vector b, double distRange)
        {
            return ((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)) <= (distRange * distRange);
        }

        public static bool BoundaryCheck(Vector minPoint, Vector maxPoint, Vector checkLocation)
        {
            if (checkLocation.X > minPoint.X && checkLocation.X < maxPoint.X &&
                checkLocation.Y > minPoint.Y && checkLocation.Y < maxPoint.Y)
            {
                return true;
            }
            return false;
        }
        
        public static double DistanceOfPoints(Vector a, Vector b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
        
    }
}
