using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Vector3 _targetPosition;
    private Action _onGrenadeBehaviourComplete;

    private void Update()
    {
        const float moveSpeed = 15f;
        var moveDir = (_targetPosition - transform.position).normalized;
        transform.position += moveDir * (moveSpeed * Time.deltaTime);

        const float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, _targetPosition) < reachedTargetDistance)
        {
            const float damageRadius = 4f;
            var colliders = Physics.OverlapSphere(_targetPosition, damageRadius);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Unit>(out var targetUnit))
                    targetUnit.Damage(30);
            }
            
            Destroy(gameObject);
            _onGrenadeBehaviourComplete();
        }
    }

    public void SetUp(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        _onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
    }
}
