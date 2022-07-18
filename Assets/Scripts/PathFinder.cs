using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    readonly List<List<Ground>> groundList;
    Vector2Int size;
    List<Ground> openList;
    List<Ground> closeList;

    public PathFinder(List<List<Ground>> groundList, Vector2Int size)
    {
        this.groundList = groundList;
        this.size = new(size.x, size.y);
    }

    public List<Ground> FindPath(Ground start, Ground end)
    {
        openList = new List<Ground> { start };
        closeList = new();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                groundList[i][j].gCost = int.MaxValue;
                groundList[i][j].CalculateFcost();
                groundList[i][j].cameFromGround = null;
            }
        }
        start.gCost = 0;
        start.hCost = CalculateHCost(start, end);
        start.CalculateFcost();
        while (openList.Count > 0)
        {
            Ground current = GetLowestFCostGround(openList);
            if (current == end)
            {
                return CalculatePath(end);
            }
            openList.Remove(current);
            closeList.Add(current);

            foreach (Ground neighbour in GetNeighbours(current))
            {
                if (closeList.Contains(neighbour)) continue;
                if (!neighbour.isMovable)
                {
                    closeList.Add(neighbour);
                    continue;
                }
                int tempGCost = current.gCost + CalculateHCost(current, neighbour);
                if (tempGCost < neighbour.gCost)
                {
                    neighbour.cameFromGround = current;
                    neighbour.gCost = tempGCost;
                    neighbour.hCost = CalculateHCost(neighbour, end);
                    neighbour.CalculateFcost();

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }

        }
        return null;
    }

    private List<Ground> GetNeighbours(Ground current)
    {
        List<Ground> neighbours = new();
        if ((int)current.transform.position.x - 1 >= 0)
        {
            neighbours.Add(GetGround((int)current.transform.position.x - 1, (int)current.transform.position.z));
            if ((int)current.transform.position.z - 1 >= 0)
                neighbours.Add(GetGround((int)current.transform.position.x - 1, (int)current.transform.position.z - 1));
            if ((int)current.transform.position.z + 1 < size.y)
                neighbours.Add(GetGround((int)current.transform.position.x - 1, (int)current.transform.position.z + 1));
        }
        if ((int)current.transform.position.x + 1 < size.x)
        {
            neighbours.Add(GetGround((int)current.transform.position.x + 1, (int)current.transform.position.z));
            if (current.transform.position.z - 1 >= 0)
                neighbours.Add(GetGround((int)current.transform.position.x + 1, (int)current.transform.position.z - 1));
            if ((int)current.transform.position.z + 1 < size.y)
                neighbours.Add(GetGround((int)current.transform.position.x + 1, (int)current.transform.position.z + 1));
        }
        if ((int)current.transform.position.z - 1 >= 0)
            neighbours.Add(GetGround((int)current.transform.position.x, (int)current.transform.position.z - 1));
        if ((int)current.transform.position.z + 1 < size.y)
            neighbours.Add(GetGround((int)current.transform.position.x, (int)current.transform.position.z + 1));
        return neighbours;
    }

    private Ground GetGround(int x, int y)
    {
        return groundList[x][y];
    }

    private List<Ground> CalculatePath(Ground end)
    {
        List<Ground> path = new()
        {
            end
        };
        Ground current = end;
        while (current.cameFromGround != null)
        {
            path.Add(current.cameFromGround);
            current = current.cameFromGround;
        }
        path.Reverse();
        return path;
    }

    private int CalculateHCost(Ground a, Ground b)
    {
        int xDistance = Mathf.Abs((int)a.transform.position.x - (int)b.transform.position.x);
        int yDistance = Mathf.Abs((int)a.transform.position.z - (int)b.transform.position.z);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return 14 * Mathf.Min(xDistance, yDistance) + 10 * remaining;
    }

    private Ground GetLowestFCostGround(List<Ground> list)
    {
        Ground lowestFcostGround = list[0];
        foreach (Ground ground in list)
        {
            if (ground.fCost < lowestFcostGround.fCost)
            {
                lowestFcostGround = ground;
            }
        }
        return lowestFcostGround;
    }
}
