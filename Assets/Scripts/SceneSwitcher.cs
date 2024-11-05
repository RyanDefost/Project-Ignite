using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake() => DontDestroyOnLoad(this);
    private void Start() => _audioSource = GetComponent<AudioSource>();

    public void SetScene(string sceneName)
    {
        _audioSource.Play();

        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame() => Application.Quit();
}
