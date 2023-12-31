using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 _targetPosition;

    private void Update()
    {
        const float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            var moveDirection = (_targetPosition - transform.position).normalized; 
            const float moveSpeed = 4f; 
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0))
            Move(MouseWorld.GetPosition());
    }

    private void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
