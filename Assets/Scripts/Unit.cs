using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    private Vector3 _targetPosition;
    private const string IsWalking = "IsWalking";
    private bool _isWalking;

    private void Update()
    {
        const float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            const float moveSpeed = 4f; 
            var moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);

            const float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime*rotateSpeed);
            
            _isWalking = true;
        } 
        else
            _isWalking = false;
        
        unitAnimator.SetBool(IsWalking, _isWalking);

        if (Input.GetMouseButtonDown(0))
            Move(MouseWorld.GetPosition());
    }

    private void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
