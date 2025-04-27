using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public Grid grid;
    public List<Node> path; // List to store the calculated path

    void Awake()
    {
        // Get the Grid component attached to the same GameObject
        grid = GetComponent<Grid>();
    }

    // Public method to get the path from startPos to targetPos
    public List<Node> GetPath(Vector3 startPos, Vector3 targetPos)
    {
        // Calculate the path 
        FindAStarPath(startPos, targetPos);
        // Return the calculated path stored in the Grid script
        return grid.path;
    }

    // A* algorithm implementation to find the path
    private void FindAStarPath(Vector3 startPos, Vector3 targetPos)
    {
        // Convert world positions to nodes in the grid
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        // List of nodes to be evaluated
        List<Node> openSet = new List<Node>();
        // HashSet of nodes already evaluated
        HashSet<Node> closedSet = new HashSet<Node>();
        // Add the start node to the open set
        openSet.Add(startNode);
        // Loop until all nodes in the open set are evaluated
        while (openSet.Count > 0)
        {
            // Get the node with the lowest fCost from the open set
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                // If the fCost of the current node is lower or if fCosts are equal but hCost is lower
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    //select this node
                    if (openSet[i].hCost < node.hCost)
                    {
                        node = openSet[i];
                    }
                }
            }
            // Remove the current node from the open set and add it to the closed set
            openSet.Remove(node);
            closedSet.Add(node);
            // If the target node is reached
            if (node == targetNode)
            {
                //Retrace the path and return
                RetracePath(startNode, targetNode);
                return;
            }
            // Loop each neighbour of the current node
            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                // Skip unwalkable or already evaluated neighbours
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }
                // Calculate the new cost to reach the neighbour
                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                // If the new cost is lower or the neighbour is not in the open set
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour; // Update gCost
                    neighbour.hCost = GetDistance(neighbour, targetNode); // Update hCost
                    neighbour.parent = node; // Set the current node as the parent of the neighbour
                    // Add the neighbour to the open set if it's not already there
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    // Retrace the path from the end node to the start node
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        // Traverse from the end node to the start node using parent references
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        // Reverse the path to get it from start to end
        path.Reverse();
        // Store the calculated path in the Grid script
        grid.path = path;
    }

    // Method to calculate the distance between two nodes
    int GetDistance(Node nodeA, Node nodeB)
    {
        // Calculate the absolute difference in X and Y coordinates
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        // Determine if the movement is more horizontal or vertical
        if (dstX > dstY)
        {
            // More horizontal movement: horizontal steps = (dstX - dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        }
        else
        {
            // More vertical movement: vertical steps = (dstY - dstX)
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
