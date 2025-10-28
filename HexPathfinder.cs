using System.Collections;
using System.Runtime.Serialization;

namespace HexLib;

public static class HexPathfinder
{
    // helper class to represent the nodes in the pathfinding process
    internal class PathNode
    {
        public HexCoordinates Coordinates { get; }
        public PathNode? Parent { get; set; }
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get; set; }

        public PathNode(HexCoordinates coordinates)
        {
            Coordinates = coordinates;
            GCost = int.MaxValue;
        }
    }

    public static List<HexCoordinates> FindPath(HexCoordinates startCoords, HexCoordinates endCoords, Func<HexCoordinates, int> getTileCost)
    {
        var openSet = new List<PathNode>();
        var closedSet = new HashSet<HexCoordinates>();

        var startNode = new PathNode(startCoords)
        {
            GCost = 0,
            HCost = HexCoordinates.Distance(startCoords, endCoords)
        };
        openSet.Add(startNode);

        var allNodes = new Dictionary<HexCoordinates, PathNode> { { startCoords, startNode } };

        while (openSet.Count > 0)
        {
            var currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode.Coordinates);

            if (currentNode.Coordinates.Equals(endCoords))
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (var neighbourCoords in currentNode.Coordinates.GetNeighbours())
            {
                if (closedSet.Contains(neighbourCoords))
                {
                    continue;
                }

                int tentativeGCost = currentNode.GCost + 1;

                if (!allNodes.TryGetValue(neighbourCoords, out var neighbourNode))
                {
                    neighbourNode = new PathNode(neighbourCoords);
                    allNodes.Add(neighbourCoords, neighbourNode);
                }

                if (tentativeGCost < neighbourNode.GCost || !openSet.Contains(neighbourNode))
                {
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = HexCoordinates.Distance(neighbourCoords, endCoords);
                    neighbourNode.Parent = currentNode;

                    if (!openSet.Contains(neighbourNode))
                    {
                        openSet.Add(neighbourNode);
                    }
                }
            }
        }

        return new List<HexCoordinates>();
    }

    private static List<HexCoordinates> RetracePath(PathNode startNode, PathNode endNode)
    {
        var path = new List<HexCoordinates>();
        var currentNode = endNode;

        while (currentNode != null && !currentNode.Equals(startNode))
        {
            path.Add(endNode.Coordinates);
            currentNode = currentNode.Parent;
        }
        path.Add(startNode.Coordinates);


        path.Reverse();
        return path;
    }
}