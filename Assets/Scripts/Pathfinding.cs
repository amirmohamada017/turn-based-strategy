using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }
    
    private const int MoveStraightCost = 10;
    private const int MoveDiagonalCost = 14;
    
    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;
    
    private GridSystem<PathNode> _gridSystem;
    private int _width;
    private int _height;
    private float _cellSize;

    private void Awake()
    {
        Instance = this;
    }

    public void SetUp(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        
        _gridSystem = new GridSystem<PathNode>(_width, _height, _cellSize,
            (GridSystem<PathNode> gridSystem,GridPosition gridPosition) => new PathNode(gridPosition));
        _gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        SetUnWalkableNodes();
    }

    private void SetUnWalkableNodes()
    {
        for (var x = 0; x < _width; x++)
        {
            for (var z = 0; z < _height; z++)
            {
                var gridPosition = new GridPosition(x, z);
                var worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                const float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up,
                        raycastOffsetDistance * 2, obstaclesLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        var openList = new List<PathNode>();
        var closeList = new List<PathNode>();

        var startNode = _gridSystem.GetGridObject(startGridPosition);
        var endNode = _gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        for (var x = 0; x < _gridSystem.GetWith(); x++)
        {
            for (var z = 0; z < _gridSystem.GetHeight(); z++)
            {
                var gridPosition = new GridPosition(x, z);
                var pathNode = _gridSystem.GetGridObject(gridPosition);
                
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromNode();
            }
        }
        
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            var currentNode = getLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (var neighbour in GetNeighbours(currentNode))
            {
                if (closeList.Contains(neighbour))
                    continue;

                if (!neighbour.IsWalkable())
                {
                    closeList.Add(neighbour);
                    continue;
                }

                var tentativeGCost = currentNode.GetGCost() +
                                     CalculateDistance(currentNode.GetGridPosition(), neighbour.GetGridPosition());

                if (tentativeGCost > neighbour.GetGCost())
                {
                    neighbour.SetCameFromPathNode(currentNode);
                    neighbour.SetGCost(tentativeGCost);
                    neighbour.SetHCost(CalculateDistance(neighbour.GetGridPosition(), endNode.GetGridPosition()));
                    neighbour.CalculateFCost();

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        pathLength = 0;
        return null;
    }

    public int CalculateDistance(GridPosition a, GridPosition b)
    {
        var gridPositionDistance = a - b;

        var xDistance = Mathf.Abs(gridPositionDistance.X);
        var zDistance = Mathf.Abs(gridPositionDistance.Z);
        var remaining = Mathf.Abs(xDistance - zDistance);
        return MoveDiagonalCost * Mathf.Min(xDistance, zDistance) + MoveStraightCost * remaining;
    }

    private PathNode getLowestFCostPathNode(List<PathNode> pathNodes)
    {
        var lowestFCostPathNode = pathNodes[0];
        foreach (var t in pathNodes)
        {
            if (t.GetFCost() < lowestFCostPathNode.GetFCost())
                lowestFCostPathNode = t;
        }

        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return _gridSystem.GetGridObject(new GridPosition(x, z));
    }
    
    private List<PathNode> GetNeighbours(PathNode pathNode)
    {
        var neighbours = new List<PathNode>();

        var gridPosition = pathNode.GetGridPosition();

        for (var i = -1; i < 2; i++)
        {
            for (var j = -1; j < 2; j++)
            {
                if (CheckValidPosition(i, j))
                    neighbours.Add(GetNode(gridPosition.X + i, gridPosition.Z + j));
            }
        }
        
        return neighbours;
    }

    private bool CheckValidPosition(int x, int z)
    {
        return x >= 0 && x < _gridSystem.GetWith() && z >= 0 && z < _gridSystem.GetHeight() && !(x==0 && z==0);
    }

    private List<GridPosition> CalculatePath(PathNode node)
    {
        var nodes = new List<PathNode> { node };

        var currentNode = node;
        
        while (currentNode.GetCameFromPathNode() != null)
        {
            nodes.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        nodes.Reverse();

        var gridPositions = new List<GridPosition>();

        foreach (var pathNode in nodes)
        {
            gridPositions.Add(pathNode.GetGridPosition());
        }
        
        return gridPositions;
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return _gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out var pathLength) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out var pathLength);
        return pathLength;
    }
}
