using System;
using UnityEngine;


public class GridSystem<TGridObject>
{
    private readonly int _width;
    private readonly int _height;
    private readonly float _cellSize;
    private readonly TGridObject[,] _gridObjects;

    public GridSystem(int width, int height, float cellSize,
        Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        _height = height;
        _width = width;
        _cellSize = cellSize;
        _gridObjects = new TGridObject[width, height];

        for (var x = 0; x < _width; x++)
        {
            for (var z = 0; z < _height; z++)
            {
                var gridPosition = new GridPosition(x, z);
                _gridObjects[x, z] = createGridObject(this, gridPosition);
            }
        }
    }
    

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.X, 0, gridPosition.Z) * _cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z / _cellSize)
        );
    }
    
    public void CreateDebugObjects(Transform debugPrefab) 
    {
        for (var x = 0; x < _width; x++)
        {
            for (var z = 0; z < _height; z++)
            {
                var gridPosition = new GridPosition(x, z);
                var debugTransform =
                    GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                var gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjects[gridPosition.X, gridPosition.Z];
    }
    
    
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return (gridPosition.X >= 0 && gridPosition.X < _width) && (gridPosition.Z >= 0 && gridPosition.Z < _height);
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }
}
