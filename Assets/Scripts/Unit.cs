using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _actions;
    private int _actionPoints = 2;

    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _actions = GetComponents<BaseAction>();
    }

    private void Update()
    { 
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }

    public MoveAction GetMoveAction()
    {
        return _moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return _spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public BaseAction[] GetActions()
    {
        return _actions;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction action)
    {
        if (CanSpendActionPointsToTakeAction(action))
        {
            SpendActionPoints(action.GetActionPointsCost());
            return true;
        }

        return false;
    }
    
    private bool CanSpendActionPointsToTakeAction(BaseAction action)
    {
        if (_actionPoints >= action.GetActionPointsCost())
        {
            return true;
        }

        return false;
    }

    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;
    }

    public int GetActionPoints()
    {
        return _actionPoints;
    }
}


