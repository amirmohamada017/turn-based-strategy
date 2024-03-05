using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool _isOpen = false;
    private bool _isActive;
    private Animator _animator;
    private GridPosition _gridPosition;
    private Action _onInteractComplete;
    private float _timer;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(_gridPosition, this);
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, _isOpen);
    }

    private void Update()
    {
        if (!_isActive) return;
        
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _isActive = false;
            _onInteractComplete?.Invoke();
        }
    }

    public void Interact(Action onInteractComplete)
    {
        _isActive = true;
        _timer = .5f;
        _onInteractComplete = onInteractComplete;
        _isOpen = !_isOpen;
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, _isOpen);
        _animator.SetBool("IsOpen", _isOpen);
    }
}
