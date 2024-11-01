using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SetFinalScene() => SceneManager.LoadScene("endScene");
    public void SetMainScene() => SceneManager.LoadScene("MainScene");
    public void QuitGame() => Application.Quit();
}
