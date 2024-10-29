using UnityEngine;

public class LocationDetector : MonoBehaviour
{
    [SerializeField] private float _activateDistance;

    [SerializeField] private GameObject destination;
    private Camera _camera;
    private CameraMovement _cameraMovement;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _cameraMovement = GetComponent<CameraMovement>();
    }

    void Update()
    {
        float distance = ComparePositions(_camera.transform, destination.transform);

        if (distance < _activateDistance)
            WaitAndSetPosition();
    }

    private float ComparePositions(Transform cameraTransform, Transform destinationTransform)
    {
        Vector3 cameraRotation = cameraTransform.rotation.eulerAngles;
        Vector3 destinationRotation = destinationTransform.rotation.eulerAngles;

        return Vector3.Distance(cameraRotation, destinationRotation);
    }

    private void WaitAndSetPosition()
    {
        _cameraMovement.CanReceiveInput = false;

        for (float i = 0; i < 1; i += 0.1f)
            _camera.transform.rotation =
                Quaternion.Lerp(_camera.transform.rotation, destination.transform.rotation, 10f);
    }
}
