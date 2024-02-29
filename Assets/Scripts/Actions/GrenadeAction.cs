using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private Transform grenadeProjectilePrefab;
    
    private const int MaxThrowDistance = 8;
    private void Update()
    {
        if (!isActive) return;
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        var grenadeProjectileTransform =
            Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        var grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.SetUp(gridPosition, OnGrenadeBehaviourComplete);
        
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        var validGridPositions = new List<GridPosition>();
        var unitGridPosition = unit.GetGridPosition();

        for (var x = -MaxThrowDistance; x <= MaxThrowDistance; x++)
        {
            for (var z = -MaxThrowDistance; z <= MaxThrowDistance; z++)
            {
                var testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (MaxThrowDistance < testDistance)
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

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction()
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }
}
