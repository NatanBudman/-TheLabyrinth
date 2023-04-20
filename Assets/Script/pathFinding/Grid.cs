using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public int gridWidth;
    public int gridHeight;
    public int gridDepth;
    public float cellSize;
    public Node[,,] grid;

    // Inicialización de la grilla
    void Start()
    {
        CreateGrid();
    }

    // Crear la grilla
    void CreateGrid()
    {
        grid = new Node[gridWidth, gridHeight, gridDepth];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWidth / 2 -
                                  Vector3.forward * gridHeight / 2 - Vector3.up * gridDepth / 2;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * cellSize + cellSize / 2) +
                                         Vector3.forward * (y * cellSize + cellSize / 2) +
                                         Vector3.up * (z * cellSize + cellSize / 2);
                    bool walkable = Physics.CheckSphere(worldPoint, cellSize / 2);
                    grid[x, y, z] = new Node(walkable, worldPoint, x, y, z);
                }
            }
        }
    }

    // Obtener el nodo correspondiente a una posición dada en el mundo
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWidth / 2) / gridWidth;
        float percentY = (worldPosition.y + gridDepth / 2) / gridDepth;
        float percentZ = (worldPosition.z + gridHeight / 2) / gridHeight;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridWidth - 1) * percentX);
        int y = Mathf.RoundToInt((gridHeight - 1) * percentY);
        int z = Mathf.RoundToInt((gridDepth - 1) * percentZ);
        return grid[x, y, z];
    }
    public int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        if (dstX > dstZ && dstX > dstY)
            return 14*dstZ + 10*(dstX-dstZ-dstY) + 10*Mathf.Min(dstY, dstZ);
        if (dstY > dstZ)
            return 14*dstZ + 10*(dstY-dstZ) + 10*Mathf.Min(dstX, dstZ);
        return 14*dstX + 10*(dstZ-dstX-dstY) + 10*Mathf.Min(dstY, dstZ);
    }
}


