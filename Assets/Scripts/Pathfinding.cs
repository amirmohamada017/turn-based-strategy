using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;
    private GridSystem<PathNode> _gridSystem;
    private int _width;
    private int _height;
    private float _cellSize;

    private void Awake()
    {
        _gridSystem = new GridSystem<PathNode>(10, 10, 2f,
            (GridSystem<PathNode> gridSystem,GridPosition gridPosition) => new PathNode(gridPosition));
        _gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }
}
