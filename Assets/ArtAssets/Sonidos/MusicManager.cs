using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Pistas (Play On Awake ✗ en todas)")]
    public AudioSource calmIntroSource;   // MusicaRelaxInicio  (Loop ✗)
    public AudioSource calmSource;        // MusicaRelaxLoop    (Loop ✓)
    public AudioSource emergencySource;   // LoopMusicaTension  (Loop ✓)
    public AudioSource uneasySource;      // MusicaInquietud    (Loop ✓)

    [Header("Sustos y cierres")]
    public AudioSource stingerSource;     // Output SFX
    public AudioClip[] appearanceStingers;   // Primera, Segunda, Tercera, Cuarta
    public AudioClip tensionEnd;             // FinalAudioTension
    public float silenceAfterFirstStinger = 1f;

    [Header("Transiciones")]
    public float fadeTime = 2f;
    public float chaseFadeTime = 0.5f;

    AudioSource current;
    float calmVol, emergencyVol, uneasyVol;
    int appearances;
    Coroutine routine;

    void Awake()
    {
        Instance = this;
        calmVol = calmSource.volume;
        emergencyVol = emergencySource.volume;
        uneasyVol = uneasySource.volume;
    }

    void Start()
    {
        current = calmSource;
        double t = AudioSettings.dspTime + 0.1;
        if (calmIntroSource != null && calmIntroSource.clip != null)
        {
            double introLength = (double)calmIntroSource.clip.samples / calmIntroSource.clip.frequency;
            calmIntroSource.PlayScheduled(t);
            calmSource.PlayScheduled(t + introLength);   // el loop entra pegado al inicio
        }
        else calmSource.PlayScheduled(t);
    }

    // ---------- Llamadas públicas ----------
    public void PlayCalm() => Switch(calmSource, calmVol, fadeTime);

    public void PlayEmergency()
    {
        AudioClip stinger = null;
        if (appearanceStingers.Length > 0)
            stinger = appearanceStingers[Mathf.Min(appearances, appearanceStingers.Length - 1)];
        appearances++;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(appearances == 1 ? FirstScare(stinger) : QuickScare(stinger));
    }

    public void PlayUneasy()
    {
        if (current == uneasySource) return;
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(EndChase());
    }

    // ---------- Secuencias ----------
    IEnumerator FirstScare(AudioClip stinger)
    {
        if (stinger) stingerSource.PlayOneShot(stinger);
        yield return Crossfade(null, 0f, 0.15f);                    // corte casi seco
        yield return new WaitForSeconds(silenceAfterFirstStinger);   // silencio
        yield return Crossfade(emergencySource, emergencyVol, fadeTime);
    }

    IEnumerator QuickScare(AudioClip stinger)
    {
        if (stinger) stingerSource.PlayOneShot(stinger);
        yield return Crossfade(emergencySource, emergencyVol, chaseFadeTime);
    }

    IEnumerator EndChase()
    {
        if (current == emergencySource && tensionEnd != null)
        {
            yield return Crossfade(null, 0f, 0.3f);
            emergencySource.volume = emergencyVol;
            emergencySource.PlayOneShot(tensionEnd);
            yield return new WaitForSeconds(Mathf.Max(0f, tensionEnd.length - 0.5f));
        }
        yield return Crossfade(uneasySource, uneasyVol, fadeTime);
    }

    // ---------- Interno ----------
    void Switch(AudioSource next, float vol, float time)
    {
        if (next == current) return;
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Crossfade(next, vol, time));
    }

    IEnumerator Crossfade(AudioSource next, float vol, float time)
    {
        current = next;
        if (next != null && !next.isPlaying) { next.volume = 0f; next.Play(); }

        AudioSource[] all = { calmIntroSource, calmSource, emergencySource, uneasySource };
        float[] start = new float[all.Length];
        for (int i = 0; i < all.Length; i++) start[i] = all[i] ? all[i].volume : 0f;

        for (float t = 0f; t < time; t += Time.deltaTime)
        {
            float k = t / time;
            for (int i = 0; i < all.Length; i++)
                if (all[i]) all[i].volume = Mathf.Lerp(start[i], all[i] == next ? vol : 0f, k);
            yield return null;
        }

        foreach (var s in all)
        {
            if (!s) continue;
            if (s == next) s.volume = vol;
            else { s.volume = 0f; s.Stop(); }
        }
    }
}