using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationDetector : MonoBehaviour
{
    [SerializeField] private float _activateDistance;

    [SerializeField] private GameObject canvas;

    [SerializeField] private List<GameObject> _FoundTexture;
    [SerializeField] private List<GameObject> _destinationObjects;
    [SerializeField] private List<GameObject> _destinations;
    private int _currentDestination = 0;

    private SceneSwitcher _sceneSwitcher;

    private Camera _camera;
    private CameraMovement _cameraMovement;

    private float _distance;

    private bool _levelFinished = false;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _cameraMovement = GetComponent<CameraMovement>();
        _sceneSwitcher = FindAnyObjectByType<SceneSwitcher>();
    }

    void FixedUpdate()
    {
        if (_levelFinished)
            return;

        _distance = ComparePositions(_camera.transform, _destinations[_currentDestination].transform);
    }

    private float ComparePositions(Transform cameraTransform, Transform destinationTransform)
    {
        Vector3 cameraPosition = cameraTransform.position;
        Vector3 destinationPosition = destinationTransform.position;

        return Vector3.Distance(cameraPosition, destinationPosition);
    }

    public void TrySolveObject()
    {
        if (_distance <= _activateDistance && !_levelFinished)
            StartCoroutine(SetFinalPosition());
    }

    /// <summary>
    /// Rotates the camera to the correct position by lerping to that location.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetFinalPosition()
    {
        _levelFinished = true;

        Quaternion currentCameraRotation = _cameraMovement.GetCenterObject().transform.rotation;
        _cameraMovement.CanReceiveInput = false;

        for (float i = 0; i < 1; i += 0.1f)
        {
            yield return new WaitForSeconds(0.01f);

            Quaternion lerpRotation = Quaternion.Lerp(currentCameraRotation, _destinations[_currentDestination].transform.rotation, i);
            _cameraMovement.SetCenterRotation(lerpRotation);
        }

        //Activate the UI when object is found.
        yield return new WaitForSeconds(1f);
        _FoundTexture[_currentDestination].SetActive(true);
        canvas.SetActive(true);
    }

    public void LockPositionSnap()
    {
        _levelFinished = true;
        _cameraMovement.CanReceiveInput = true;

        _FoundTexture[_currentDestination].SetActive(false);
    }

    public void SetNextObject()
    {
        if (_currentDestination == _destinationObjects.Count - 1)
        {
            _sceneSwitcher.SetFinalScene();
            return;
        }

        _FoundTexture[_currentDestination].SetActive(false);
        _destinationObjects[_currentDestination].SetActive(false);
        _destinationObjects[_currentDestination + 1].SetActive(true);
        _currentDestination++;

        canvas.SetActive(false);
        _cameraMovement.CanReceiveInput = true;
        _levelFinished = false;
    }
}