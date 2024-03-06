using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;
    
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction _selectedAction;
    private bool _isBusy;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (_isBusy) return;
        
        if (!TurnSystem.Instance.IsPlayerTurn()) return;
        
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (HandleUnitSelection()) return;
        
        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            var mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!_selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;

            if (!selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction)) return;
            
            SetBusy();
            _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetBusy()
    {
        _isBusy = true;
        OnBusyChanged?.Invoke(this, _isBusy);
        
    }

    private void ClearBusy()
    {
        _isBusy = false;
        OnBusyChanged?.Invoke(this, _isBusy);
    }
    
    private bool HandleUnitSelection()
    {
        if (!InputManager.Instance.IsMouseButtonDownThisFrame())
            return false;
        
        var ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());

        if (Physics.Raycast(ray, out var raycastHit, float.MaxValue, unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out var unit))
            {
                if (unit == selectedUnit)
                    return false;

                if (unit.IsEnemy())
                    return false;

                SetSelectedUnit(unit);
                return true;
            }
        }
        
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public void SetSelectedAction(BaseAction action)
    {
        _selectedAction = action;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public BaseAction GetSelectedAction()
    {
        return _selectedAction;
    }
}

