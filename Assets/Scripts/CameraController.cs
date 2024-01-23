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
        var inputMoveDir = new Vector3(0, 0, 0);
        
        if (Input.GetKey(KeyCode.W)) inputMoveDir.z++;
        if (Input.GetKey(KeyCode.S)) inputMoveDir.z--;
        if (Input.GetKey(KeyCode.A)) inputMoveDir.x--;
        if (Input.GetKey(KeyCode.D)) inputMoveDir.x++;
        
        const float moveSpeed = 10f;
        var moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * (moveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        var inputRotateDir = new Vector3(0, 0, 0);
        
        if (Input.GetKey(KeyCode.E)) inputRotateDir.y--;
        if (Input.GetKey(KeyCode.Q)) inputRotateDir.y++;
        
        const float rotateSpeed = 100f;
        transform.Rotate(inputRotateDir * (rotateSpeed * Time.deltaTime));

        _transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _transposer.m_FollowOffset;
    }

    private void Zoom()
    {
        if (Input.mouseScrollDelta.y > 0 && _transposer.m_FollowOffset.y > MinFollowOffset)
            _targetFollowOffset.y--;
        else if (Input.mouseScrollDelta.y < 0 && _transposer.m_FollowOffset.y < MaxFollowOffset)
            _targetFollowOffset.y++;
        
        
        const float zoomSpeed = 30f;
        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MinFollowOffset, MaxFollowOffset);
        _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset,
            _targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
