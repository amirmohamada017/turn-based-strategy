using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    
    [SerializeField] private Transform gridDebugObjectPrefab;
    private GridSystem _gridSystem;
    private void Awake()
    {
        Instance = this;
        _gridSystem = new GridSystem(10, 10, 2f);
        _gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        var gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }
    
    public List<Unit> GetUnitsAtGridPosition(GridPosition gridPosition)
    {
        var gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnits();
    }
    
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        var gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public GridPosition GetGridPosition(Vector3 position)
    {
        return _gridSystem.GetGridPosition(position);
    }
    
    public void UnitMovedGridPosition(Unit unit, GridPosition oldGridPosition, GridPosition newGridPosition)
    {
        RemoveUnitAtGridPosition(oldGridPosition, unit);
        AddUnitAtGridPosition(newGridPosition, unit);
    }
}