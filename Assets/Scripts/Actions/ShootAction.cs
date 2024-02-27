using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }
    
    private enum State
    {
        Aiming,
        Shooting,
        CoolOff,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    
    private const int MaxShootDistance = 6;
    private const float AimingStateTime = 1f;
    private const float ShootingStateTime = .1f;
    private const float CoolOffStateTime = .5f;
    private const int DamageAmount = 40;
    private Unit _targetUnit;
    private State _state;
    private float _stateTimer;
    private bool _canShootBullet;


    private void Update()
    {
        if (!isActive)
            return;

        _stateTimer -= Time.deltaTime;

        switch (_state)
        {
            case State.Aiming:
                const float rotateSpeed = 10f;
                var aimDir = (_targetUnit.GetWorldPosition()) - (unit.GetWorldPosition());
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime*rotateSpeed);
                break;
            case State.Shooting:
                if (_canShootBullet)
                {
                    Shoot();
                    _canShootBullet = false;
                }
                break;
            case State.CoolOff:
                break;
        }
        
        if (_stateTimer < 0f)
            NextState();
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = _targetUnit,
            shootingUnit = unit
        });
        _targetUnit.Damage(DamageAmount);
    }

    private void NextState()
    {
        switch (_state)
        {
            case State.Aiming:
                _state = State.Shooting;
                _stateTimer = ShootingStateTime;
                break;
            case State.Shooting:
                _state = State.CoolOff;
                _stateTimer = CoolOffStateTime;
                break;
            case State.CoolOff:
                ActionComplete();
                break;
        }
    }
    
    public override string GetActionName()
    {
        return "Shoot";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        _state = State.Aiming;
        _stateTimer = AimingStateTime;
        _canShootBullet = true;
        
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        var unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositions(unitGridPosition);
    }

    private List<GridPosition> GetValidActionGridPositions(GridPosition unitGridPosition)
    {
        var validGridPositions = new List<GridPosition>();

        for (var x = -MaxShootDistance; x <= MaxShootDistance; x++)
        {
            for (var z = -MaxShootDistance; z <= MaxShootDistance; z++)
            {
                var testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (MaxShootDistance < testDistance)
                    continue;
                
                var offsetGridPosition = new GridPosition(x, z);
                var testGridPosition = unitGridPosition + offsetGridPosition;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;
                
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;

                var targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                    continue;

                const float unitShoulderHeight = 1.7f;
                var unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                var shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir,
                        Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()), obstaclesLayerMask))
                    continue;
                
                validGridPositions.Add(testGridPosition);
            }
        }
        
        return validGridPositions;
    }

    public Unit GetTargetUnit()
    {
        return _targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return MaxShootDistance;
    }
    
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        var targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositions(gridPosition).Count;
    }
}