using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public MonoBehaviour[] disableOnPause;   // scripts de cámara y movimiento del Player
    public string menuScene = "MainMenu";
    public AudioMixer mixer;

    bool paused;
    float musicBase, sfxBase, ambBase;

    void Start()
    {
        pausePanel.SetActive(false);
        mixer.GetFloat("MusicVol", out musicBase);
        mixer.GetFloat("SfxVol", out sfxBase);
        mixer.GetFloat("AmbVol", out ambBase);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (paused) Resume(); else Pause();
        }
    }

    public void Pause()
    {
        paused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        foreach (var m in disableOnPause) if (m) m.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        paused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        foreach (var m in disableOnPause) if (m) m.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

    // Sliders: Min 0.0001, Max 1. Con 1 se queda el volumen que ya tenías en el mixer
    public void SetMusic(float v) => mixer.SetFloat("MusicVol", musicBase + Mathf.Log10(v) * 20f);
    public void SetSfx(float v) => mixer.SetFloat("SfxVol", sfxBase + Mathf.Log10(v) * 20f);
    public void SetAmb(float v) => mixer.SetFloat("AmbVol", ambBase + Mathf.Log10(v) * 20f);
}
