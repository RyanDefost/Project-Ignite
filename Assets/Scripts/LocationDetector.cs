using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationDetector : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private GameObject canvas;

    [Space]
    [SerializeField] private float _activateDistance;

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
        SetDistanceVolume();
    }
    public void TrySolveObject()
    {
        if (_distance <= _activateDistance && !_levelFinished)
        {
            _source.Play();
            StartCoroutine(SetFinalPosition());
        }
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
            _sceneSwitcher.SetScene("EndScene");
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

    private float ComparePositions(Transform cameraTransform, Transform destinationTransform)
    {
        Vector3 cameraPosition = cameraTransform.position;
        Vector3 destinationPosition = destinationTransform.position;

        return Vector3.Distance(cameraPosition, destinationPosition);
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

        for (float i = 0; i <= 1; i += 0.1f)
        {
            yield return new WaitForSeconds(0.01f);

            Quaternion lerpRotation = Quaternion.Lerp(currentCameraRotation, _destinations[_currentDestination].transform.rotation, i);
            _cameraMovement.SetCenterRotation(lerpRotation);
        }
        _cameraMovement.SetCenterRotation(_destinations[_currentDestination].transform.rotation);

        //Activate the UI when object is found.
        yield return new WaitForSeconds(1f);
        _FoundTexture[_currentDestination].SetActive(true);
        canvas.SetActive(true);
    }

    /// <summary>
    /// Based on the maxValue sets the music volume louder or quieter based on the distance.
    /// </summary>
    private void SetDistanceVolume()
    {
        float maxValue = 20;

        if (_distance < maxValue)
            _destinationObjects[_currentDestination].GetComponent<AudioSource>().volume = 0;

        float distancePercentage = (_distance / maxValue) * 100;
        float SoundPercentage = 100 - distancePercentage;

        _destinationObjects[_currentDestination].GetComponent<AudioSource>().volume = SoundPercentage / 100;
    }

}