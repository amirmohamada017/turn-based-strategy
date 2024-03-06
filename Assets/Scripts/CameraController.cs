using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    
    private const float MinFollowOffset = 1f;
    private const float MaxFollowOffset = 15f;
    private CinemachineTransposer _transposer;
    private Vector3 _targetFollowOffset;


    private void Start()
    {
        _transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _transposer.m_FollowOffset;
    }

    private void Update()
    {
        Move();
        Rotate();
        Zoom();
    }

    private void Move()
    {
        var inputMoveDir = InputManager.Instance.GetCameraMoveVector();
        var moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        const float moveSpeed = 10f;
        transform.position += moveVector * (moveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        var rotationVector = new Vector3(0, 0, 0);
        rotationVector.y += InputManager.Instance.GetCameraRotateAmount();
        
        const float rotateSpeed = 100f;
        transform.Rotate(rotationVector * (rotateSpeed * Time.deltaTime));

        _transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _transposer.m_FollowOffset;
    }

    private void Zoom()
    {
        const float zoomIncreaseAmount = 1f;
        _targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;
        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MinFollowOffset, MaxFollowOffset);
        
        const float zoomSpeed = 30f;
        
        _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset,
            _targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
