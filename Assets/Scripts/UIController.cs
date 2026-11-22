using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pickUpText;
    [SerializeField] private TextMeshProUGUI objectiveText;



    private void OnEnable()
    {
        GameEvents.interactableHovered += UpdatePickUpText;
        GameEvents.onObjectiveChanged += UpdateObjectiveText;
    }

    private void OnDisable()
    {
        GameEvents.interactableHovered -= UpdatePickUpText;
        GameEvents.onObjectiveChanged -= UpdateObjectiveText;
    }

    private void UpdatePickUpText(string pickUpText)
    {
        this.pickUpText.text = pickUpText;
    }

    private void UpdateObjectiveText(string objectiveText)
    {
        this.objectiveText.text = objectiveText;
    }
}
