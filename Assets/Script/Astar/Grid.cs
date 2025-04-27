using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // LayerMask to define which objects are unwalkable
    public LayerMask unwalkableMask;
    // The size of the grid in world units
    public Vector2 gridWorldSize;
    // The radius of each node 
    public float nodeRadius;
    // 2D array to store all nodes in the grid
    Node[,] grid;
    // Diameter of each node 
    float nodeDiameter;
    // X and Y axes of the grid
    int gridSizeX, gridSizeY;
    void Awake()
    {
        // Calculate the node diameter
        nodeDiameter = nodeRadius * 2;
        // Calculate the number of nodes along the X and Y axes based on the grid size and node diameter
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    // Method to create the grid of nodes
    void CreateGrid()
    {
        // Initialize the grid array
        grid = new Node[gridSizeX, gridSizeY];
        // Calculate the bottom-left corner of the grid in world space
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        // Loop through each cell in the grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Calculate the world position of the current node
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                // Check if the node is walkable by performing a physics check for obstacles
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                // Create a new node and add it to the grid
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // Method to get the neighboring nodes of a given node
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        // Loop through the neighbors 
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Skip the current node 
                if (x == 0 && y == 0)
                {
                    continue;
                }
                // Calculate the grid coordinates of the neighbor
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                // Check if the neighbor is within the grid bounds
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    // Add the neighbor to the list
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    // Method to convert a world position to a node in the grid
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // Calculate the percentage of the world position within the grid
        float percentX = (worldPosition.x + gridWorldSize.x / 7) / gridWorldSize.x;
        float percentY = (float)(worldPosition.z + gridWorldSize.y / 2.7) / gridWorldSize.y;
        // Clamp the percentages to ensure they stay within the grid bounds
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        // Convert the percentages to grid coordinates
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        // Return the node at the calculated grid coordinates
        return grid[x, y];
    }

    // List to store the calculated path
    public List<Node> path;
    // Method to draw gizmos
    void OnDrawGizmos()
    {
        // Draw a wireframe cube to represent the grid boundaries
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        // If the grid is initialized draw each node
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                // Set the color of the node based on whether it's walkable or part of the path
                Gizmos.color = (n.walkable) ? Color.yellow : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.blue;
                // Draw a cube at the node's position
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
