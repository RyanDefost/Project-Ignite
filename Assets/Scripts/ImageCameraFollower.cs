using UnityEngine;

public class ImageCameraFollower : MonoBehaviour
{
    private Camera _camera;

    void Start() => _camera = FindObjectOfType<Camera>();

    void Update() => transform.LookAt(_camera.transform);
}
