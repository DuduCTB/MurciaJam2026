using UnityEngine;
using UnityEngine.UI;

public class RageUI : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Color calmColor = Color.white;
    [SerializeField] private Color rageColor = Color.red;
    [SerializeField] private float smoothSpeed = 3f;

    [Header("Rango del multiplicador")]
    [SerializeField] private float minRage = 0.8f; // Valor inicial: 0% rojo
    [SerializeField] private float maxRage = 3.3f; // Valor final: 100% rojo

    private float targetRage01;
    private float currentRage01;

    private void Start()
    {
        // Parte del color calmado según el valor inicial
        targetImage.color = calmColor;
    }

    private void Update()
    {
        currentRage01 = Mathf.MoveTowards(currentRage01, targetRage01, smoothSpeed * Time.deltaTime);
        targetImage.color = Color.Lerp(calmColor, rageColor, currentRage01);
    }

    // Pásale el valor real del multiplicador (0.8, 1.1, 1.6...)
    public void SetRage(float rageValue)
    {
        targetRage01 = Mathf.InverseLerp(minRage, maxRage, rageValue);
    }

}
