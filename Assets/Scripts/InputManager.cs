#define USE_NEW_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        Instance = this;
        
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }
    
    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }
    
    public bool IsMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return _playerInputActions.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return _playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        var inputMoveDir = new Vector2(0,0);
        
        if (Input.GetKey(KeyCode.W)) inputMoveDir.y++;
        if (Input.GetKey(KeyCode.S)) inputMoveDir.y--;
        if (Input.GetKey(KeyCode.A)) inputMoveDir.x--;
        if (Input.GetKey(KeyCode.D)) inputMoveDir.x++;
        
        return inputMoveDir;
#endif
    }
    
    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return _playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
        var rotateAmount = 0f;
        
        if (Input.GetKey(KeyCode.Q)) rotateAmount++;
        if (Input.GetKey(KeyCode.E)) rotateAmount--;
        
        return rotateAmount;
#endif
    }
    
    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return _playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
        var zoomAmount = 0f;
        
        if (Input.mouseScrollDelta.y < 0) zoomAmount++;
        if (Input.mouseScrollDelta.y > 0) zoomAmount--;
        
        return zoomAmount;
#endif

    }
}
