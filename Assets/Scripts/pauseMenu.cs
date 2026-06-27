using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class pauseMenu : MonoBehaviour
{
    public GameObject PauseMenu;
    public InputAction pauseControl;
    bool read = true;
    void Start()
    {
        Time.timeScale = 1;
    }
    void Update()
    {
        if(pauseControl.ReadValue<float>() == 1 && read)
        {
            PauseButtonPressed();
            read = false;
        } else if(pauseControl.ReadValue<float>() == 0)
        {
            read = true;
        }
    }

    void OnEnable()
    {
        pauseControl.Enable();
    }
    void OnDisable()
    {
        pauseControl.Disable();
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
