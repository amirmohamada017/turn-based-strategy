using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    public Vector2 GetMouseScreenPosition()
    {
        return Input.mousePosition;
    }
    
    public bool IsMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public Vector2 GetCameraMoveVector()
    {
        var inputMoveDir = new Vector2(0,0);
        
        if (Input.GetKey(KeyCode.W)) inputMoveDir.y++;
        if (Input.GetKey(KeyCode.S)) inputMoveDir.y--;
        if (Input.GetKey(KeyCode.A)) inputMoveDir.x--;
        if (Input.GetKey(KeyCode.D)) inputMoveDir.x++;
        
        return inputMoveDir;
    }
    
    public float GetCameraRotateAmount()
    {
        var rotateAmount = 0f;
        
        if (Input.GetKey(KeyCode.Q)) rotateAmount++;
        if (Input.GetKey(KeyCode.E)) rotateAmount--;
        
        return rotateAmount;
    }
    
    public float GetCameraZoomAmount()
    {
        var zoomAmount = 0f;
        
        if (Input.mouseScrollDelta.y < 0) zoomAmount++;
        if (Input.mouseScrollDelta.y > 0) zoomAmount--;
        
        return zoomAmount;
    }
}
