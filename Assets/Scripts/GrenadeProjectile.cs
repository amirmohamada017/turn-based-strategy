using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;
    
    [SerializeField] private Transform grenadeExplodeVFXPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    
    private Action _onGrenadeBehaviourComplete;
    private Vector3 _targetPosition;
    private Vector3 _positionXZ;
    private float _totalDistance;

    private void Update()
    {
        const float moveSpeed = 15f;
        var moveDir = (_targetPosition - _positionXZ).normalized;
        _positionXZ += moveDir * (moveSpeed * Time.deltaTime);
        
        var distance = Vector3.Distance(_positionXZ, _targetPosition);
        var normalizedDistance = 1 - (distance / _totalDistance);
        
        var maxHeight = _totalDistance / 2f;
        var positionY = arcYAnimationCurve.Evaluate(normalizedDistance) * maxHeight;
        transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);
        
        const float reachedTargetDistance = .2f;
        if (Vector3.Distance(_positionXZ, _targetPosition) < reachedTargetDistance)
        {
            const float damageRadius = 4f;
            var colliders = Physics.OverlapSphere(_targetPosition, damageRadius);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Unit>(out var targetUnit))
                    targetUnit.Damage(30);
            }
            
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplodeVFXPrefab, _targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
            _onGrenadeBehaviourComplete();
        }
    }

    public void SetUp(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        _onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        
        _positionXZ = transform.position;
        _positionXZ.y = 0f;
        
        _totalDistance = Vector3.Distance(_positionXZ, _targetPosition);
    }
}
