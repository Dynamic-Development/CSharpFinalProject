using System;
using System.Collections.Generic;


namespace C_Sharp_Final_Project
{
    class PathMaster
    {
        Queue<PathRequest> pathRequests = new Queue<PathRequest>();
        PathRequest currentPathRequest;
        static PathMaster instance;
        Pathfinder pathfinding;
        public int nextPathCoolDown = 0;

        bool isProcessingPath;

        public PathMaster()
        {
            instance = this;
            pathfinding = new Pathfinder();
        }

        public void RequestPath(int pathStartX, int pathStartY, int pathEndX, int pathEndY, Action<int[], int[], bool> callback)
        {
            PathRequest newRequest = new PathRequest(pathStartX, pathStartY, pathEndX, pathEndY, callback);
            instance.pathRequests.Enqueue(newRequest);
            // to delete
        }

        public void TryProcessNext(bool processNow)
        {
            if (processNow && !isProcessingPath && pathRequests.Count > 0)
            {
                currentPathRequest = pathRequests.Dequeue();
                isProcessingPath = true;
                pathfinding.FindPath(currentPathRequest.pathStartX, currentPathRequest.pathStartY,
                                       currentPathRequest.pathEndX, currentPathRequest.pathEndY);
            }
        }

        public void FinishedrocessingPath(int[] pathX, int[] pathY, bool success)
        {
            currentPathRequest.callback(pathX, pathY, success);
            isProcessingPath = false;
         // to delete
        }

        struct PathRequest
        {
            public int pathStartX, pathStartY;
            public int pathEndX, pathEndY;
            public Action<int[], int[], bool> callback;
            public PathRequest(int startX, int startY, int endX, int endY, Action<int[], int[], bool> callback)
            {
                pathEndX = endX;
                pathEndY = endY;
                pathStartX = startX;
                pathStartY = startY;
                this.callback = callback;
            }
        }
    }
}

