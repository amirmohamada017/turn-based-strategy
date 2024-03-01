using System;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyCrateDestroyed;

    private GridPosition _gridPosition;

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }
    
    public void Damage()
    {
        Destroy(gameObject);
        OnAnyCrateDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
}
