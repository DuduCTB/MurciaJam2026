using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup fadeGroup;        // el CanvasGroup de FadeNegro
    public float fadeTime = 1f;
    public bool fadeInOnStart = true;    // aparece desde negro al cargar la escena
    public AudioSource[] audioToFade;    // música del menú, viento...

    void Start()
    {
        if (fadeInOnStart) StartCoroutine(Fade(1f, 0f));
        else { fadeGroup.alpha = 0f; fadeGroup.blocksRaycasts = false; }
    }

    public void FadeToScene(string sceneName) => StartCoroutine(FadeAndLoad(sceneName));

    IEnumerator FadeAndLoad(string sceneName)
    {
        float[] startVol = new float[audioToFade.Length];
        for (int i = 0; i < audioToFade.Length; i++)
            if (audioToFade[i]) startVol[i] = audioToFade[i].volume;

        fadeGroup.blocksRaycasts = true;     // evita dobles clics durante el fundido
        for (float t = 0f; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            float k = t / fadeTime;
            fadeGroup.alpha = k;
            for (int i = 0; i < audioToFade.Length; i++)
                if (audioToFade[i]) audioToFade[i].volume = startVol[i] * (1f - k);
            yield return null;
        }
        fadeGroup.alpha = 1f;
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Fade(float from, float to)
    {
        fadeGroup.blocksRaycasts = true;
        for (float t = 0f; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            fadeGroup.alpha = Mathf.Lerp(from, to, t / fadeTime);
            yield return null;
        }
        fadeGroup.alpha = to;
        fadeGroup.blocksRaycasts = to > 0.5f;
    }
}
