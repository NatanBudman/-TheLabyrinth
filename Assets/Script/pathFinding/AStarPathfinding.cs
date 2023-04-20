using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public LayerMask wallMask;
    public Transform start, end;
    public float nodeRadius;
    Node startNode, endNode;
    [SerializeField]private Grid _grid;

    [SerializeField] private List<Node> www;
    void Start()
    {
        FindPath(start.position,end.position);
    }

    void FindPath(Vector3 star,Vector3 end)
    {
        startNode =_grid.NodeFromWorldPoint(star);
        endNode = _grid.NodeFromWorldPoint(end);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < currentNode.fCost || 
                    openSet[i].fCost == currentNode.fCost && 
                    openSet[i].hCost < currentNode.hCost) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode) {
                GetFinalPath(startNode, endNode);
                return;
            }

            foreach (Node neighbour in GetNeighbours(currentNode)) {
                if (closedSet.Contains(neighbour)) {
                    continue;
                }

                int newCostToNeighbour = currentNode.gCost + _grid.GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = _grid.GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void GetFinalPath(Node start, Node end)
    {
        List<Node> finalPath = new List<Node>();
        Node currentNode = end;
        while (currentNode != start) {
            finalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        finalPath.Reverse();
    }


    List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        www = neighbours;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && y == 0 && z == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    int checkZ = node.gridZ + z;

                    if (checkX >= 0 && checkX < _grid.gridWidth && 
                        checkY >= 0 && checkY < _grid.gridHeight && 
                        checkZ >= 0 && checkZ < _grid.gridDepth)
                    {
                        neighbours.Add(_grid.grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }

        return neighbours;
    }
}