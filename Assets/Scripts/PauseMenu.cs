using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject pauseButtons;
    public GameObject optionsPanel;
    public MonoBehaviour[] disableOnPause;
    public string menuScene = "MainMenu";

    bool paused;

    void Start()
    {
        pausePanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;

        if (!paused) Pause();
        else if (optionsPanel && optionsPanel.activeSelf) CloseOptions();   // Esc en opciones = volver
        else Resume();
    }

    public void Pause()
    {
        paused = true;
        pausePanel.SetActive(true);
        if (pauseButtons) pauseButtons.SetActive(true);
        if (optionsPanel) optionsPanel.SetActive(false);
        Time.timeScale = 0f;
        foreach (var m in disableOnPause) if (m) m.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        paused = false;
        pausePanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        foreach (var m in disableOnPause) if (m) m.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenOptions()
    {
        pauseButtons.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pauseButtons.SetActive(true);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
