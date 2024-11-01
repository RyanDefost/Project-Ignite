using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationDetector : MonoBehaviour
{
    [SerializeField] private float _activateDistance;

    [SerializeField] private GameObject canvas;
    [SerializeField] private List<GameObject> _destinationObjects;
    [SerializeField] private List<GameObject> _destinations;

    private Camera _camera;
    private CameraMovement _cameraMovement;

    private Coroutine waitngCoroutine = null;
    private bool _isWaiting;

    private bool _levelFinished = false;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _cameraMovement = GetComponent<CameraMovement>();

    }

    void FixedUpdate()
    {
        if (_levelFinished)
            return;

        float distance = ComparePositions(_camera.transform, _destinations[0].transform);

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
        Quaternion currentCameraRotation = _cameraMovement.GetCenterObject().transform.rotation;
        _cameraMovement.CanReceiveInput = false;

        for (float i = 0; i <= 1; i += 0.1f)
        {
            yield return new WaitForSeconds(0.01f);

            Quaternion lerpRotation = Quaternion.Lerp(currentCameraRotation, _destinations[0].transform.rotation, i);
            _cameraMovement.SetCenterRotation(lerpRotation);
        }

        //Activate the UI when object is found.
        yield return new WaitForSeconds(1f);
        canvas.SetActive(true);
    }

    public void LockPositionSnap()
    {
        _levelFinished = true;
        _cameraMovement.CanReceiveInput = true;
    }
}