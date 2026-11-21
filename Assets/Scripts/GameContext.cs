using UnityEngine;

public class GameContext : MonoBehaviour
{

    public static GameContext instance;

    [Header("MainSettings")]

    public float mouseSensibility = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
