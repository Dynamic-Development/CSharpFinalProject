using System;
using System.Collections.Generic;
using System.Windows;

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

        public void RequestPath(Vector pathStart, Vector pathEnd, Action<List<Node>, bool> callback)
        {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
            instance.pathRequests.Enqueue(newRequest);
        }

        public void TryProcessNext(bool processNow)
        {
            if (processNow && !isProcessingPath && pathRequests.Count > 0)
            {
                currentPathRequest = pathRequests.Dequeue();
                isProcessingPath = true;
                pathfinding.FindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
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
            public Action<List<Node>, bool> callback;
            public PathRequest(Vector start, Vector end, Action<List<Node> , bool> callback)
            {
                pathStart = start;
                pathEnd = end;
                this.callback = callback;
            }
        }
    }
}

