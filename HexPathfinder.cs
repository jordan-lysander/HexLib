namespace HexLib;

public static class HexPathfinder
{
    /// <summary>
    /// A helper class to represent the nodes in the pathfinding process.
    /// Implements IComparable to allow PriorityQueues to automatically determine priority.
    /// </summary>
    internal class PathNode : IComparable<PathNode>
    {
        public HexCoordinates Coordinates { get; }
        public PathNode? Parent { get; set; }
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => GCost + HCost;

        public PathNode(HexCoordinates coordinates)
        {
            Coordinates = coordinates;
            GCost = int.MaxValue;       // initialise with high cost
        }

        /// <summary>
        /// Compares this PathNode to another PathNode for priority queue ordering.
        /// It compares by FCost, then by HCost in the event of a tie.
        /// </summary>
        public int CompareTo(PathNode? other)
        {
            if (other == null) return 1;
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
                compare = HCost.CompareTo(other.HCost);
            return compare;
        }
    }

    /// <summary>
    /// Finds the shortest path between two hexagonal coordinates on a grid.
    /// </summary>
    /// <param name="startCoords">The starting coordinates for the path.</param>
    /// <param name="endCoords">The destination coordinates for the path.</param>
    /// <param name="getTileCost">A delegate function to retrieve tile movement cost, if it exists.</param>
    /// <returns>A list of HexCoordinates representing the path from start to end. Returns empty list if no path is found.</returns>
    public static List<HexCoordinates> FindPath(HexCoordinates startCoords, HexCoordinates endCoords, Func<HexCoordinates, int> getTileCost)
    {
        // the set of nodes to be evaluated, prioritised by FCost
        var openSet = new PriorityQueue<PathNode, PathNode>();

        // the set of nodes that have already been evaluated
        var closedSet = new HashSet<HexCoordinates>();

        // storage for all nodes in a dict to look them up by their coordinates
        var allNodes = new Dictionary<HexCoordinates, PathNode>();

        // create and initialise the starting node
        var startNode = new PathNode(startCoords)
        {
            GCost = 0,
            HCost = HexCoordinates.Distance(startCoords, endCoords)
        };
        allNodes.Add(startCoords, startNode);
        openSet.Enqueue(startNode, startNode);

        while (openSet.Count > 0)
        {
            var currentNode = openSet.Dequeue();

            if (currentNode.Coordinates.Equals(endCoords))
            {
                return RetracePath(currentNode);
            }

            if (closedSet.Contains(currentNode.Coordinates))
                continue;

            closedSet.Add(currentNode.Coordinates);

            // get the neighbours of the current node
            foreach (var neighbourCoords in currentNode.Coordinates.GetNeighbours())
            {
                if (closedSet.Contains(neighbourCoords))
                    continue;

                // use the delegate function to retrieve the movement cost of the tile
                int moveCost = getTileCost(neighbourCoords);
                if (moveCost == int.MaxValue)
                    continue;

                // get/create the PathNode for the neighbour
                if (!allNodes.TryGetValue(neighbourCoords, out var neighbourNode))
                {
                    neighbourNode = new PathNode(neighbourCoords);
                    allNodes.Add(neighbourCoords, neighbourNode);
                }

                int tentativeGCost = currentNode.GCost + moveCost;

                // if this path is better than any previous one, record it
                if (tentativeGCost < neighbourNode!.GCost)
                {
                    neighbourNode.Parent = currentNode;
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = HexCoordinates.Distance(neighbourCoords, endCoords);

                    // enqueue the neighbour with its updated cost
                    openSet.Enqueue(neighbourNode, neighbourNode);
                    }
                }
            }

        // return empty list if no path is found
        return [];
    }

    private static List<HexCoordinates> RetracePath(PathNode endNode)
    {
        var path = new List<HexCoordinates>();
        var currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(currentNode.Coordinates);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
}