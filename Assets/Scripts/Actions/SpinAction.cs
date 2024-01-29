using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;

    private void Update()
    {
        if (!isActive) return;
        
        var spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        _totalSpinAmount += spinAddAmount;
        if (_totalSpinAmount >= 360f)
        {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        
        _totalSpinAmount = 0f;
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        return new List<GridPosition>
        {
            unit.GetGridPosition()
        };
    }


    public override string GetActionName()
    {
        return "Spin";
    }
}
