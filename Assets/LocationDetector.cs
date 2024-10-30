using System.Collections;
using UnityEngine;

public class LocationDetector : MonoBehaviour
{
    [SerializeField] private float _activateDistance;
    [SerializeField] private GameObject destination;

    private Camera _camera;
    private CameraMovement _cameraMovement;

    private Coroutine waitngCoroutine = null;
    private bool _isWaiting;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _cameraMovement = GetComponent<CameraMovement>();
    }

    void FixedUpdate()
    {
        float distance = ComparePositions(_camera.transform, destination.transform);

        //Checks if the distance between the camera and final point is close enough
        if (distance <= _activateDistance && !_isWaiting)
        {
            _isWaiting = true;
            waitngCoroutine = StartCoroutine(Wait());
        }
        //Stops the coroutine if outside of the range.
        if (distance > _activateDistance && _cameraMovement.CanReceiveInput)
        {
            if (waitngCoroutine == null)
                return;

            StopCoroutine(waitngCoroutine);
            _isWaiting = false;
        }
    }

    private float ComparePositions(Transform cameraTransform, Transform destinationTransform)
    {
        Vector3 cameraPosition = cameraTransform.position;
        Vector3 destinationPosition = destinationTransform.position;

        return Vector3.Distance(cameraPosition, destinationPosition);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);

        StartCoroutine(SetFinalPosition());
    }

    /// <summary>
    /// Rotates the camera to the correct position by lerping to that location.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetFinalPosition()
    {
        Quaternion currentCameraRotation = _cameraMovement.GetCenterRotation();
        _cameraMovement.CanReceiveInput = false;

        for (float i = 0; i <= 1; i += 0.1f)
        {
            yield return new WaitForSeconds(0.01f);

            Quaternion lerpRotation = Quaternion.Lerp(currentCameraRotation, destination.transform.rotation, i);
            _cameraMovement.SetCenterRotation(lerpRotation);
        }
    }
}