using UnityEngine;
using UnityEngine.SceneManagement;
public class pauseMenu : MonoBehaviour
{
    public GameObject PauseMenu;

    void Start()
    {
        Time.timeScale = 1;
    }
    public void PauseButtonPressed()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);
        Time.timeScale = PauseMenu.activeSelf ? 0 : 1;
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    public void Menu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
