using System;
using System.Collections.Generic;
using System.Windows;

namespace C_Sharp_Final_Project
{
    class Pathmaster
    {
        Queue<PathRequest> pathRequests = new Queue<PathRequest>();
        PathRequest currentPathRequest;
        static Pathmaster instance;
        Pathfinder pathfinding;
        public int nextPathCoolDown = 0;

        bool isProcessingPath;

        public Pathmaster()
        {
            instance = this;
            pathfinding = new Pathfinder();
        }

        public void RequestPath(Vector pathStart, Vector pathEnd, double distFromTarget, Action<List<Node>, bool> callback)
        {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, distFromTarget, callback);
            instance.pathRequests.Enqueue(newRequest);
        }

        public void TryProcessNext(bool processNow)
        {
            if (processNow && !isProcessingPath && pathRequests.Count > 0)
            {
                currentPathRequest = pathRequests.Dequeue();
                isProcessingPath = true;
                pathfinding.FindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, currentPathRequest.distFromTarget);
            }
        }

        public void FinishedrocessingPath(List<Node> nodePath, bool success)
        {
            currentPathRequest.callback(nodePath, success);
            isProcessingPath = false;
        }

        struct PathRequest
        {
            public Vector pathStart;
            public Vector pathEnd;
            public double distFromTarget;
            public Action<List<Node>, bool> callback;
            public PathRequest(Vector start, Vector end, double dist, Action<List<Node> , bool> callback)
            {
                pathStart = start;
                pathEnd = end;
                distFromTarget = dist;
                this.callback = callback;
            }
        }
    }
}

