using System;
using UnityEngine;

public class TriggerPlaceObjects : TriggerAction
{
    [SerializeField] private GameObject gnome, mineral;

    [SerializeField] private Interactable interactable;

    private void OnEnable()
    {
        GameEvents.interactableHovered += ChangeObjectName;
    }

    private void OnDisable()
    {
        GameEvents.interactableHovered -= ChangeObjectName;
    }

    public override bool ExecuteAction(bool instataneus = false, Action onActionFinished = null)
    {
        if (GameContext.instance.inventory.pickedGnome)
        {
            PlaceItems();
        }
        
        return true;
    }

    public override bool UndoAction(bool instataneus = false, Action onActionFinished = null)
    {
        if (GameContext.instance.inventory.pickedGnome)
        {
            PlaceItems();
        }

        return true;
    }


    private void PlaceItems()
    {
        gnome.SetActive(true);
        mineral.SetActive(true);
        GameContext.instance.inventory.DropHoldingObject();
    }

    private void ChangeObjectName(string name)
    {
        if (GameContext.instance.inventory.pickedGnome)
        {
            interactable.SetName("place Gnome and fire");
        }
        else
        {
            interactable.SetName("needs mineral fuel");
        }
    }
}
