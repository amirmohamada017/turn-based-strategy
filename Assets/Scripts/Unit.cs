using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition _gridPosition;
    private MoveAction _moveAction;

    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
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

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
}
