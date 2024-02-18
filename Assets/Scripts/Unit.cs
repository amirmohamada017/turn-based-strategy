using System;
using UnityEngine;

public class Unit : MonoBehaviour
{ 
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private bool isEnemy;
    
    private const int ActionPointsMax = 2;
    private HealthSystem _healthSystem;
    private GridPosition _gridPosition;
    private BaseAction[] _actions;
    private int _actionPoints = ActionPointsMax;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _actions = GetComponents<BaseAction>();
    }

    private void Update()
    { 
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            var oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        _healthSystem.OnDead += HealthSystem_OnDead;
        
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }
    
    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public BaseAction[] GetActions()
    {
        return _actions;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction action)
    {
        if (CanSpendActionPointsToTakeAction(action))
        {
            SpendActionPoints(action.GetActionPointsCost());
            return true;
        }

        return false;
    }
    
    public bool CanSpendActionPointsToTakeAction(BaseAction action)
    {
        if (_actionPoints >= action.GetActionPointsCost())
        {
            return true;
        }

        return false;
    }

    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return _actionPoints;
    }
    
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        var isPlayerTurn = TurnSystem.Instance.IsPlayerTurn();
        if ((IsEnemy() && !isPlayerTurn) || (!IsEnemy() && isPlayerTurn))
        {
            _actionPoints = ActionPointsMax;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
        
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        _healthSystem.Damage(damageAmount);
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return _healthSystem.GetHealthNormalized();
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (var action in _actions)
        {
            if (action is T)
                return (T)action;
        }

        return null;
    }
}


