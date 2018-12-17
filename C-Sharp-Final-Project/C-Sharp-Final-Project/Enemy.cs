using System;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Enemy
    {
        private const int SPEED = 10; //May subject to change
        private const int SHOOTING_RANGE = 30;
      
        public double xPos, yPos;
        private double xVel, yVel;
        private SDL_Rect dest;
        private IntPtr objTexture;

        private EnemyAI.Pathfinder pathfind;
        private List<Node> nodePath;
        private int findPathCoolDown;
        private int nodeIndex;

        public Enemy(int xPos, int yPos, int width, int height, string texture)
        {
            pathfind = new EnemyAI.Pathfinder();

            this.xPos = xPos;
            this.yPos = yPos;
            xVel = 0;
            yVel = 0;

            findPathCoolDown = 20;
            nodeIndex = 0;

            //TODO: Set objTexture to *Texture.LoadTexture*
            dest.w = width;
            dest.h = height;
        }
        public void Update()
        {

            //TODO: Finish Pathfinding
            if (CoolDown(ref findPathCoolDown, 20))
            {
                nodePath = pathfind.ReturnPath((int)xPos, (int)yPos, Game.Player.xPos, Game.Player.yPos); //TODO: Add player object
                nodeIndex = 0;
            }
            else
            {
                if (nodeIndex < nodePath.Count && distanceToPlayer > SHOOTING_RANGE)
                {
                    VelocityInDirection((int)nodePath[nodeIndex].worldX, (int)nodePath[nodeIndex].worldY);
                    nodeIndex++;
                }
            }

            xPos += xVel;
            yPos += yVel;

            dest.x = (int) xPos - dest.w / 2;
            dest.y = (int) yPos - dest.h / 2;
        }
        private bool CoolDown(ref int coolDownCurrentTime, int coolDownMax)
        {
            if (coolDownCurrentTime < coolDownMax)
                coolDownCurrentTime++;
            else
            {
                coolDownCurrentTime = 0;
                return true;
            }
            return false;
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
