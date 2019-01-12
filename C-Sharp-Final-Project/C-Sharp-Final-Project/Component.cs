using System;
using System.Windows;

namespace C_Sharp_Final_Project
{
    class Component
    {
        //cool down timer: return false while decrement current time until 0, then reset it to cool down max.
        public static bool CoolDown(ref int coolDownCurrentTime, int coolDownMax, bool autoReset)
        {
            if (coolDownCurrentTime > 0)
            {
                coolDownCurrentTime--;
            } else
            {
                if (autoReset)
                    coolDownCurrentTime = coolDownMax;
                return true;
            }
            return false;
        }

        public static bool DistanceOfPointsLessThan(Vector a, Vector b, double distRange)
        {
            return ((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)) <= (distRange * distRange);
        }

        public static bool BoundaryCheck(Vector minPoint, Vector maxPoint, int posX, int posY)
        {
            if (posX > minPoint.X && posX < maxPoint.X &&
                posY > minPoint.Y && posY < maxPoint.Y)
            {
                return true;
            }
            return false;
        }

        public static bool WallCollision(Tile wall, Vector checkLocation, int radius)
        {
            double DeltaX = checkLocation.X - Math.Max(wall.corner.X, Math.Min(checkLocation.X, wall.corner.X + wall.width ));
            double DeltaY = checkLocation.Y - Math.Max(wall.corner.Y, Math.Min(checkLocation.Y, wall.corner.Y + wall.height));
            
            return (DeltaX * DeltaX + DeltaY * DeltaY) < (radius * radius);
        }

        public static bool ScreenCollision(Vector checkLocation, int radius)
        {
            if (checkLocation.X + radius > Screen.Width ||
                checkLocation.X - radius < 0 || 
                checkLocation.Y + radius > Screen.Height ||
                checkLocation.Y - radius < 0)
            {
                return true;
            }
            
            return false;
        }

        public static bool ScreenCollision(Vector checkLocation)
        {
            if (checkLocation.X > Screen.Width ||
                checkLocation.X < 0 ||
                checkLocation.Y > Screen.Height ||
                checkLocation.Y < 0)
            {
                return true;
            }

            return false;
        }

        public static bool BulletWallCollision(Tile wall, Vector position)
        {
            if (position.X > wall.boundary[0].X &&
                position.X < wall.boundary[1].X &&
                position.Y > wall.boundary[0].Y &&
                position.Y < wall.boundary[1].Y)
            {
                return true;
            }

            return false;
        }

        public static double DistanceOfPoints(Vector a, Vector b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
        
        public static string ChooseRandomWallType()
        {
            Random rs = new Random();
            int rn = rs.Next(0, 10);
            if (rn <= 3)
                return "Textures/BrickWall_Base.png";
            else if (rn <= 4)
                return "Textures/BrickWall_Variant.png";
            else if (rn <= 8)
                return "Textures/BrickWall_Window.png";
            return "Textures/BrickWall_Hole.png";
        }

        public static byte ChooseRandomEnemyType()
        {
            Random rs = new Random();
            int rn = rs.Next(0, 9);
            if (rn <= 3)
                return 1;
            if (rn <= 7)
                return 2;
            return 3;
        }
    }
}
