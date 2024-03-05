using System.Collections.Generic;

public class GridObject
{
    private GridSystem<GridObject> _gridSystem;
    private readonly GridPosition _gridPosition;
    private List<Unit> _units;
    private IInteractable _interactable;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _units = new List<Unit>();
    }
    
    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
    }
    
    public List<Unit> GetUnits()
    {
        return _units;
    }
    
    public void RemoveUnit(Unit unit)
    {
        _units.Remove(unit);
    }
    
    public override string ToString()
    {
        var unitString = "";
        foreach (var unit in _units)
        {
            unitString += unit + "\n";
        }
        
        return _gridPosition.ToString() + unitString;
    }

    public bool HasAnyUnit()
    {
        return _units.Count > 0;
    }

    public Unit GetUnit()
    {
        return HasAnyUnit() ? _units[0] : null;
    }
    
    public IInteractable GetInteractable()
    {
        return _interactable;
    }
    
    public void SetInteractable(IInteractable interactable)
    {
        _interactable = interactable;
    }
}
