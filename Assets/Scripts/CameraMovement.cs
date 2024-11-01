using UnityEngine;

/// <summary>
/// A class that moves the camera in a orbit around a given position.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject _centerPosition;
    [Space]
    [SerializeField] private float _distance = 2;

    [Header("Speed")]
    [SerializeField] private float _mouseSpeed = 5;
    [SerializeField] private float _pitchSpeed = 5;
    [SerializeField] private float _zoomSpeed = 0.5f;

    private Camera _camera;
    private Vector2 _mousePosition;

    private Vector3 _lastPosition;
    private Vector3 _lastPitch;

    public bool CanReceiveInput = true;

    void Start() => _camera = GetComponent<Camera>();

    void FixedUpdate()
    {
        if (!CanReceiveInput)
            return;

        //Sets the distance of the camera.
        SetCameraDistanceChange();
        if (_distance != _camera.transform.position.z)
        {
            Vector3 _currentCameraDistance = _camera.transform.localPosition;
            _currentCameraDistance = new Vector3(_currentCameraDistance.x, _currentCameraDistance.y, _distance);

            _camera.transform.localPosition = _currentCameraDistance;
        }

        _mousePosition = Input.mousePosition;

        //Sets the Movement when the left mouse is pressed.
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 direction = GetCameraDiraction();
            _centerPosition.transform.Rotate(direction.x, direction.y, 0, Space.Self);
        }
        else _lastPosition = Vector3.zero;

        //Sets the pitch when the right mouse is pressed.
        if (Input.GetKey(KeyCode.Mouse1))
        {
            float pitch = GetCameraPitch();
            _centerPosition.transform.Rotate(0, 0, pitch, Space.Self);
        }
        else _lastPitch = Vector3.zero;
    }

    /// <summary>
    /// Takes the current and last position of the mouse and gives the direction the mouse is moving.
    /// </summary>
    /// <returns>Returns the normalized distance of the X and Y position as a vector3</returns>
    private Vector3 GetCameraDiraction()
    {
        Vector3 currentPosition = _mousePosition;

        if (_lastPosition != Vector3.zero)
        {
            Vector3 direction = _lastPosition - currentPosition;

            Vector3 correctedDiraction;
            correctedDiraction.x = -direction.normalized.y * _mouseSpeed;
            correctedDiraction.y = -direction.normalized.x * _mouseSpeed;
            correctedDiraction.z = 0;

            _lastPosition = currentPosition;
            return correctedDiraction;
        }

        _lastPosition = currentPosition;
        return Vector3.zero;
    }

    /// <summary>
    /// Takes the current and last position of the mouse and gives the pitch.
    /// </summary>
    /// <returns>Returns the normalized difference in X position as a float</returns>
    private float GetCameraPitch()
    {
        Vector3 currentPosition = _mousePosition;

        if (_lastPitch != Vector3.zero)
        {
            Vector3 direction = _lastPitch - currentPosition;

            float correctedPitch;
            correctedPitch = -direction.normalized.x * _pitchSpeed;

            _lastPitch = currentPosition;
            return correctedPitch;
        }

        _lastPitch = currentPosition;
        return 0;
    }

    /// <summary>
    /// Sets the distance based on the input of the scroll wheel (1 or -1).
    /// </summary>
    private void SetCameraDistanceChange()
    {
        if (Input.mouseScrollDelta == Vector2.zero)
            return;

        float scrollInput = Input.mouseScrollDelta.x + Input.mouseScrollDelta.y;

        _distance += -scrollInput * _zoomSpeed;
        _distance = Mathf.Clamp(_distance, 1, 10);
    }

    /// <summary>
    /// Gets the center rotation that the camera is rotated around to move it.
    /// </summary>
    public GameObject GetCenterObject() => _centerPosition;
    public void SetCenterRotation(Quaternion eulerAngles) => _centerPosition.transform.rotation = eulerAngles;

    public void SetDistance(float distance) => _distance = distance;
}
