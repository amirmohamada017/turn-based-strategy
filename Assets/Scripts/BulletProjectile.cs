using System;
using Unity.Mathematics;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVfxPrefab;
    
    private Vector3 _targetPosition;
    
    public void SetUp(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        var moveDir = (_targetPosition - transform.position).normalized;
        const float moveSpeed = 200f;

        var distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);

        transform.position += moveDir * (moveSpeed * Time.deltaTime);
        
        var distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = _targetPosition;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletHitVfxPrefab, _targetPosition, Quaternion.identity);
        }
    }
}
