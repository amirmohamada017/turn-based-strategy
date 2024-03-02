using System;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyCrateDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;
    
    private GridPosition _gridPosition;

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }
    
    public void Damage()
    {
        var crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        ApplyExplosionToChildren(crateDestroyedTransform, 100f, transform.position, 10f);
        
        Destroy(gameObject);
        OnAnyCrateDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
    
    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition,
        float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out var childRigidbody))
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
