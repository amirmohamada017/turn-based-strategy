using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }
    
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Yellow,
        RedSoft
    }
    
    [SerializeField] private Transform gridSystemSingleVisualPrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;

    private GridSystemVisualSingle[,] _gridSystemVisualSingles;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var width = LevelGrid.Instance.GetWidth();
        var height = LevelGrid.Instance.GetHeight();
        
        _gridSystemVisualSingles = new GridSystemVisualSingle[width, height];
        for (var x = 0; x < width; x++)
        {
            for (var z = 0; z < height; z++)
            {
                var gridPosition = new GridPosition(x, z);
                var gridSystemVisualSingleTransform = Instantiate(gridSystemSingleVisualPrefab, 
                    LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                _gridSystemVisualSingles[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
        
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void HideAllGridPositions()
    {
        var width = LevelGrid.Instance.GetWidth();
        var height = LevelGrid.Instance.GetHeight();

        
        for (var x = 0; x < width; x++)
        {
            for (var z = 0; z < height; z++)
            {
                _gridSystemVisualSingles[x, z].Hide();
            }
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        var gridPositions = new List<GridPosition>();
        
        for (var x = -range; x <= range; x++)
        {
            for (var z = -range; z <= range; z++)
            {
                var offsetGridPosition = new GridPosition(x, z);
                var testGridPosition = gridPosition + offsetGridPosition;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                var testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (range < testDistance)
                {
                    continue;
                }
                
                gridPositions.Add(testGridPosition);
            }
        }
        
        ShowGridPositions(gridPositions, gridVisualType);
    }

    private void ShowGridPositionRangeDiagonal(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        var gridPositions = new List<GridPosition>();
        
        for (var x = -range; x <= range; x++)
        {
            for (var z = -range; z <= range; z++)
            {
                var offsetGridPosition = new GridPosition(x, z);
                var testGridPosition = gridPosition + offsetGridPosition;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;
                
                gridPositions.Add(testGridPosition);
            }
        }
        
        ShowGridPositions(gridPositions, gridVisualType);
    }
    
    private void ShowGridPositions(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach (var gridPosition in gridPositions)
        {
            _gridSystemVisualSingles[gridPosition.X, gridPosition.Z].Show(GetGridVisualMaterial(gridVisualType));
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPositions();

        var unit = UnitActionSystem.Instance.GetSelectedUnit();
        var action = UnitActionSystem.Instance.GetSelectedAction();

        var gridVisualType = action switch
        {
            MoveAction moveAction => GridVisualType.White,
            ShootAction shootAction => GridVisualType.Blue,
            SpinAction spinAction => GridVisualType.Red,
            _ => GridVisualType.White
        };

        switch (action)
        {
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(unit.GetGridPosition(), shootAction.GetMaxShootDistance(),
                    GridVisualType.RedSoft);
                break;
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeDiagonal(unit.GetGridPosition(), swordAction.GetMaxSwordDistance(),
                    GridVisualType.RedSoft);
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            default:
                gridVisualType = GridVisualType.Yellow;
                break;
        }

        ShowGridPositions(action.GetValidActionGridPositions(), gridVisualType);
    }

    private Material GetGridVisualMaterial(GridVisualType gridVisualType)
    {
        foreach (var gridVisualTypeMaterial in gridVisualTypeMaterials)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
                return gridVisualTypeMaterial.material;
        }

        return null;
    }
}
