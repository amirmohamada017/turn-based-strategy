using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    
    [SerializeField] private int maxMoveDistance = 4;
    
    private List<Vector3> _positions;
    private int _currentPositionIndex;
    
    private void Update()
    {
        if (!isActive) return;
        
        var targetPosition = _positions[_currentPositionIndex];
        var moveDirection = (targetPosition - transform.position).normalized;
        
        const float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
        
        const float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            const float moveSpeed = 4f; 
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        }
        else
        {
            _currentPositionIndex++;
            
            if (_currentPositionIndex >= _positions.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        var pathGridPositions = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out var pathLength);
        
        _currentPositionIndex = 0;
        _positions = new List<Vector3>();

        foreach (var pathGridPosition in pathGridPositions)
            _positions.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        
        ActionStart(onActionComplete);
    }
    
    public override List<GridPosition> GetValidActionGridPositions()
    {
        var validGridPositions = new List<GridPosition>();
        var unitGridPosition = unit.GetGridPosition();

        for (var x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (var z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                var offsetGridPosition = new GridPosition(x, z);
                var testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                if (unitGridPosition == testGridPosition)
                    continue;

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;
                
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                    continue;
                
                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                    continue;

                var pathFindingMaxMoveDistance = maxMoveDistance * 10;
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > pathFindingMaxMoveDistance)
                    continue;

                validGridPositions.Add(testGridPosition);
            }
        }
        
        return validGridPositions;
    }
    
    public override string GetActionName()
    {
        return "Move";
    }
    
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        var targetCountAtPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtPosition,
        };
    }
}
