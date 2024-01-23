using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    
    private GridObject _gridObject;
    
    public void SetGridObject(GridObject gridObject)
    {
        _gridObject = gridObject;
    }
    
    private void Update()
    {
        text.text = _gridObject.ToString();
    }
}
