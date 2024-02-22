using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    
    private object _gridObject;
    
    public virtual void SetGridObject(object gridObject)
    {
        _gridObject = gridObject;
    }
    
    protected virtual void Update()
    {
        text.text = _gridObject.ToString();
    }
}
