using System;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Enemy
    {
        private const int SPEED = 1; //May subject to change
        private const int SHOOTING_RANGE = 30;
      
        public double xPos, yPos;

        private double xVel, yVel;
        private SDL_Rect dest;
        private IntPtr objTexture;   

        private int searchPlayerCoolDown;
        private int[] xPath, yPath;
        private int currentTargetX, currentTargetY;
        private int targetI;
        private bool pathPending;
        private int oldPlayerX;
        private int oldPlayerY;

        public Enemy(int xPos, int yPos, int width, int height, string texture)
        {
            objTexture = Texture.LoadTexture(texture);

            this.xPos = xPos;
            this.yPos = yPos;
            xVel = 0;
            yVel = 0;
 
            searchPlayerCoolDown = 0;
            pathPending = false;
            oldPlayerX = Game.Player.xpos;
            oldPlayerY = Game.Player.ypos;

            dest.w = width;
            dest.h = height;
        }

        public void Update()
        {
            if (Component.CoolDown(ref searchPlayerCoolDown, 20))
            {
                if (oldPlayerX != Game.Player.xpos || oldPlayerY != Game.Player.ypos)
                {
                    oldPlayerY = Game.Player.ypos;
                    oldPlayerX = Game.Player.xpos;
                    if (!pathPending)
                    {
                        Game.PathManager.RequestPath((int)xPos, (int)yPos, Game.Player.xpos, Game.Player.ypos, OnPathFound);
                        pathPending = true;
                    }
                }
            }
            
        
            if (xPath != null && yPath != null /*&& distanceToPlayer > SHOOTING_RANGE /*&& sawPlayer*/)
            {         
                if (targetI < xPath.Length - 1 && currentTargetY == (int) Math.Round(yPos, MidpointRounding.AwayFromZero) && 
                                                  currentTargetX == (int) Math.Round(xPos, MidpointRounding.AwayFromZero))
                {
                    targetI++;
                    currentTargetX = xPath[targetI];
                    currentTargetY = yPath[targetI];
                    VelocityInDirection(currentTargetX, currentTargetY);
                }

                if (!(xPath[xPath.Length - 1] == (int) Math.Round(xPos, MidpointRounding.AwayFromZero) &&
                      yPath[yPath.Length - 1] == (int) Math.Round(yPos, MidpointRounding.AwayFromZero)))
                {
                    xPos += xVel;
                    yPos += yVel;
                }
            }

            dest.x = (int) xPos - dest.w / 2;
            dest.y = (int) yPos - dest.h / 2;
        }

        private void OnPathFound(int[] xPositions, int[] yPositions, bool found)
        {
            if (found)
            {
                if (xPositions.Length != 0 && yPositions.Length != 0)
                {
                    xPath = xPositions;
                    yPath = yPositions;
                  
                    currentTargetX = xPath[0];
                    currentTargetY = yPath[0];
                    VelocityInDirection(currentTargetX, currentTargetY);

                    targetI = 0;
                }
                pathPending = false;                
            }
        }

        private void VelocityInDirection(int gotoX, int gotoY)
        {
            double distance = Math.Sqrt(((gotoX - xPos) * (gotoX - xPos)) + ((gotoY-yPos) * (gotoY-yPos)));
            double directionX = (gotoX - xPos) / distance;
            double directionY = (gotoY - yPos) / distance;

            xVel = directionX * SPEED;
            yVel = directionY * SPEED;
        }

        public void Render()
        {
            SDL_RenderCopy(Game.Renderer, objTexture, IntPtr.Zero, ref dest);
        }
    }
}
