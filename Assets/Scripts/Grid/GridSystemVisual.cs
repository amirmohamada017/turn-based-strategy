using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    
    [SerializeField] private Transform gridSystemSingleVisualPrefab;

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
    }

    private void Update()
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

    private void ShowGridPositions(List<GridPosition> gridPositions)
    {
        foreach (var gridPosition in gridPositions)
        {
            _gridSystemVisualSingles[gridPosition.X, gridPosition.Z].Show();
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPositions();
        var action = UnitActionSystem.Instance.GetSelectedAction();
        ShowGridPositions(action.GetValidActionGridPositions());
    }
}
