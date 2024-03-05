using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    private const int MaxInteractDistance = 1;
    
    public override string GetActionName()
    {
        return "Interact";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        var door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
        door.Interact(OnInteractComplete);
        ActionStart(onActionComplete);
    }
    
    private void OnInteractComplete()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        var validPositions = new List<GridPosition>();
        var unitGridPosition = unit.GetGridPosition();
        
        for (var x = -MaxInteractDistance; x <= MaxInteractDistance; x++)
        {
            for (var z = -MaxInteractDistance; z <= MaxInteractDistance; z++)
            {
                var testGridPosition = new GridPosition(unitGridPosition.X + x, unitGridPosition.Z + z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;
                
                var door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);
                if (door == null)
                    continue;
                
                validPositions.Add(testGridPosition);
            }
        }
        
        return validPositions;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction()
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }
}
