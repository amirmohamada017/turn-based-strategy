using System;
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
            isActive = false;
            onActionComplete();
        }
            

    }

    public void Spin(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        _totalSpinAmount = 0f;
        isActive = true;
    }


    public override string GetActionName()
    {
        return "Spin";
    }
}
