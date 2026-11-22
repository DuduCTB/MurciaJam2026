using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSliders : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider music, sfx, amb;

    [Header("Volumen del mixer con el slider al máximo (dB)")]
    public float musicBase = -12f;
    public float sfxBase = 0f;
    public float ambBase = -6f;

    void Start()
    {
        Setup(music, "MusicVol", musicBase);
        Setup(sfx, "SfxVol", sfxBase);
        Setup(amb, "AmbVol", ambBase);
    }

    void Setup(Slider s, string param, float baseDb)
    {
        float v = PlayerPrefs.GetFloat(param, 1f);
        Apply(param, baseDb, v);
        if (!s) return;
        s.minValue = 0.0001f;
        s.maxValue = 1f;
        s.value = v;
        s.onValueChanged.AddListener(x =>
        {
            Apply(param, baseDb, x);
            PlayerPrefs.SetFloat(param, x);
        });
    }

    void Apply(string param, float baseDb, float v) =>
        mixer.SetFloat(param, baseDb + Mathf.Log10(v) * 20f);
}
