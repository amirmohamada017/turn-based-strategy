using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;
    
    private Vector3 _targetPosition;
    
    private const string IsWalking = "IsWalking";
    private bool _isWalking;
    
    protected override void Awake()
    {
        base.Awake();
        _targetPosition = transform.position;
    }
    
    private void Update()
    {
        if (!isActive) return;
        
        const float stoppingDistance = .1f;
        var moveDirection = (_targetPosition - transform.position).normalized;
        
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            const float moveSpeed = 4f; 
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            
            _isWalking = true;
        }
        else
        {
            _isWalking = false;
            ActionComplete();
        }

        const float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
        
        unitAnimator.SetBool(IsWalking, _isWalking);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        
        _targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
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
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                
                validGridPositions.Add(testGridPosition);
            }
        }
        
        return validGridPositions;
    }
    
    public override string GetActionName()
    {
        return "Move";
    }
}
