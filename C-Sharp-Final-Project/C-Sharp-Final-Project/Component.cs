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

        public static double DistanceOfPoints(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
        
        public static double DistanceOfPoints(Vector a, Vector b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }
}
