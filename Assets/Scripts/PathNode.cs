public class PathNode
{
    private GridPosition _gridPosition;
    private PathNode _cameFromPathNode;
    private int _gCost;
    private int _hCost;
    private int _fCost;
    
    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return _gridPosition.ToString();
    }

    public int GetGCost()
    {
        return _gCost;
    }
    
    public int GetHCost()
    {
        return _hCost;
    }
    
    public int GetFCost()
    {
        return _fCost;
    }
}
