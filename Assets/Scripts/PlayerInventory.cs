using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Pickable pickedObject;

    [SerializeField] private GameObject holdMineral, holdGnome;
    [SerializeField] private GameObject currentObject;

    public void UpdatePlayerInventory(Pickable pickedObject)
    {
        this.pickedObject = pickedObject;

        switch (pickedObject.objectID)
        {
            case 0:
                ActivateHoldObject(holdMineral);
                break;
            case 1:
                ActivateHoldObject(holdGnome);
                break;
        }
    }

    public void ActivateHoldObject(GameObject holdObject)
    {
        holdObject.SetActive(true);
        currentObject = holdObject;
    }

    public void DropHoldingObject(Pickable pickedObject)
    {
        if (currentObject != null)
        {
            currentObject.SetActive(false);
            currentObject = null;
        }
    }

}
