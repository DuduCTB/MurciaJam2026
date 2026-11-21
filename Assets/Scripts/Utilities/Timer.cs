using UnityEngine;

public class Timer
{
    private float currentTime = 0;
    private readonly float timeoutTime;
    private bool timeout = false;
    private bool started = false;

    public Timer(float timeout = 1f) { timeoutTime = timeout; }

    /// <summary>
    /// Actualiza el contador. Si está en marcha suma el tiempo del último frame.
    /// </summary>
    public void Update()
    {
        if (!started) return;
        if (currentTime < timeoutTime)  currentTime += Time.deltaTime;
        else                            { timeout = true; started = false; }
    }

    /// <summary>
    /// Comienza el contador.
    /// </summary>
    public void StartTimer() { started = true; }

    /// <summary>
    /// Reinicia el contador. Si 'start' es TRUE, el contador comienza a contar inmediatamente.
    /// </summary>
    /// <param name="start"></param>
    public void Reset(bool start = false) { 
        currentTime = 0; 
        timeout = false;
        started = start;
    }

    /// <summary>
    /// Devuelve el tiempo transcurrido desde el inicio del contador.
    /// </summary>
    public float GetCurrentTime() { return currentTime; }

    /// <summary>
    /// Devuelve el porcentaje de tiempo transcurrido respecto al tiempo total del contador.
    /// </summary>
    public float GetCurrentTimePercentage() { return currentTime/timeoutTime; }

    /// <summary>
    /// Devuelve TRUE si el contador ha terminado.
    /// </summary>
    public bool CheckTimeout() { return timeout; }

    /// <summary>
    /// Devuelve TRUE si el contador está en marcha. No ha terminado.
    /// </summary>
    public bool IsRunning() { return started; }
}
