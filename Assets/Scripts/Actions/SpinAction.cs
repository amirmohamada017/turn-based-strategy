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
            isActive = false;

    }

    public void Spin()
    {
        isActive = true;
        _totalSpinAmount = 0f;
    }
}
