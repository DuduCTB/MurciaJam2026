using UnityEngine;
using System;

public class TriggerAddInventory : TriggerAction
{
    [SerializeField] private Pickable item;

    private Interactable interactable; //Referencia al Script Interactable del mismo objeto

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        if (interactable != null)
            interactable.SetName(item.name);
    }

    public override bool ExecuteAction(bool instataneus = false, Action onActionFinished = null)
    {
        AddItemToInventory(item);
        return true;
    }

    private void AddItemToInventory(Pickable item)
    {
        GameContext.instance.inventory.UpdatePlayerInventory(item);

        Destroy(gameObject);
    }
}
