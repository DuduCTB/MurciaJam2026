using UnityEngine;

public class GameContext : MonoBehaviour
{

    public static GameContext instance;

    public PlayerController player;
    public PlayerInventory inventory;
    public RageUI rageUI;

    [Header("MainSettings")]

    public float mouseSensibility = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


    }


    
   
   
}
