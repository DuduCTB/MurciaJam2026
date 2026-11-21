
interface IInteractable 
{
    string Text { get; }

    bool ShouldInteract();

    void Interact();
}
