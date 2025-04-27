using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // Whether the node is walkable
    public bool walkable;
    // The world position of the node 
    public Vector3 worldPosition;
    // The grid coordinates of the node 
    public int gridX;
    public int gridY;
    // The cost from the start node to this node g(n)
    public int gCost;
    // The heuristic cost from this node to the target node h(n)
    public int hCost;
    // Reference to the parent node
    public Node parent;
    // Internal field for storing the position
    internal Vector3 position;
    // Initialize a node
    public Node(bool walk, Vector3 pos, int X, int Y)
    {
        walkable = walk; // Set whether the node is walkable
        worldPosition = pos; // Set the world position of the node
        gridX = X; // Set the X coordinate in the grid
        gridY = Y; // Set the Y coordinate in the grid
    }
    //Calculate the total cost f(n)
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}

