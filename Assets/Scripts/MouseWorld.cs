using System;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld _instance;

    [SerializeField] private LayerMask mousePlaneLayerMask;
    private void Awake()
    {
        _instance = this;
    }

    public static Vector3 GetPosition()
    {
        var ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out var raycastHit, float.MaxValue, _instance.mousePlaneLayerMask);
        return raycastHit.point;
    }
}
