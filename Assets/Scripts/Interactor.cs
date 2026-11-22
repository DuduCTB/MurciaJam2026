using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactRange;
    [SerializeField] private InputActionReference interact;

    private IInteractable interactObj;
    private bool shouldInteract = false;

    private void OnEnable()
    {
        interact.action.started += Interact;
    }

    private void OnDisable()
    {
        interact.action.started -= Interact;
    }

    void Update()
    {
        DetectIfInteractableInFrontOfPlayer();
    }

    private void OnDrawGizmos()
    {
        // Rayo que detecta si hay interactables frente al jugador
        ////Debug.DrawLine(transform.position, transform.position + (transform.forward * interactRange), Color.blue, 0.01f);
    }

    private void DetectIfInteractableInFrontOfPlayer()
    {
        Ray r = new Ray(transform.position, transform.forward);
        int mask = LayerMask.GetMask("Interactables", "Default", "MistPassable"); // añade aquí las layers de bloqueo que tengas

        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange, mask))
        {
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Interactables")
                && hitInfo.collider.TryGetComponent(out interactObj))
            {
                shouldInteract = interactObj.ShouldInteract();
                if (shouldInteract)
                    GameEvents.interactableHovered.Invoke(interactObj.Text);
            }
            else
                StopInteraction();
        }
        else
            StopInteraction();
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (interactObj != null && shouldInteract)
        {
            Debug.Log("Interaccion llamada");
            interactObj.Interact();
            GameEvents.interactionSuccessful?.Invoke();
        }
    }

    private void StopInteraction()
    {
        // Al dejar de mirar el objeto, se olvida para que no se pueda interactuar "a ciegas"
        interactObj = null;
        shouldInteract = false;

        GameEvents.interactableHovered?.Invoke("");
        GameEvents.interactionCancelled?.Invoke();
    }
}
