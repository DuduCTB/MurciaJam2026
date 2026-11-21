using System;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public string Text
    {
        get { return isBlocked ? UIBlockedText : interactableNameText; }
    }

    [SerializeField] private TriggerAction targetAction;

    [SerializeField] private string interactableNameText = "Switch";
    [SerializeField] private string UIBlockedText = "Bloqueado";

    [SerializeField] private float interactionCooldown = 1f;
    [SerializeField] private bool blockOnActivate = false; // Si es true, el interruptor se bloqueará después de ser activado
    [SerializeField] private bool isActivated = false;
    [SerializeField] private bool isBlocked = false;

    [Header("OPTIONAL")]
    [SerializeField] Animator animator;

    private Timer interactionCooldownTimer = new(1f); // Tiempo de espera entre interacciones

    public event Action Interacted;

    void Start()
    {
        // El timer se crea primero para que esté listo antes de cualquier acción
        interactionCooldownTimer = new(interactionCooldown);
        interactionCooldownTimer.StartTimer();

        if (targetAction == null)
        {
            Debug.LogError("Interactable: No se ha asignado un TriggerTarget al objeto " + gameObject.name);
            isBlocked = true;
            return;
        }

        // Si el interruptor empieza activado, se restaura el estado del objetivo
        // directamente, sin cambiar isActivated ni disparar el evento Interacted
        if (isActivated)
            targetAction.ExecuteAction(true);

        if (animator != null)
            animator.SetBool("Activated", isActivated);

        targetAction.started = isActivated;
    }

    void Update()
    {
        if (interactionCooldownTimer.IsRunning())
            interactionCooldownTimer.Update();
    }

    public void Interact()
    {
        ExecuteAction();
    }

    public bool ShouldInteract()
    {
        return !isBlocked && interactionCooldownTimer.CheckTimeout();
    }

    /// <summary>
    /// Ejecuta o deshace la acción del objetivo y alterna el estado del interruptor.
    /// </summary>
    /// <param name="instantaneus"></param>
    public void ExecuteAction(bool instantaneus = false)
    {
        Debug.Log("Ejecutando accion");
        Interacted?.Invoke();

        if (isActivated)
            targetAction.UndoAction(instantaneus);
        else
            targetAction.ExecuteAction(instantaneus);

        isActivated = !isActivated;
        if (blockOnActivate)
            isBlocked = true;
        interactionCooldownTimer.Reset(true);

        if (animator != null)
            animator.SetBool("Activated", isActivated);
    }

    public void Block(bool blocked) => isBlocked = blocked;

    public bool IsActivated() { return isActivated; }

    public void SetName(string newName)
    {
        interactableNameText = newName;
    }
}
