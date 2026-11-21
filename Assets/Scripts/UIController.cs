using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pickUpText;



    private void OnEnable()
    {
        GameEvents.interactableHovered += UpdatePickUpText;
    }

    private void OnDisable()
    {
        GameEvents.interactableHovered -= UpdatePickUpText;
    }

    private void UpdatePickUpText(string pickUpText)
    {
        this.pickUpText.text = pickUpText;
    }
}
