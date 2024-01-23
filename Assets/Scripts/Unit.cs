using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    
    private Vector3 _targetPosition;
    private GridPosition _gridPosition;
    
    private const string IsWalking = "IsWalking";
    private bool _isWalking;

    private void Awake()
    {
        _targetPosition = transform.position;
    }

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
        
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }

    public void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
