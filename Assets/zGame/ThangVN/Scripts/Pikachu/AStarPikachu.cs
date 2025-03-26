using System.Collections.Generic;
using UnityEngine;

public class AStarPikachu : MonoBehaviour
{
    //public Transform startPos, endPos;
    //public LayerMask obstacleLayer;
    //public Vector2Int gridSize = new Vector2Int(10, 10);
    //public float nodeSize = 1f;

    //private Node[,] grid;

    ////void Start()
    ////{
    ////    CreateGrid();
    ////    FindPath(startPos.position, endPos.position);
    ////}

    ////void CreateGrid()
    ////{
    ////    grid = new Node[gridSize.x, gridSize.y];
    ////    Vector3 bottomLeft = transform.position - Vector3.right * gridSize.x * nodeSize / 2 - Vector3.forward * gridSize.y * nodeSize / 2;

    ////    for (int x = 0; x < gridSize.x; x++)
    ////    {
    ////        for (int y = 0; y < gridSize.y; y++)
    ////        {
    ////            Vector3 worldPos = bottomLeft + Vector3.right * (x * nodeSize + nodeSize / 2) + Vector3.forward * (y * nodeSize + nodeSize / 2);
    ////            bool isObstacle = Physics.CheckSphere(worldPos, nodeSize / 2, obstacleLayer);
    ////            grid[x, y] = new Node(isObstacle, worldPos, new Vector2Int(x, y));
    ////        }
    ////    }
    ////}

    //void FindPath(Vector3 startPosition, Vector3 endPosition)
    //{
    //    Node startNode = NodeFromWorldPoint(startPosition);
    //    Node endNode = NodeFromWorldPoint(endPosition);

    //    List<Node> openSet = new List<Node>();
    //    HashSet<Node> closedSet = new HashSet<Node>();
    //    openSet.Add(startNode);

    //    while (openSet.Count > 0)
    //    {
    //        Node currentNode = openSet[0];
    //        for (int i = 1; i < openSet.Count; i++)
    //        {
    //            if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
    //            {
    //                currentNode = openSet[i];
    //            }
    //        }

    //        openSet.Remove(currentNode);
    //        closedSet.Add(currentNode);

    //        if (currentNode == endNode)
    //        {
    //            RetracePath(startNode, endNode);
    //            return;
    //        }

    //        foreach (Node neighbor in GetNeighbors(currentNode))
    //        {
    //            if (neighbor.isObstacle || closedSet.Contains(neighbor))
    //            {
    //                continue;
    //            }

    //            int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
    //            if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
    //            {
    //                neighbor.gCost = newCostToNeighbor;
    //                neighbor.hCost = GetDistance(neighbor, endNode);
    //                neighbor.parent = currentNode;

    //                if (!openSet.Contains(neighbor))
    //                {
    //                    openSet.Add(neighbor);
    //                }
    //            }
    //        }
    //    }
    //}

    //void RetracePath(Node startNode, Node endNode)
    //{
    //    List<Node> path = new List<Node>();
    //    Node currentNode = endNode;

    //    while (currentNode != startNode)
    //    {
    //        path.Add(currentNode);
    //        currentNode = currentNode.parent;
    //    }
    //    path.Reverse();

    //    // Hiển thị đường đi (ví dụ: Debug.Log, vẽ đường line, di chuyển nhân vật)
    //    foreach (Node node in path)
    //    {
    //        //Debug.Log(node.worldPosition);
    //    }
    //}

    //List<Node> GetNeighbors(Node node)
    //{
    //    List<Node> neighbors = new List<Node>();

    //    for (int x = -1; x <= 1; x++)
    //    {
    //        for (int y = -1; y <= 1; y++)
    //        {
    //            if (x == 0 && y == 0) continue;

    //            int checkX = node.gridX + x;
    //            int checkY = node.gridY + y;

    //            if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
    //            {
    //                neighbors.Add(grid[checkX, checkY]);
    //            }
    //        }
    //    }

    //    return neighbors;
    //}

    //Node NodeFromWorldPoint(Vector3 worldPosition)
    //{
    //    float percentX = (worldPosition.x + gridSize.x * nodeSize / 2) / (gridSize.x * nodeSize);
    //    float percentY = (worldPosition.z + gridSize.y * nodeSize / 2) / (gridSize.y * nodeSize);
    //    percentX = Mathf.Clamp01(percentX);
    //    percentY = Mathf.Clamp01(percentY);

    //    int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
    //    int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);
    //    return grid[x, y];
    //}

    //int GetDistance(Node nodeA, Node nodeB)
    //{
    //    int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
    //    int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

    //    if (distX > distY)
    //        return 14 * distY + 10 * (distX - distY);
    //    return 14 * distX + 10 * (distY - distX);
    //}

    
}
