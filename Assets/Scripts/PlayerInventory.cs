using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool pickedGnome;


    [SerializeField] private GameObject holdMineral, holdGnome;
    [SerializeField] private GameObject currentObject;

    public void UpdatePlayerInventory(Pickable pickedObject)
    {

        switch (pickedObject.objectID)
        {
            case 0:
                ActivateHoldObject(holdMineral);
                break;
            case 1:
                ActivateHoldObject(holdGnome);
                pickedGnome = true;
                break;
        }
    }

    public void ActivateHoldObject(GameObject holdObject)
    {
        holdObject.SetActive(true);
        currentObject = holdObject;
    }

    public void DropHoldingObject()
    {
        if (currentObject != null)
        {
            currentObject.SetActive(false);
            currentObject = null;

            holdMineral.SetActive(false);
            holdGnome.SetActive(false);
        }
    }

}
