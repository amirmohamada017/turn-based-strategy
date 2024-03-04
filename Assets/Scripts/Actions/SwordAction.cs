using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static event EventHandler OnAnySwordHit;
    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;
    
    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit
    }
    
    private const int MaxSwordDistance = 1;
    private State _state;
    private float _stateTimer;
    private Unit _targetUnit;
    

    private void Update()
    {
        if (!isActive) return;
        _stateTimer -= Time.deltaTime;
        
        if (_stateTimer <= 0f)
            NextState();
    }
    
    private void NextState()
    {
        switch (_state)
        {
            case State.SwingingSwordBeforeHit:
                _stateTimer = 0.5f;
                _state = State.SwingingSwordAfterHit;
                
                const float rotateSpeed = 10f;
                var aimDir = (_targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                
                _targetUnit.Damage(100);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
            default:
                break;
        }
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        _state = State.SwingingSwordBeforeHit;
        _stateTimer = 0.7f;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        var validGridPositions = new List<GridPosition>();
        var unitGridPosition = unit.GetGridPosition();

        for (var x = -MaxSwordDistance; x <= MaxSwordDistance; x++)
        {
            for (var z = -MaxSwordDistance; z <= MaxSwordDistance; z++)
            {
                var offsetGridPosition = new GridPosition(x, z);
                var testGridPosition = unitGridPosition + offsetGridPosition;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;
                
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;

                var targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                    continue;

                validGridPositions.Add(testGridPosition);
            }
        }
        
        return validGridPositions;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 250
        };
    }

    public int GetMaxSwordDistance()
    {
        return MaxSwordDistance;
    }
}
